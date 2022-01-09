namespace NIS_project.Models.AlterObjectDTOs
{
    public class AlterManufacturerDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime Since { get; set; }
        public List<Guid> Cars { get; set; }
        public List<Guid> Engines { get; set; }
    }
}
