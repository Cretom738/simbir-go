using Domain.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos
{
    public class RentAdminUpdateDto
    {
        [Range(1, long.MaxValue)]
        public long TransportId { get; set; }
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
