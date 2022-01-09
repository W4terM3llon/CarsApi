using GraphQL;
using GraphQL.Types;
using NIS_project.GraphQL.GraphQLTypes;
using NIS_project.Models.Repositories;

namespace NIS_project.GraphQL.GraphQLQueries
{
    public class AppQuery : ObjectGraphType
    {
        public AppQuery(ICarRepository carRepository, IManufacturerRepository manufacturerRepository, IEngineRepository engineRepository, IOwnerRepository ownerRepository)
        {
            FieldAsync<ListGraphType<CarType>>(
               "cars",
               resolve: async context => await carRepository.GetAll()
               );
            FieldAsync<CarType>(
                "car",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" }),
                resolve: async context => await carRepository.GetById(context.GetArgument<Guid>("id"))
                );
            FieldAsync<ListGraphType<ManufacturerType>>(
                "manufacturers",
                resolve: async context => await manufacturerRepository.GetAll()
                );
            FieldAsync<ManufacturerType>(
                "manufacturer",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" }),
                resolve: async context => await manufacturerRepository.GetById(context.GetArgument<Guid>("id"))
                );
            FieldAsync<ListGraphType<EngineType>>(
                "engines",
                resolve: async context => await engineRepository.GetAll()
                );
            FieldAsync<EngineType>(
                "engine",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" }),
                resolve: async context => await engineRepository.GetById(context.GetArgument<Guid>("id"))
                );
            FieldAsync<ListGraphType<OwnerType>>(
                "owners",
                resolve: async context => await ownerRepository.GetAll()
                );
            FieldAsync<OwnerType>(
                "owner",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" }),
                resolve: async context => await ownerRepository.GetById(context.GetArgument<Guid>("id"))
                );
        }
    }
}
