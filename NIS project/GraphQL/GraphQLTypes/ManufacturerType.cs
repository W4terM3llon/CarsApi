using GraphQL.Types;
using NIS_project.Models;
using NIS_project.Models.QueryObjectDTOs;

namespace NIS_project.GraphQL.GraphQLTypes
{
    public class ManufacturerType : ObjectGraphType<QueryManufacturerDTO>
    {
        public ManufacturerType()
        {
            Field(x => x.Id, type: typeof(IdGraphType)).Description("Id of the car");
            Field(x => x.Since, type: typeof(DateType)).Description("Since year");
            Field(x => x.Name).Description("Manufacturer of the car");

        }
    }
}
