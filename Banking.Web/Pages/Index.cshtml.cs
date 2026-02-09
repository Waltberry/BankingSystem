using System.Net.Http.Json;
//using Banking.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Banking.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _http;

        public IndexModel(IHttpClientFactory http)
        {
            _http = http;
        }

        [BindProperty] public string AccountNumber { get; set; } = "";
        [BindProperty] public string OwnerName { get; set; } = "";
        [BindProperty] public decimal OpeningBalance { get; set; }

        [BindProperty] public decimal Amount { get; set; }
        [BindProperty] public string FromAccountNumber { get; set; } = "";
        [BindProperty] public string ToAccountNumber { get; set; } = "";

        public string Message { get; set; } = "";

        public async Task<IActionResult> OnPostCreate()
        {
            var client = _http.CreateClient("BankingApi");

            var resp = await client.PostAsJsonAsync("api/accounts",
                new CreateAccountRequest(AccountNumber, OwnerName, OpeningBalance));

            Message = resp.IsSuccessStatusCode ? "Account created." : await resp.Content.ReadAsStringAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostDeposit()
        {
            var client = _http.CreateClient("BankingApi");

            var resp = await client.PostAsJsonAsync($"api/accounts/{AccountNumber}/deposit",
                new MoneyRequest(Amount, "Web deposit"));

            Message = resp.IsSuccessStatusCode ? "Deposit successful." : await resp.Content.ReadAsStringAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostTransfer()
        {
            var client = _http.CreateClient("BankingApi");

            var resp = await client.PostAsJsonAsync("api/accounts/transfer",
                new TransferRequest(FromAccountNumber, ToAccountNumber, Amount, "Web transfer"));

            Message = resp.IsSuccessStatusCode ? "Transfer successful." : await resp.Content.ReadAsStringAsync();
            return Page();
        }
    }
}
