var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

// Typed HttpClient for calling the API
builder.Services.AddHttpClient("BankingApi", client =>
{
    client.BaseAddress = new Uri("https://localhost:7001/"); // adjust to match your API launchSettings
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapRazorPages();
app.Run();
