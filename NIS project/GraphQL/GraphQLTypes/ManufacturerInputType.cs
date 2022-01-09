using GraphQL.Types;

namespace NIS_project.GraphQL.GraphQLTypes
{
    public class ManufacturerInputType : InputObjectGraphType
    {
        public ManufacturerInputType()
        {
            Field<NonNullGraphType<StringGraphType>>("Name");
            Field<NonNullGraphType<DateGraphType>>("Since");
        }
    }
}
