using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class TransportType
    {
        [Key]
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false), MaxLength(255)]
        public string Name { get; set; } = null!;
    }
}
