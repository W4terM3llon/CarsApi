using GraphQL.Types;

namespace NIS_project.GraphQL.GraphQLTypes
{
    public class CarInputType : InputObjectGraphType
    {
        public CarInputType()
        {
            Field<NonNullGraphType<StringGraphType>>("Name");
            Field<NonNullGraphType<IdGraphType>>("Manufacturer");
            Field<NonNullGraphType<IdGraphType>>("Engine");
            Field<NonNullGraphType<FloatGraphType>>("Price");
        }
    }
}
