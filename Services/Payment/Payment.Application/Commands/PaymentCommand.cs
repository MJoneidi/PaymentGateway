using Payment.Domain.Enums;
using System;

namespace Payment.Application.Commands
{
    public class CreatePaymentCommand : Command
    {
        public CreatePaymentCommand()
        { }

        public Guid MerchantId { internal get; set; }

        public Guid TransactionId { get; set; }

        public PaymentStatus PaymentStatus { get; set; }
      
        public Guid GatewayPaymentId { get; set; }

        public double Amount { get; set; }


        public string Currency { get; set; }


        public string CardNumber { get; set; }


        public int CardCvv { get; set; }


        public string CardExpiry { get; set; }

        public string ErrorDescription { get; set; }
    }
}
