using System.ComponentModel.DataAnnotations;

namespace Gira.Data.Entities
{
    public abstract class BaseEntity
    {
        [Required]
        public int Id { get; set; }
    }
}