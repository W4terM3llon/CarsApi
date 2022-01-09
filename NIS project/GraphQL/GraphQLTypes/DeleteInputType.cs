using GraphQL.Types;

namespace NIS_project.GraphQL.GraphQLTypes
{
    public class DeleteInputType : InputObjectGraphType
    {
        public DeleteInputType()
        { 
            Field<NonNullGraphType<IdGraphType>>("Id");
        }
    }
}
