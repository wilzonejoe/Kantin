using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Models.Abstracts
{
    public abstract class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime CreatedDateUTC { get; set; }
        public DateTime UpdatedDateUTC { get; set; }
    }
}
