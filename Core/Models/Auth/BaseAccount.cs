using Core.Models.Abstracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Models.Auth
{
    public class BaseAccount : ValidationEntity
    {
        [Required]
        [MaxLength(200)]
        public string Fullname { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public bool IsArchived { get; set; }

        public Guid OrganisationId { get; set; }
    }
}
