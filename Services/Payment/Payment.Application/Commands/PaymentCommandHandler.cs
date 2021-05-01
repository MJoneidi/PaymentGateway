using Payment.Application.Commands.Contracts;
using Payment.Application.Contracts;
using Payment.Domain.DTO.Response;
using Payment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Application.Commands
{
    public class PaymentCommandHandler : ICommandHandler<PaymentCommand>
    {
        private readonly IAcquiringBankAdapter _acquiringBankAdapter;
        public PaymentCommandHandler(IAcquiringBankAdapter acquiringBankAdapter)
        {
            _acquiringBankAdapter = acquiringBankAdapter;
        }

        public async Task<ICommandResult> Handle(PaymentCommand command)
        {

            PaymentResponse response = await _acquiringBankAdapter.SendRequestAsync(command);

            var paymentMethod = new PaymentMethod()
            {

            };

            return this.Success(paymentMethod);
        }
    }
}
