using FluentValidation;
using Microsoft.Extensions.Logging;
using Payment.Application.Commands.Contracts;
using Payment.Application.BankAdaptors.Contracts;
using Payment.Domain.Exceptions;
using Payment.Domain.Entities;
using Payment.Infrastructure.Data.Repositories.Contracts;
using System;
using System.Linq;
using System.Threading.Tasks;
using Payment.Application.Validations;

namespace Payment.Application.Commands
{
    public class PaymentCommandHandler : ICommandHandler<PaymentCommand>
    {
        private readonly ILogger<PaymentCommandHandler> _logger;
        private readonly IAcquiringBankAdapter _acquiringBankAdapter;
        private readonly IPaymentMethodRepository _paymentMethodRepository;
   
        public PaymentCommandHandler(ILogger<PaymentCommandHandler> logger,IAcquiringBankAdapter acquiringBankAdapter, IPaymentMethodRepository paymentMethodRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _acquiringBankAdapter = acquiringBankAdapter ?? throw new ArgumentNullException(nameof(acquiringBankAdapter));
            _paymentMethodRepository = paymentMethodRepository ?? throw new ArgumentNullException(nameof(paymentMethodRepository));
        }

        /// <summary>
        /// this method will handle a payment request from validation to send request to bank and then record the result
        /// 
        /// here was lots of idea, such as prevent duplicate http requests by comparing or a key which is mixed of uniq requestId and merchantId, but it has extra cost of reading, so for now I skip it
        /// </summary>
        /// <param name="command"></param>
        /// <returns>PaymentCommandResult</returns>
        public async Task<T> Handle<T>(PaymentCommand command)
        {
            _logger.LogInformation($"----- Sending command: paymentRequestCommand, MerchantId:{command.MerchantId}, Amount:{command.Amount})");

            var validator = new PaymentCommandValidation();
            var failures= validator.Validate(command);
           
            if (!failures.IsValid)
            {
                _logger.LogWarning("Validation errors - PaymentCommand - Command: {@Command} - Errors: {@ValidationErrors}", command, failures.Errors);

                throw new PaymentApiException(string.Join(",", failures.Errors));
            }

            var response = await _acquiringBankAdapter.SendRequestAsync(command);

            var paymentMethod = new PaymentMethod()
            {
                Id = new Guid(),
                AcquiringBankId = command.MerchantId,
                MerchantId = command.MerchantId,
                Amount = command.Amount,
                CardExpiry = command.CardExpiry,
                CardNumber = command.CardNumber,
                CurrencyCode = command.Currency,
                CVV = command.CardCvv,

                TransactionId = response.TransactionId,
                Status = response.PaymentStatus,
                ErrorDescription = response.ErrorDescription
            };

            await _paymentMethodRepository.Add(paymentMethod);

            //need to handle failer 
            var result = new PaymentCommandResult() { Status = paymentMethod.Status, PaymentResultId = paymentMethod.Id, ErrorDescription = paymentMethod.ErrorDescription };

            return (T)Convert.ChangeType(result, typeof(T));
        }
    }
}
