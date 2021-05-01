using Payment.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Payment.Infrastructure.Data.Repositories.Contracts
{

    public interface IPaymentMethodRepository : IAsyncRepository<PaymentMethod>
    {
        Task<PaymentMethod> GetById(Guid Id);
    }
}
