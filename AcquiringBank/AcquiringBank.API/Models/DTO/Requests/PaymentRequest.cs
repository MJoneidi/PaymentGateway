using System;
using System.ComponentModel.DataAnnotations;

namespace AcquiringBank.API.Models.DTO.Requests
{
    public class PaymentRequest
    {
        [Required]
        public Guid GatewayPaymentId { get; set; }

        [Required]
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
        public string CardExpiry { get; set; }
    }
}
