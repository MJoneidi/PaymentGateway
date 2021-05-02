using Microsoft.AspNetCore.Mvc;
using Payment.Domain.Enums;
using System;

namespace Payment.Application.Commands.Contracts
{
    public interface ICommandResult 
    {
      
    }


    public class PaymentCommandResult : ICommandResult
    {
        public Guid PaymentResultId { get; set; }
        public PaymentStatus Status { get; set; }
    }
}
