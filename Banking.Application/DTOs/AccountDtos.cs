using System;

namespace Banking.Application.DTOs
{
    public record AccountSummaryDto(Guid Id, string AccountNumber, string OwnerName, decimal Balance);

    public record CreateAccountRequest(string AccountNumber, string OwnerName, decimal OpeningBalance);

    public record MoneyRequest(decimal Amount, string? Memo);

    public record TransferRequest(string FromAccountNumber, string ToAccountNumber, decimal Amount, string? Memo);
}
