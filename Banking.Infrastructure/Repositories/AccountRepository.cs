using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Banking.Application.Abstractions;
using Banking.Domain.Entities;
using Banking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Banking.Infrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly BankingDbContext _db;

        public AccountRepository(BankingDbContext db)
        {
            _db = db;
        }

        public Task<Account?> GetByAccountNumberAsync(string accountNumber, CancellationToken ct)
            => _db.Accounts
                  .Include(a => a.Transactions)
                  .SingleOrDefaultAsync(a => a.AccountNumber == accountNumber, ct);

        public Task<Account?> GetByIdAsync(Guid id, CancellationToken ct)
            => _db.Accounts
                  .Include(a => a.Transactions)
                  .SingleOrDefaultAsync(a => a.Id == id, ct);

        public Task<List<Account>> ListAsync(CancellationToken ct)
            => _db.Accounts.AsNoTracking().ToListAsync(ct);

        public Task AddAsync(Account account, CancellationToken ct)
            => _db.Accounts.AddAsync(account, ct).AsTask();

        public Task SaveChangesAsync(CancellationToken ct)
            => _db.SaveChangesAsync(ct);
    }
}
