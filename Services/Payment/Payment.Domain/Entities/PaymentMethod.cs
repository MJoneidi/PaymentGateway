using Payment.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Payment.Domain.Entities
{
    public class PaymentMethod : BaseEntity
    {
        public Guid AcquiringBankId { get; init; }
        public Guid MerchantId { get; init; }
        public Guid TransactionId { get; init; }

        [StringLength(3)]
        public string CurrencyCode { get; init; }

        public double Amount { get; init; }

        [StringLength(5)]
        public string CardExpiry { get; init; }

        public int CVV { get; init; }

        [StringLength(20)]
        public string CardNumber { get; init; }

        public PaymentStatus Status { get; set; }

        [StringLength(500)]
        public string ErrorDescription { get; set; }
    }
}