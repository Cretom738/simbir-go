using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Application.Options
{
    public class JwtOptions
    {
        public string Issuer { get; set; } = null!;
        public string Audience { get; set; } = null!;
        public string SigningKey { get; set; } = null!;
        public int SecondsLifeTime { get; set; }
        public SymmetricSecurityKey SecurityKey => new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(SigningKey));
    }
}
