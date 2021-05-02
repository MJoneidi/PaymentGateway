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

        public PaymentMethod()
        {
            base.CreatedDate = DateTime.Now;
        }
        public PaymentMethod(Guid acquiringBankId, Guid merchantId, string currencyCode, double amount, string cardExpiry, int cvv, string cardNumber): base()
        {
            AcquiringBankId = acquiringBankId != Guid.Empty ? acquiringBankId : throw new ArgumentNullException(nameof(acquiringBankId));
            MerchantId = merchantId != Guid.Empty ? merchantId : throw new ArgumentNullException(nameof(merchantId)); 
            CurrencyCode = !string.IsNullOrWhiteSpace(currencyCode) ? currencyCode : throw new ArgumentNullException(nameof(currencyCode));
            Amount = amount;
            CardExpiry = !string.IsNullOrWhiteSpace(cardExpiry) ? cardExpiry : throw new ArgumentNullException(nameof(cardExpiry)); 
            CVV = cvv;
            CardNumber = !string.IsNullOrWhiteSpace(cardNumber) ? cardNumber : throw new ArgumentNullException(nameof(cardNumber));
        }
    }
}