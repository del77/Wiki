using System;
using System.Threading.Tasks;

namespace Wiki.Infrastructure.Commands
{
    public interface ICommandHandler<T> where T : ICommand
    {
        Task HandleAsync(T command);
    }
}
