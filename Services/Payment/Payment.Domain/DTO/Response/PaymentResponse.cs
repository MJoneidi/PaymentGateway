using Payment.Domain.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payment.Domain.DTO.Response
{
    public class PaymentResponse
    {
        public Guid TransactionId { get; set; }      
        public BankPaymentStatus PaymentStatus { get; set; }
        public string ErrorDescription { get; set; }
    }
}
