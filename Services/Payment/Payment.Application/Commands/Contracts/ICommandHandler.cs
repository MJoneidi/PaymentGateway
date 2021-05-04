using System.Threading.Tasks;

namespace Payment.Application.Commands.Contracts
{
    public interface ICommandHandler<in TCommand> where TCommand : Command
    {
        Task<T> Handle<T>(TCommand command);
    }
}
