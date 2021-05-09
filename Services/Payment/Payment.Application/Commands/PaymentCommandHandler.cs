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
    public class PaymentCommandHandler : ICommandHandler<CreatePaymentCommand>
    {
        private readonly ILogger<PaymentCommandHandler> _logger;       
        private readonly IPaymentMethodRepository _paymentMethodRepository;
   
        public PaymentCommandHandler(ILogger<PaymentCommandHandler> logger, IPaymentMethodRepository paymentMethodRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));           
            _paymentMethodRepository = paymentMethodRepository ?? throw new ArgumentNullException(nameof(paymentMethodRepository));
        }

           
        public async Task Handle(CreatePaymentCommand command)
        {
            _logger.LogInformation($"----- Sending command: paymentRequestCommand, MerchantId:{command.MerchantId}, Amount:{command.Amount})");  
          
            var paymentMethod = new PaymentMethod()
            {
                Id = command.Id,
                AcquiringBankId = command.MerchantId,
                MerchantId = command.MerchantId,
                Amount = command.Amount,
                CardExpiry = command.CardExpiry,
                CardNumber = command.CardNumber,
                CurrencyCode = command.Currency,
                CVV = command.CardCvv,

                TransactionId = command.TransactionId,
                Status = command.PaymentStatus,
                ErrorDescription = command.ErrorDescription
            };

            await _paymentMethodRepository.Add(paymentMethod);          
        }
    }
}
