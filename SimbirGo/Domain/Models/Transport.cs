using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class Transport
    {
        [Key]
        public long Id { get; set; }
        public bool CanBeRented { get; set; }
        [Required(AllowEmptyStrings = false), MaxLength(255)]
        public string Model { get; set; } = null!;
        [Required(AllowEmptyStrings = false), MaxLength(255)]
        public string Color { get; set; } = null!;
        [Required(AllowEmptyStrings = false), MaxLength(255)]
        public string Identifier { get; set; } = null!;
        [MaxLength(500)]
        public string? Description { get; set; }
        [Range(-90D, 90D)]
        public double Latitude { get; set; }
        [Range(-180D, 180D)]
        public double Longitude { get; set; }
        [Range(0D, double.MaxValue)]
        public double? MinutePrice { get; set; }
        [Range(0D, double.MaxValue)]
        public double? DayPrice { get; set; }
        [Range(1, int.MaxValue)]
        public int TransportTypeId { get; set; }
        [ForeignKey(nameof(TransportTypeId))]
        public TransportType TransportType { get; set; } = null!;
        [Range(1, long.MaxValue)]
        public long OwnerId { get; set; }
        [ForeignKey(nameof(OwnerId))]
        public Account Owner { get; set; } = null!;
        public IEnumerable<Rent> Rents { get; set; } = new List<Rent>();
    }
}
