using Domain.Enumerations;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configuration
{
    public class RentTypeConfiguration : IEntityTypeConfiguration<RentType>
    {
        public void Configure(EntityTypeBuilder<RentType> builder)
        {
            builder.HasIndex(rt => rt.Name).IsUnique();
            foreach (RentTypeEnum rentTypeEnum in Enum.GetValues(typeof(RentTypeEnum)))
            {
                builder.HasData(new AccountRole
                {
                    Id = (int)rentTypeEnum,
                    Name = rentTypeEnum.ToString()
                });
            }
        }
    }
}
