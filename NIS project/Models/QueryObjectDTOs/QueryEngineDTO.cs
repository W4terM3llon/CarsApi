namespace NIS_project.Models.QueryObjectDTOs
{
    public class QueryEngineDTO
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public QueryManufacturerDTO Manufacturer { get; set; }
        public int HP { get; set; }

        public static explicit operator QueryEngineDTO(Engine v)
        {
            if (v == null) return null;

            return new QueryEngineDTO()
            {
                Id = v.Id,
                Type = v.Type,
                Manufacturer = (QueryManufacturerDTO)v.Manufacturer,
            };
        }
    }
}
