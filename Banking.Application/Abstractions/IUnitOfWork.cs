using System;
using System.Threading;
using System.Threading.Tasks;

namespace Banking.Application.Abstractions
{
    public interface IUnitOfWork
    {
        Task ExecuteInTransactionAsync(Func<Task> action, CancellationToken ct);
    }
}
