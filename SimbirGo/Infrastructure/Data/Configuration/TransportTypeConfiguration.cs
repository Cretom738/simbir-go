using Domain.Enumerations;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configuration
{
    public class TransportTypeConfiguration : IEntityTypeConfiguration<TransportType>
    {
        public void Configure(EntityTypeBuilder<TransportType> builder)
        {
            builder.HasIndex(tt => tt.Name).IsUnique();
            foreach (TransportTypeEnum transportTypeEnum in Enum.GetValues(typeof(TransportTypeEnum)))
            {
                builder.HasData(new TransportType
                {
                    Id = (int)transportTypeEnum,
                    Name = transportTypeEnum.ToString()
                });
            }
        }
    }
}
