using Payment.Domain.Enums;
using System;

namespace Payment.Domain.DTO.Response
{
    public class PaymentResponse
    {
        public Guid PaymentResultId { get; set; }
        public PaymentStatus Status { get; set; }
        public string ErrorDescription { get; set; }
    }
}
