using Payment.Application.Commands.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Application.Commands
{
    public class SuccessCommandResult<TResult> : SuccessCommandResult
    {
        public SuccessCommandResult(TResult entity)
        {
            Entity = entity;
        }

        public TResult Entity { get; }
    }

    public class SuccessCommandResult : ICommandResult
    {
        public static SuccessCommandResult<TResult> WithResult<TResult>(TResult result)
        {
            return new SuccessCommandResult<TResult>(result);
        }
    }

    public class FailureCommandResult : ICommandResult
    {
        public string Reason { get; }

        public FailureCommandResult(string reason)
        {
            Reason = reason;
        }
    }

    public class InvalidCommandResult : FailureCommandResult
    {
        public InvalidCommandResult(string reason) : base(reason)
        {
        }
    }

    public class EntityConflictCommandResult : FailureCommandResult
    {
        public EntityConflictCommandResult(object id) : base($"Conflict on entity {id}.")
        {
        }
    }
}
