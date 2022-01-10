namespace NIS_project.Models.QueryObjectDTOs
{
    public class QueryCarDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public QueryManufacturerDTO Manufacturer { get; set; }
        public QueryEngineDTO Engine { get; set; }

        public static explicit operator QueryCarDTO(Car? v)
        {
            if (v == null) return null;

            return new QueryCarDTO()
            {
                Id = v.Id,
                Name = v.Name,
                Manufacturer = (QueryManufacturerDTO)v.Manufacturer,
                Engine = (QueryEngineDTO)v.Engine
            };
        }
    }
}
