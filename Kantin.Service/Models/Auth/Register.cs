using Core.Models.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Kantin.Service.Models.Auth
{
    public class Register : IValidationObject
    {
        [Required]
        public string Fullname { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
