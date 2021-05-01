using Payment.Domain.Entities;
using Payment.Infrastructure.Data.Repositories.Contracts;

namespace Payment.Infrastructure.Data.Repositories
{
    public class PaymentMethodRepository : EfRepository<PaymentMethod>, IPaymentMethodRepository
    {
        public PaymentMethodRepository(PaymentDbContext dbContext) : base(dbContext) { }
    }
}