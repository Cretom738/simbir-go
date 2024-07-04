using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Domain.Enumerations;

namespace Application.Dtos
{
    public class RentAdminCreateDto
    {
        [Range(1, long.MaxValue)]
        public long TransportId { get; set; }
        [JsonPropertyName("userId")]
        [Range(1, long.MaxValue)]
        public long RenterId { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime? TimeEnd { get; set; }
        [Range(0D, double.MaxValue)]
        public double PriceOfUnit { get; set; }
        public RentTypeEnum RentType { get; set; }
        [Range(0D, double.MaxValue)]
        public double? FinalPrice { get; set; }
    }
}
