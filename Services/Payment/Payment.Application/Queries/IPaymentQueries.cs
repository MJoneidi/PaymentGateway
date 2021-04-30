using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Application.Queries
{
    public interface IPaymentQueries
    {
        Task<PaymentResponse> GetPaymentAsync(Guid id);
    }
}
