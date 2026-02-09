using Banking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Banking.Infrastructure.Data
{
    public class BankingDbContext : DbContext
    {
        public BankingDbContext(DbContextOptions<BankingDbContext> options) : base(options) { }

        public DbSet<Account> Accounts => Set<Account>();
        public DbSet<Transaction> Transactions => Set<Transaction>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.AccountNumber).IsRequired().HasMaxLength(64);
                b.HasIndex(x => x.AccountNumber).IsUnique();

                b.Property(x => x.OwnerName).IsRequired().HasMaxLength(200);

                b.Property(x => x.Balance).HasColumnType("decimal(18,2)");

                // Concurrency token
                b.Property(x => x.RowVersion).IsRowVersion();

                b.HasMany(x => x.Transactions)
                 .WithOne()
                 .HasForeignKey(t => t.AccountId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Transaction>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.Amount).HasColumnType("decimal(18,2)");
                b.Property(x => x.Type).IsRequired();
                b.Property(x => x.Memo).HasMaxLength(500);
            });
        }
    }
}
