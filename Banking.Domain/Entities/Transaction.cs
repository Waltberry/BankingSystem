using System;

namespace Banking.Domain.Entities
{
    public enum TransactionType
    {
        Deposit = 1,
        Withdrawal = 2,
        TransferOut = 3,
        TransferIn = 4
    }

    public class Transaction
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public Guid AccountId { get; private set; }
        public TransactionType Type { get; private set; }
        public decimal Amount { get; private set; }
        public string? Memo { get; private set; }
        public DateTimeOffset CreatedAtUtc { get; private set; } = DateTimeOffset.UtcNow;

        // Transfer correlation
        public Guid? RelatedAccountId { get; private set; }

        private Transaction() { } // EF

        private Transaction(Guid accountId, TransactionType type, decimal amount, string? memo, Guid? relatedAccountId)
        {
            if (amount <= 0) throw new ArgumentException("Amount must be > 0.");

            AccountId = accountId;
            Type = type;
            Amount = amount;
            Memo = memo;
            RelatedAccountId = relatedAccountId;
        }

        public static Transaction CreateDeposit(Guid accountId, decimal amount, string? memo)
            => new(accountId, TransactionType.Deposit, amount, memo, null);

        public static Transaction CreateWithdrawal(Guid accountId, decimal amount, string? memo)
            => new(accountId, TransactionType.Withdrawal, amount, memo, null);

        public static Transaction CreateTransferOut(Guid accountId, Guid toAccountId, decimal amount, string? memo)
            => new(accountId, TransactionType.TransferOut, amount, memo, toAccountId);

        public static Transaction CreateTransferIn(Guid accountId, Guid fromAccountId, decimal amount, string? memo)
            => new(accountId, TransactionType.TransferIn, amount, memo, fromAccountId);
    }
}
