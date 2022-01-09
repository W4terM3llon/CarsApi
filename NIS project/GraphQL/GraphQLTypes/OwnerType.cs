using GraphQL.Types;
using NIS_project.Models;

namespace NIS_project.GraphQL.GraphQLTypes
{
    public class OwnerType : ObjectGraphType<Owner>
    {
        public OwnerType()
        {
            Field(x => x.Id, type: typeof(IdGraphType)).Description("Id of the owner");
            Field(x => x.FirstName).Description("Owner first name");
            Field(x => x.LastName).Description("Owner last name");
            Field(x => x.Cars, type: typeof(ListGraphType<CarType>)).Description("Owner's cars");
        }
    }
}
