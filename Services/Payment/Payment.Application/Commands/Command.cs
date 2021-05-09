using Payment.Application.Commands.Contracts;
using System;

namespace Payment.Application.Commands
{
    public abstract class Command : IMessage
    {
        public Guid Id { get => new Guid(); }
    }
}
