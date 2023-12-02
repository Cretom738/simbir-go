using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configuration
{
    public class TransportConfiguration : IEntityTypeConfiguration<Transport>
    {
        public void Configure(EntityTypeBuilder<Transport> builder)
        {
            builder
                .HasOne(t => t.Owner)
                .WithMany(a => a.OwnedTransports)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
