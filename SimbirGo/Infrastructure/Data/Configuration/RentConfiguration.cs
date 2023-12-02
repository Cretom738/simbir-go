using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configuration
{
    public class RentConfiguration : IEntityTypeConfiguration<Rent>
    {
        public void Configure(EntityTypeBuilder<Rent> builder)
        {
            builder
                .Ignore(r => r.Duration)
                .Ignore(r => r.FinalMinutePrice)
                .Ignore(r => r.FinalDayPrice);
            builder
                .HasOne(r => r.Renter)
                .WithMany(a => a.RentedTransports)
                .OnDelete(DeleteBehavior.Cascade);
            builder
                .HasOne(r => r.Transport)
                .WithMany(t => t.Rents)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
