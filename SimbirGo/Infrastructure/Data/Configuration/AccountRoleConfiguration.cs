using Domain.Enumerations;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configuration
{
    public class AccountRoleConfiguration : IEntityTypeConfiguration<AccountRole>
    {
        public void Configure(EntityTypeBuilder<AccountRole> builder)
        {
            builder.HasIndex(ar => ar.Name).IsUnique();
            foreach (AccountRoleEnum accountRoleEnum in Enum.GetValues(typeof(AccountRoleEnum)))
            {
                builder.HasData(new AccountRole
                {
                    Id = (int)accountRoleEnum,
                    Name = accountRoleEnum.ToString()
                });
            }
        }
    }
}
