using System.Threading.Tasks;

namespace Payment.Application.Commands.Contracts
{
    public interface ICommandHandler<in TCommand> where TCommand : Command
    {
        Task<ICommandResult> Handle(TCommand command);
    }
}
