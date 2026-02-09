using System;
using System.Threading;
using System.Threading.Tasks;
using Banking.Application.DTOs;
using Banking.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Banking.Api.Controllers
{
    [ApiController]
    [Route("api/accounts")]
    public class AccountsController : ControllerBase
    {
        private readonly BankingService _bank;

        public AccountsController(BankingService bank)
        {
            _bank = bank;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAccountRequest req, CancellationToken ct)
        {
            try
            {
                var created = await _bank.CreateAccountAsync(req, ct);
                return CreatedAtAction(nameof(GetByNumber), new { accountNumber = created.AccountNumber }, created);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{accountNumber}")]
        public async Task<IActionResult> GetByNumber(string accountNumber, CancellationToken ct)
        {
            try
            {
                var dto = await _bank.GetAccountAsync(accountNumber, ct);
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpPost("{accountNumber}/deposit")]
        public async Task<IActionResult> Deposit(string accountNumber, [FromBody] MoneyRequest req, CancellationToken ct)
        {
            try
            {
                await _bank.DepositAsync(accountNumber, req, ct);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("{accountNumber}/withdraw")]
        public async Task<IActionResult> Withdraw(string accountNumber, [FromBody] MoneyRequest req, CancellationToken ct)
        {
            try
            {
                await _bank.WithdrawAsync(accountNumber, req, ct);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("transfer")]
        public async Task<IActionResult> Transfer([FromBody] TransferRequest req, CancellationToken ct)
        {
            try
            {
                await _bank.TransferAsync(req, ct);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
