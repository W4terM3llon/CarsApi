namespace NIS_project.Models.AlterObjectDTOs
{
    public class AlterCarDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public Guid Manufacturer { get; set; } 
        public Guid Engine { get; set; }
        public List<Guid> Owners { get; set; }
    }
}
