using GraphQL;
using GraphQL.Types;
using NIS_project.GraphQL.GraphQLQueries;
using NIS_project.GraphQL.GraphQLTypes;

namespace NIS_project.GraphQL.GraphQLSchema
{
    public class AppSchema : Schema
    {
        public AppSchema(IServiceProvider provider)
            : base(provider)
        {
            Query = provider.GetRequiredService<AppQuery>();
            Mutation = provider.GetRequiredService<AppMutation>();
        }
    }
}
