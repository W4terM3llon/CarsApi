using System.ComponentModel.DataAnnotations;

namespace NIS_project.Models
{
    public class Engine
    {
        [Key]
        public int DbId { get; set; }
        public Guid Id { get; set; }
        public string Type { get; set; }
        public Manufacturer Manufacturer { get; set; }
        public int HP { get; set; }
    }
}
