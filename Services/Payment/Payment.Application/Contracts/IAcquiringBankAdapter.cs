using Payment.Application.Commands;
using Payment.Domain.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Application.Contracts
{
    public interface IAcquiringBankAdapter
    {
        Task<PaymentResponse> SendRequestAsync(PaymentCommand request);
    }
}
