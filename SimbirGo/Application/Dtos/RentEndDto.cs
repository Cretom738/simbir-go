using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.Dtos
{
    public class RentEndDto
    {
        [JsonPropertyName("lat")]
        [Range(-90D, 90D)]
        public double Latitude { get; set; }
        [JsonPropertyName("long")]
        [Range(-180D, 180D)]
        public double Longitude { get; set; }
    }
}
