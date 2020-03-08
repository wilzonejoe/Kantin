namespace Kantin.Service.Models.Auth
{
    public class LoginResult
    {
        public bool Success { get; set; }
        public string Token { get; set; }
        public string Scheme { get; set; }
    }
}
