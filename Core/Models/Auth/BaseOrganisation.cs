using Core.Models.Abstracts;
using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Models.Auth
{
    public class BaseOrganisation : ValidationEntity
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        [Required]
        public DateTime ExpiryDateUTC { get; set; }
    }
}
