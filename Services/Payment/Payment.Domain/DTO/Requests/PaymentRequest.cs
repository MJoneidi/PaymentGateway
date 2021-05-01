using System;
using System.ComponentModel.DataAnnotations;

namespace Payment.Domain.DTO.Requests
{
    public class PaymentRequest
    {
        [Required]
        public Guid MerchantId { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public double Amount { get; set; }

        [Required]
        [StringLength(3, MinimumLength = 3)]
        public string Currency { get; set; }

        [Required]
        [StringLength(20)]
        public string CardNumber { get; set; }

        [Required]
        public int CardCvv { get; set; }

        [Required]
        [StringLength(5)]
        public string CardExpiry { get; set; }
    }
}
