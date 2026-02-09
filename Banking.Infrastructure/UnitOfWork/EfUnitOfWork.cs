using System;
using System.Threading;
using System.Threading.Tasks;
using Banking.Application.Abstractions;
using Banking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Banking.Infrastructure.UnitOfWork
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private readonly BankingDbContext _db;

        public EfUnitOfWork(BankingDbContext db)
        {
            _db = db;
        }

        public async Task ExecuteInTransactionAsync(Func<Task> action, CancellationToken ct)
        {
            // Works with SQLite (transaction scope supported).
            await using var tx = await _db.Database.BeginTransactionAsync(ct);

            try
            {
                await action();
                await tx.CommitAsync(ct);
            }
            catch
            {
                await tx.RollbackAsync(ct);
                throw;
            }
        }
    }
}
