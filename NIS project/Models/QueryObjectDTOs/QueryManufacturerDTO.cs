namespace NIS_project.Models.QueryObjectDTOs
{
    public class QueryManufacturerDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime Since { get; set; }

        public static explicit operator QueryManufacturerDTO(Manufacturer? v)
        {
            if (v == null) return null;

            return new QueryManufacturerDTO()
            {
                Id = v.Id,
                Name = v.Name,
                Since = v.Since,
            };
        }
    }
}
