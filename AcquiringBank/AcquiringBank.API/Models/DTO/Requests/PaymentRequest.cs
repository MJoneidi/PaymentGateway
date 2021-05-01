using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
        public string CardCvv { get; set; }

        [Required]
        public string CardExpiry { get; set; }
    }
}
