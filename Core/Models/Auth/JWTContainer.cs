using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Core.Models.Auth
{
    public interface IJWTContainer { }

    public class JWTContainer : IJWTContainer
    {
        public int ExpireMinutes { get; set; } = 10080; // 7 days.
        public string SecurityAlgorithm { get; set; } = SecurityAlgorithms.HmacSha256Signature;
        public Claim[] Claims { get; set; }
    }
}
