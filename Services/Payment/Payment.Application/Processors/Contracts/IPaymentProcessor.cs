using Payment.Domain.DTO.Requests;
using Payment.Domain.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Application.Processors.Contracts
{
    public interface IPaymentProcessor
    {
        Task<PaymentResponse> ProcessAsync(PaymentRequest request);
    }
}
