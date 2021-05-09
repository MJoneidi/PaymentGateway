using Payment.Domain.Enums;
using System;

namespace Payment.Domain.DTO.Response
{
    public class FinancialResponse
    {
        public Guid TransactionId { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string ErrorDescription { get; set; }
    }
}
