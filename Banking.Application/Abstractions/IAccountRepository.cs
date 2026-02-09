using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Banking.Domain.Entities;

namespace Banking.Application.Abstractions
{
    public interface IAccountRepository
    {
        Task<Account?> GetByAccountNumberAsync(string accountNumber, CancellationToken ct);
        Task<Account?> GetByIdAsync(Guid id, CancellationToken ct);
        Task<List<Account>> ListAsync(CancellationToken ct);
        Task AddAsync(Account account, CancellationToken ct);
        Task SaveChangesAsync(CancellationToken ct);
    }
}
