using System;
using System.ComponentModel.DataAnnotations;
using Core.Interface;
using Core.Models.Abstracts;
using Core.Models.Interfaces;
using Newtonsoft.Json;

namespace Kantin.Data.Models
{
    public class MenuItemAttachment : ValidationEntity, IOrganisationModel, IAttachment
    {
        [Required]
        public string FileName { get; set; }

        [Required]
        public Guid OrganisationId { get; set; }

        [Required]
        public Guid MenuItemId { get; set; }

        [JsonIgnore]
        public Organisation Organisation { get; set; }

        [JsonIgnore]
        public MenuItem MenuItem { get; set; }
    }
}
