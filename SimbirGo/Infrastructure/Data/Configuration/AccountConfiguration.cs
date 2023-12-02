using Domain.Enumerations;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configuration
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.HasIndex(a => a.Username).IsUnique();
            builder.HasData(new Account
            {
                Id = 1,
                Username = "Admin",
                // Password: "Admin"
                PasswordHash = "$2a$13$02pX6t3i/SewMaRWGOVcGuUMxMxZORBRdoEohdIytu.Hc5xknLKX.",
                Balance = 0,
                AccountRoleId = (int)AccountRoleEnum.Admin,
            });
        }
    }
}
