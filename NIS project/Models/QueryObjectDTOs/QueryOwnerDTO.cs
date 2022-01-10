namespace NIS_project.Models.QueryObjectDTOs
{
    public class QueryOwnerDTO
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public List<QueryCarDTO> Cars { get; set; }

        public static explicit operator QueryOwnerDTO(Owner v)
        {
            if (v == null) return null;

            return new QueryOwnerDTO()
            {
                Id = v.Id,
                FirstName = v.FirstName,
                LastName = v.LastName,
                Age = v.Age,
                Cars = v.Cars.Select(x => (QueryCarDTO)x).ToList(),
            };
        }
    }
}
