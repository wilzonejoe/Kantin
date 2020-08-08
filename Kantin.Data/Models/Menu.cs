using Core.Models.Abstracts;
using Core.Models.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Kantin.Data.Models
{
    public class Menu : ValidationEntity, IOrganisationModel
    {
        [Required]
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public bool Available { get; set; }
        public Guid OrganisationId { get; set; }

        [JsonIgnore]
        public Organisation Organisation { get; set; }

        [JsonIgnore]
        public ICollection<MenuItemMenu> MenuItemMenus { get; set; }

        public Menu() : base()
        {
            MenuItemMenus = new List<MenuItemMenu>();
            Available = true;
        }
    }
}
