using Microsoft.AspNetCore.Mvc;
using Payment.Domain.Enums;
using System;

namespace Payment.Application.Commands.Contracts
{
    public class PaymentCommandResult 
    {
        public Guid PaymentResultId { get; set; }
        public PaymentStatus Status { get; set; }
        public string ErrorDescription { get; set; }
    }
}
