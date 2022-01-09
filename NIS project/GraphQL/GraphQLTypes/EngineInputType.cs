using GraphQL.Types;

namespace NIS_project.GraphQL.GraphQLTypes
{
    public class EngineInputType : InputObjectGraphType
    {
        public EngineInputType()
        {
            Field<NonNullGraphType<StringGraphType>>("Type");
            Field<NonNullGraphType<IdGraphType>>("Manufacturer");
            Field<NonNullGraphType<IntGraphType>>("HP");
        }
    }
}
