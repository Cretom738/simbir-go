using Domain.Models;
using Infrastructure.Data.Configuration;
using Infrastructure.Data.Configuration.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Rent> Rents { get; set; }
        public DbSet<Transport> Transports { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options) 
        { 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.SetRestrictDeleteBehavior();
            modelBuilder.ApplyConfiguration(new AccountConfiguration());
            modelBuilder.ApplyConfiguration(new AccountRoleConfiguration());
            modelBuilder.ApplyConfiguration(new RentConfiguration());
            modelBuilder.ApplyConfiguration(new RentTypeConfiguration());
            modelBuilder.ApplyConfiguration(new TransportConfiguration());
            modelBuilder.ApplyConfiguration(new TransportTypeConfiguration());
        }
    }
}
