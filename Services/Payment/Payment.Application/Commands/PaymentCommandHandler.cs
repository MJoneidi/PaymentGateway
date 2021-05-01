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

        public async Task<ICommandResult> Handle(PaymentCommand command)
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

            return this.Success(paymentMethod);
        }
    }
}
