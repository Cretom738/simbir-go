using System.ComponentModel.DataAnnotations;

namespace Application.Dtos
{
    public class AccountAdminUpdateDto
    {
        [Required(AllowEmptyStrings = false), MaxLength(255)]
        public string Username { get; set; } = null!;
        [Required(AllowEmptyStrings = false), MaxLength(255)]
        public string Password { get; set; } = null!;
        public bool IsAdmin { get; set; }
        [Range(0D, double.MaxValue)]
        public double Balance { get; set; }
    }
}
