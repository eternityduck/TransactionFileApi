using System;
using DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace DAL
{
    public class ApiContext : IdentityDbContext<User>
    {
        public ApiContext(DbContextOptions<ApiContext> options)
            : base(options)
        {
            Database.EnsureCreated();
            
        }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Transaction>()
                .Property(x => x.Amount)
                .HasColumnType("decimal(18, 3)");
            builder.Entity<Transaction>()
                .Property(e => e.Status)
                .HasConversion(
                    v => v.ToString(),
                    v => (Status)Enum.Parse(typeof(Status), v));
            builder.Entity<Transaction>()
                .Property(e => e.TransactionType)
                .HasConversion(
                    v => v.ToString(),
                    v => (TransactionType)Enum.Parse(typeof(TransactionType), v));
            builder.Entity<Transaction>()
                .Property(x => x.Id)
                .ValueGeneratedNever();

        }
    }
}