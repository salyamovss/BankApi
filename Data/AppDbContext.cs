using BankApi.dal.Models;
using Microsoft.EntityFrameworkCore;

namespace BankApi.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Phone> Phones => Set<Phone>();
    public DbSet<PassportDetail> PassportDetails => Set<PassportDetail>();
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<Card> Cards => Set<Card>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<ExchangeRate> ExchangeRates => Set<ExchangeRate>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema("public");

        // Автоматически заполняем таблицу курсов валют при первой миграции
        modelBuilder.Entity<ExchangeRate>().HasData(
            new ExchangeRate { Id = 1, FromCurrency = "KGS", ToCurrency = "KGS", Rate = 1.0m, UpdatedAt = DateTime.UtcNow },
            new ExchangeRate { Id = 2, FromCurrency = "USD", ToCurrency = "KGS", Rate = 87.50m, UpdatedAt = DateTime.UtcNow },
            new ExchangeRate { Id = 3, FromCurrency = "EUR", ToCurrency = "KGS", Rate = 95.20m, UpdatedAt = DateTime.UtcNow },
            new ExchangeRate { Id = 4, FromCurrency = "RUB", ToCurrency = "KGS", Rate = 0.98m, UpdatedAt = DateTime.UtcNow }
        );
    
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Email).IsRequired().HasMaxLength(255);
            entity.HasIndex(u => u.Email).IsUnique();
            entity.Property(u => u.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(u => u.LastName).IsRequired().HasMaxLength(100);
            entity.Property(u => u.MiddleName).HasMaxLength(100);
        });

        modelBuilder.Entity<PassportDetail>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.HasOne(p => p.User)
                  .WithOne(u => u.Passport)
                  .HasForeignKey<PassportDetail>(p => p.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.Property(p => p.Series).IsRequired().HasMaxLength(10);
            entity.Property(p => p.Number).IsRequired().HasMaxLength(20);
            entity.Property(p => p.IssuedBy).IsRequired().HasMaxLength(255);
        });

        modelBuilder.Entity<Phone>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.HasOne(p => p.User)
                  .WithMany(u => u.Phones)
                  .HasForeignKey(p => p.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.Property(p => p.Number).IsRequired().HasMaxLength(20);
        });

        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(a => a.Id);
            entity.HasOne(a => a.User)
                  .WithMany(u => u.Accounts)
                  .HasForeignKey(a => a.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.Property(a => a.AccountNumber).IsRequired().HasMaxLength(22);
            entity.HasIndex(a => a.AccountNumber).IsUnique();
            entity.Property(a => a.Currency).IsRequired().HasMaxLength(3);
            entity.Property(a => a.Balance).HasPrecision(18, 2);
            entity.Property(a => a.Type).HasConversion<string>().IsRequired();
            entity.Property(a => a.Status).HasConversion<string>().IsRequired();
        });

        modelBuilder.Entity<Card>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.HasOne(c => c.Account)
                  .WithMany(a => a.Cards)
                  .HasForeignKey(c => c.AccountId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.Property(c => c.CardNumber).IsRequired().HasMaxLength(16);
            entity.HasIndex(c => c.CardNumber).IsUnique();
            entity.Property(c => c.Product).HasConversion<string>().IsRequired();
            entity.Property(c => c.PaymentSystem).HasConversion<string>().IsRequired();
            entity.Property(c => c.Status).HasConversion<string>().IsRequired();
            entity.Property(c => c.HolderName).IsRequired().HasMaxLength(150);
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.HasOne(t => t.FromAccount)
                  .WithMany()
                  .HasForeignKey(t => t.FromAccountId)
                  .OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(t => t.ToAccount)
                  .WithMany()
                  .HasForeignKey(t => t.ToAccountId)
                  .OnDelete(DeleteBehavior.NoAction);
            entity.Property(t => t.Amount).HasPrecision(18, 2);
            entity.Property(t => t.ConvertedAmount).HasPrecision(18, 2);
            entity.Property(t => t.ExchangeRate).HasPrecision(18, 6);
            entity.Property(t => t.Currency).IsRequired().HasMaxLength(3);
        });

        modelBuilder.Entity<ExchangeRate>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FromCurrency).IsRequired().HasMaxLength(3);
            entity.Property(e => e.ToCurrency).IsRequired().HasMaxLength(3);
            entity.Property(e => e.Rate).HasPrecision(18, 6);
            entity.HasIndex(e => new { e.FromCurrency, e.ToCurrency }).IsUnique();
        });
    }
    
}