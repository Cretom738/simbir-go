namespace Application.Dtos
{
    public class RentDto
    {
        public long Id { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime? TimeEnd { get; set; }
        public double PriceOfUnit { get; set; }
        public double? FinalPrice { get; set; }
        public string RentType { get; set; } = null!;
        public long RenterId { get; set; }
        public long TransportId { get; set; }
    }
}
