using Core.Models.Abstracts;
using Core.Models.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Models.Auth
{
    public class BaseAccount : ValidationEntity, IOrganisationModel
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
