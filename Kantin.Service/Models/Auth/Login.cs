using Core.Models.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Kantin.Service.Models.Auth
{
    public class Login : IValidationObject
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
