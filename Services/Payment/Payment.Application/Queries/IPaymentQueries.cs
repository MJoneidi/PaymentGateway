using Payment.Domain.DTO.Requests;
using System;
using System.Threading.Tasks;

namespace Payment.Application.Queries
{
    public interface IPaymentQueries
    {
        Task<PaymentResponse> GetPaymentAsync(Guid merchantId, Guid paymentId);
    }
}
