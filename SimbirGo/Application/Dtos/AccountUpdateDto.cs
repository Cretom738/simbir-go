using System.ComponentModel.DataAnnotations;

namespace Application.Dtos
{
    public class AccountUpdateDto
    {
        [Required(AllowEmptyStrings = false), MaxLength(255)]
        public string Username { get; set; } = null!;
        [Required(AllowEmptyStrings = false), MaxLength(255)]
        public string Password { get; set; } = null!;
    }
}
