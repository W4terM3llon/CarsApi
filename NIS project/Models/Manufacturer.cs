using System.ComponentModel.DataAnnotations;

namespace NIS_project.Models
{
    public class Manufacturer
    {
        [Key]
        public int DbId { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime Since { get; set; }
        public List<Car> Cars { get; set; }
        public List<Engine> Engines { get; set; }
    }
}
