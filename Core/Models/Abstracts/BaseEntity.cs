using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models.Abstracts
{
    public abstract class BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
    }
}
