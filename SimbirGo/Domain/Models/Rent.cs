using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class Rent
    {
        [Key]
        public long Id { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime? TimeEnd { get; set; }
        [Range(0D, double.MaxValue)]
        public double PriceOfUnit { get; set; }
        [Range(0D, double.MaxValue)]
        public double? FinalPrice { get; set; }
        [Range(1, long.MaxValue)]
        public long RenterId { get; set; }
        [ForeignKey(nameof(RenterId))]
        public Account Renter { get; set; } = null!;
        [Range(1, int.MaxValue)]
        public int RentTypeId { get; set; }
        [ForeignKey(nameof(RentTypeId))]
        public RentType RentType { get; set; } = null!;
        [Range(1, long.MaxValue)]
        public long TransportId { get; set; }
        [ForeignKey(nameof(TransportId))]
        public Transport Transport { get; set; } = null!;
        public TimeSpan Duration => (TimeEnd ?? DateTime.UtcNow) - TimeStart;
        public double FinalMinutePrice => Math.Ceiling(Duration.TotalMinutes) * PriceOfUnit;
        public double FinalDayPrice => Math.Ceiling(Duration.TotalDays) * PriceOfUnit;
    }
}
