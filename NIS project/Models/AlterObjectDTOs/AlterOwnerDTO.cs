namespace NIS_project.Models.AlterObjectDTOs
{
    public class AlterOwnerDTO
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public List<Guid> Cars { get; set; }
    }
}
