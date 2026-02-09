using Banking.Application.Services;
using Banking.Infrastructure;
using Banking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// App services
builder.Services.AddScoped<BankingService>();

// Infrastructure (DbContext, repos, UoW)
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Auto-migrate on startup (ok for dev; for prod you usually do it in pipeline)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BankingDbContext>();
    db.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
