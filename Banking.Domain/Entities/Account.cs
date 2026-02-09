using System;
using System.Collections.Generic;
using System.Transactions;

namespace Banking.Domain.Entities
{
    public class Account
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string AccountNumber { get; private set; } = default!;
        public string OwnerName { get; private set; } = default!;

        // Stored as decimal to avoid floating point money bugs.
        public decimal Balance { get; private set; }

        // Concurrency token (used by EF Core).
        public byte[] RowVersion { get; private set; } = Array.Empty<byte>();

        private readonly List<Transaction> _transactions = new();
        public IReadOnlyCollection<Transaction> Transactions => _transactions.AsReadOnly();

        private Account() { } // For EF

        public Account(string accountNumber, string ownerName, decimal openingBalance)
        {
            if (string.IsNullOrWhiteSpace(accountNumber)) throw new ArgumentException("Account number is required.");
            if (string.IsNullOrWhiteSpace(ownerName)) throw new ArgumentException("Owner name is required.");
            if (openingBalance < 0) throw new ArgumentException("Opening balance cannot be negative.");

            AccountNumber = accountNumber.Trim();
            OwnerName = ownerName.Trim();
            Balance = openingBalance;

            if (openingBalance > 0)
            {
                _transactions.Add(Transaction.CreateDeposit(Id, openingBalance, "Opening balance"));
            }
        }

        public void Deposit(decimal amount, string? memo = null)
        {
            if (amount <= 0) throw new ArgumentException("Deposit amount must be > 0.");
            Balance += amount;
            _transactions.Add(Transaction.CreateDeposit(Id, amount, memo));
        }

        public void Withdraw(decimal amount, string? memo = null)
        {
            if (amount <= 0) throw new ArgumentException("Withdraw amount must be > 0.");
            if (Balance - amount < 0) throw new InvalidOperationException("Insufficient funds.");

            Balance -= amount;
            _transactions.Add(Transaction.CreateWithdrawal(Id, amount, memo));
        }
    }
}
