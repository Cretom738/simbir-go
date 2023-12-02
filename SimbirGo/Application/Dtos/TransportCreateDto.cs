using Domain.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos
{
    public class TransportCreateDto
    {
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
        public TransportTypeEnum TransportType { get; set; }
    }
}
