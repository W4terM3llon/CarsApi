using System.ComponentModel.DataAnnotations;

namespace NIS_project.Models
{
    public class Car
    {
        [Key]
        public int DbId { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public Manufacturer? Manufacturer { get; set; } //nullable to prevent delete cycles
        public Engine Engine { get; set; }
        public List<Owner> Owners { get; set; }
    }
}
