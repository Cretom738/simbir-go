namespace Application.Dtos
{
    public class AccountDto
    {
        public long Id { get; set; }
        public string Username { get; set; } = null!;
        public double Balance { get; set; }
        public string Role { get; set; } = null!;
    }
}
