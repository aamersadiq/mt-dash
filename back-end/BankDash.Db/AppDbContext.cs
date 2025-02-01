using BankDash.Common;
using BankDash.Model.Enitity;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserRole>()
            .HasKey(ur => new { ur.UserId, ur.RoleId });

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId);

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId);

        modelBuilder.Entity<Account>()
            .HasOne(a => a.User)
            .WithMany(u => u.Accounts)
            .HasForeignKey(a => a.UserId);

        //modelBuilder.Entity<Transaction>()
        //    .HasOne(t => t.Account)
        //    .WithMany(a => a.Transactions)
        //    .HasForeignKey(t => t.AccountId);

        // Seed Roles
        var accountRead = new Role { Id = 1, Name = RoleType.AccountRead.ToString() };
        var accountManage = new Role { Id = 2, Name = RoleType.AccountManage.ToString() };

        modelBuilder.Entity<Role>()
            .HasData(accountRead, accountManage);

        // Seed Users
        var accountHolderUser = new User
        {
            Id = 1,
            Username = "account-holder",
            PasswordHash = PasswordHelper.HashPassword("account-holder@123")
        };

        modelBuilder.Entity<User>()
            .HasData(accountHolderUser);

        // Seed UserRoles
        modelBuilder.Entity<UserRole>()
            .HasData(
                new UserRole { UserId = 1, RoleId = 1 }, // with accountRead and AccountManage role
                new UserRole { UserId = 1, RoleId = 2 }
            );

        modelBuilder.Entity<Account>()
            .HasData(
                new Account { Id = 1, UserId = 1, Balance = 10000 },
                new Account { Id = 2, UserId = 1, Balance = 5000 }
            );

        modelBuilder.Entity<Transaction>()
            .HasData(
                new Transaction { Id = 1, AccountId = 1, Type = TransactionType.Credit.ToString(), Amount = 1000, TransactionDate = DateTime.UtcNow },
                new Transaction { Id = 2, AccountId = 2, Type = TransactionType.Debit.ToString(), Amount = 1000, TransactionDate = DateTime.UtcNow }
            );
    }
}