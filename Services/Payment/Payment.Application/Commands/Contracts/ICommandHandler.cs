using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Application.Commands.Contracts
{
    public interface ICommandHandler<in TCommand> where TCommand : Command
    {
        Task<ICommandResult> Handle(TCommand command);
    }
}
