using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Banking.Application.Abstractions;
using Banking.Application.DTOs;
using Banking.Domain.Entities;

namespace Banking.Application.Services
{
    public class BankingService
    {
        private readonly IAccountRepository _accounts;
        private readonly IUnitOfWork _uow;

        public BankingService(IAccountRepository accounts, IUnitOfWork uow)
        {
            _accounts = accounts;
            _uow = uow;
        }

        public async Task<AccountSummaryDto> CreateAccountAsync(CreateAccountRequest req, CancellationToken ct)
        {
            if (req.OpeningBalance < 0) throw new ArgumentException("Opening balance cannot be negative.");

            var existing = await _accounts.GetByAccountNumberAsync(req.AccountNumber, ct);
            if (existing is not null) throw new InvalidOperationException("Account number already exists.");

            var account = new Account(req.AccountNumber, req.OwnerName, req.OpeningBalance);
            await _accounts.AddAsync(account, ct);
            await _accounts.SaveChangesAsync(ct);

            return new AccountSummaryDto(account.Id, account.AccountNumber, account.OwnerName, account.Balance);
        }

        public async Task<AccountSummaryDto> GetAccountAsync(string accountNumber, CancellationToken ct)
        {
            var acc = await _accounts.GetByAccountNumberAsync(accountNumber, ct)
                      ?? throw new InvalidOperationException("Account not found.");

            return new AccountSummaryDto(acc.Id, acc.AccountNumber, acc.OwnerName, acc.Balance);
        }

        public async Task DepositAsync(string accountNumber, MoneyRequest req, CancellationToken ct)
        {
            var acc = await _accounts.GetByAccountNumberAsync(accountNumber, ct)
                      ?? throw new InvalidOperationException("Account not found.");

            acc.Deposit(req.Amount, req.Memo);
            await _accounts.SaveChangesAsync(ct);
        }

        public async Task WithdrawAsync(string accountNumber, MoneyRequest req, CancellationToken ct)
        {
            var acc = await _accounts.GetByAccountNumberAsync(accountNumber, ct)
                      ?? throw new InvalidOperationException("Account not found.");

            acc.Withdraw(req.Amount, req.Memo);
            await _accounts.SaveChangesAsync(ct);
        }

        public async Task TransferAsync(TransferRequest req, CancellationToken ct)
        {
            if (req.Amount <= 0) throw new ArgumentException("Transfer amount must be > 0.");
            if (string.Equals(req.FromAccountNumber, req.ToAccountNumber, StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException("Cannot transfer to the same account.");

            await _uow.ExecuteInTransactionAsync(async () =>
            {
                var from = await _accounts.GetByAccountNumberAsync(req.FromAccountNumber, ct)
                           ?? throw new InvalidOperationException("From account not found.");

                var to = await _accounts.GetByAccountNumberAsync(req.ToAccountNumber, ct)
                         ?? throw new InvalidOperationException("To account not found.");

                // Withdraw first, then deposit (atomic because we're inside a DB transaction)
                from.Withdraw(req.Amount, req.Memo);
                to.Deposit(req.Amount, req.Memo);

                // Add transfer-specific records
                // NOTE: Domain currently records Withdraw/Deposit. We'll add explicit transfer entries too.
                // (You can choose one style; this keeps history clear.)
                // Easiest: add extra transactions as well:
                // but Account exposes private list; so we'd encode in domain if we want strict.
                // We'll keep it simple and rely on withdraw/deposit memos for now.

                await _accounts.SaveChangesAsync(ct);
            }, ct);
        }
    }
}
