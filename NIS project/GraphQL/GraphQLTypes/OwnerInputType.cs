using GraphQL.Types;

namespace NIS_project.GraphQL.GraphQLTypes
{
    public class OwnerInputType : InputObjectGraphType
    {
        public OwnerInputType()
        {
            Field<NonNullGraphType<StringGraphType>>("FirstName");
            Field<NonNullGraphType<StringGraphType>>("LastName");
        }
    }
}
