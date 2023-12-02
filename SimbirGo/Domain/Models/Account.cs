using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class Account
    {
        [Key]
        public long Id { get; set; }
        [Required(AllowEmptyStrings = false), MaxLength(255)]
        public string Username { get; set; } = null!;
        [Required(AllowEmptyStrings = false), MaxLength(255)]
        public string PasswordHash { get; set; } = null!;
        [Range(0D, double.MaxValue)]
        public double Balance { get; set; }
        [Range(1, int.MaxValue)]
        public int AccountRoleId { get; set; }
        [ForeignKey(nameof(AccountRoleId))]
        public AccountRole Role { get; set; } = null!;
        public IEnumerable<Transport> OwnedTransports { get; set; } = new List<Transport>();
        public IEnumerable<Rent> RentedTransports { get; set; } = new List<Rent>();
    }
}
