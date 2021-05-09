using Payment.Domain.DTO.Requests;
using Payment.Domain.DTO.Response;
using System.Threading.Tasks;

namespace Payment.Application.BankAdaptors.Contracts
{
    public interface IAcquiringBankAdapter
    {
        Task<FinancialResponse> SendRequestAsync(PaymentRequest request);
    }
}
