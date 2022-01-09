using GraphQL.Types;
using NIS_project.Models;

namespace NIS_project.GraphQL.GraphQLTypes
{
    public class EngineType : ObjectGraphType<Engine>
    {
        public EngineType()
        {
            Field(x => x.Id, type: typeof(IdGraphType)).Description("Id of the car");
            Field(x => x.Type).Description("Since year");
            Field(x => x.Manufacturer, type: typeof(ManufacturerType)).Description("Manufacturer of the car");
            Field(x => x.HP).Description("Horepower of the car");
        }
    }
}
