using GraphQL.Types;
using NIS_project.Models;

namespace NIS_project.GraphQL.GraphQLTypes
{
    public class CarType : ObjectGraphType<Car>
    {
        public CarType() {
            Field(x => x.Id).Description("Id of the car");
            Field(x => x.Name).Description("Name of the car");
            Field(x => x.Manufacturer, type: typeof(ManufacturerType)).Description("Manufacturer of the car");
            Field(x => x.Engine, type: typeof(EngineType)).Description("Engine of the car");
            Field(x => x.Price).Description("Price of the car");
        }
    }
}
