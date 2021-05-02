using Payment.Application.Commands.Contracts;
using Payment.Application.Contracts;
using Payment.Domain.DTO.Response;
using Payment.Domain.Entities;
using Payment.Infrastructure.Data.Repositories.Contracts;
using System;
using System.Threading.Tasks;

namespace Payment.Application.Commands
{
    public class PaymentCommandHandler : ICommandHandler<PaymentCommand>
    {
        private readonly IAcquiringBankAdapter _acquiringBankAdapter;
        private readonly IPaymentMethodRepository _paymentMethodRepository;

        public PaymentCommandHandler(IAcquiringBankAdapter acquiringBankAdapter, IPaymentMethodRepository paymentMethodRepository)
        {
            _acquiringBankAdapter = acquiringBankAdapter ?? throw new ArgumentNullException(nameof(acquiringBankAdapter));
            _paymentMethodRepository = paymentMethodRepository ?? throw new ArgumentNullException(nameof(paymentMethodRepository));
        }

        /// <summary>
        /// this method will handle a payment request from validation to send request to bank and then record the result
        /// 
        /// here was lots of idea, such as prevent duplicate http requests by comparing or a key which is mixed of uniq requestId and merchantId, but it has extra cost of reading, so for now I skip it
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<T> Handle<T>(PaymentCommand command) 
        {
            PaymentResponse response = await _acquiringBankAdapter.SendRequestAsync(command);

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
