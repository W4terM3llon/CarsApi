using GraphQL.Types;
using NIS_project.Models;

namespace NIS_project.GraphQL.GraphQLTypes
{
    public class ManufacturerType : ObjectGraphType<Manufacturer>
    {
        public ManufacturerType()
        {
            Field(x => x.Id, type: typeof(IdGraphType)).Description("Id of the car");
            Field(x => x.Since, type: typeof(DateType)).Description("Since year");
            Field(x => x.Name).Description("Manufacturer of the car");
            Field(x => x.Engines, type: typeof(ListGraphType<EngineType>)).Description("Manufacturer's engines");
            Field(x => x.Cars, type: typeof(ListGraphType<CarType>)).Description("Manufacturer's cars");

        }
    }
}
