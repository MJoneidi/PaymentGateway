﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Payment.Application.Commands
{    
    public class PaymentCommand : Command
    {
        public PaymentCommand()
        { }

        public Guid MerchantId { internal get; set; }

        
        public Guid GatewayPaymentId { get; set; }

        public double Amount { get; set; }

       
        public string Currency { get; set; }

      
        public string CardNumber { get; set; }

        
        public int CardCvv { get; set; }

        
        public string CardExpiry { get; set; }
    }
}
