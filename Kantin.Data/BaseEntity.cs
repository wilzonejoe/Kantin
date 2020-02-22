using System.ComponentModel.DataAnnotations.Schema;

namespace Kantin.Data
{
    public abstract class BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
    }
}
