using Payment.Application.Commands;
using Payment.Domain.DTO.Response;
using System.Threading.Tasks;

namespace Payment.Application.BankAdaptors.Contracts
{
    public interface IAcquiringBankAdapter
    {
        Task<FinancialResponse> SendRequestAsync(PaymentCommand request);
    }
}
