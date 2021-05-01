using AcquiringBank.API.Models.Enums;
using System;

namespace AcquiringBank.API.Models.DTO.Response
{
    public class FinancialResponse
    {
        public Guid TransactionId { get; set; }
        public string ErrorDescription { get; set; }
        public BankPaymentStatus PaymentStatus { get; set; }
    }
}
