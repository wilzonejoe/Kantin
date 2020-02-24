using Core.Models.Abstracts;
using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public abstract class BaseAccount : ValidationEntity
    {
        [Required]
        [MaxLength(200)]
        public string Fullname { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        [MaxLength(50)]
        public string Password { get; set; }
    }
}
