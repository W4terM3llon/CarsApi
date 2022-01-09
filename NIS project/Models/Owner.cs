using System.ComponentModel.DataAnnotations;

namespace NIS_project.Models
{
    public class Owner
    {
        [Key]
        public int DbId { get; set; }
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public List<Car> Cars { get; set; }
    }
}
