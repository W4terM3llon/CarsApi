using GraphQL;
using GraphQL.Types;
using NIS_project.GraphQL.GraphQLTypes;
using NIS_project.Models;
using NIS_project.Models.AlterObjectDTOs;
using NIS_project.Models.Repositories;

namespace NIS_project.GraphQL.GraphQLQueries
{
    public class AppMutation : ObjectGraphType 
    {
        public AppMutation(ICarRepository carRepository, IManufacturerRepository manufacturerRepository, IEngineRepository engineRepository, IOwnerRepository ownerRepository) {
            FieldAsync<CarType>(
                "CreateCar",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<CarInputType>> { Name = "Car" }),
                resolve: async context => {
                    var carDTO = context.GetArgument<AlterCarDTO>("Car");
                    carDTO.Id = Guid.Empty;
                    var carConverted = await carRepository.ConvertAlterDTO(carDTO);
                    var car = await carRepository.Create(carConverted);
                    if (car == null)
                    {
                        throw new ExecutionError("Car contains not existing dependencies");
                    }
                    return car;
                }
                );
            FieldAsync<CarType>(
                "UpdateCar",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "Id" }, new QueryArgument<NonNullGraphType<CarInputType>> { Name = "Car" }),
                resolve: async context => {
                    var carDTO = context.GetArgument<AlterCarDTO>("Car");
                    carDTO.Id = context.GetArgument<Guid>("Id");
                    if (!await carRepository.IfExists(carDTO.Id)) 
                    {
                        throw new ExecutionError("Car with this id does not exist");
                    }
                    var carConverted = await carRepository.ConvertAlterDTO(carDTO);
                    var car = await carRepository.Update(carConverted);
                    if (car == null)
                    {
                        throw new ExecutionError("Car contains not existing dependencies");
                    }
                    return car;
                }
                );
            FieldAsync<BooleanGraphType>(
                "DeleteCar",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "Id" }),
                resolve: async context => {
                    var carGuid = context.GetArgument<Guid>("Id");
                    if (!await carRepository.Delete(carGuid))
                    {
                        throw new ExecutionError("Engine with this id does not exist");
                    }
                    return true;
                }
                );
            FieldAsync<EngineType>(
                "CreateEngine",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<EngineInputType>> { Name = "Engine" }),
                resolve: async context => {
                    var engineDTO = context.GetArgument<AlterEngineDTO>("Engine");
                    engineDTO.Id = Guid.Empty;
                    var engineConverted = await engineRepository.ConvertAlterDTO(engineDTO);
                    var engine = await engineRepository.Create(engineConverted);
                    if (engine == null)
                    {
                        throw new ExecutionError("Engine contains not existing dependencies");
                    }
                    return engine;
                }
                );
            FieldAsync<EngineType>(
                "UpdatEngine",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "Id" }, new QueryArgument<NonNullGraphType<EngineInputType>> { Name = "Engine" }),
                resolve: async context => {
                    var engineDTO = context.GetArgument<AlterEngineDTO>("Engine");
                    engineDTO.Id = context.GetArgument<Guid>("Id");
                    if (!await engineRepository.IfExists(engineDTO.Id))
                    {
                        throw new ExecutionError("Engine with this id does not exist");
                    }
                    var engineConverted = await engineRepository.ConvertAlterDTO(engineDTO);
                    var engine = await engineRepository.Update(engineConverted);
                    if (engine == null)
                    {
                        throw new ExecutionError("Engine contains not existing dependencies");
                    }
                    return engine;
                }
                );
            FieldAsync<BooleanGraphType>(
                "DeleteEngine",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "Id" }),
                resolve: async context => {
                    var engineGuid = context.GetArgument<Guid>("Id");
                    if (!await engineRepository.Delete(engineGuid))
                    {
                        throw new ExecutionError("Engine with this id does not exist");
                    }
                    return true;
                }
                );
            FieldAsync<ManufacturerType>(
                "CreateManufacturer",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<ManufacturerInputType>> { Name = "Manufacturer" }),
                resolve: async context => {
                    var manufacturerarDTO = context.GetArgument<AlterManufacturerDTO>("Manufacturer");
                    manufacturerarDTO.Id = Guid.Empty;
                    var manufacturerConverted = await manufacturerRepository.ConvertAlterDTO(manufacturerarDTO);
                    var manufacturer = await manufacturerRepository.Create(manufacturerConverted);
                    if (manufacturer == null)
                    {
                        throw new ExecutionError("Manufacturer contains not existing dependencies");
                    }
                    return manufacturer;
                }
                );
            FieldAsync<ManufacturerType>(
                "UpdateManufacturer",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "Id" }, new QueryArgument<NonNullGraphType<ManufacturerInputType>> { Name = "Manufacturer" }),
                resolve: async context => {
                    var manufacturerarDTO = context.GetArgument<AlterManufacturerDTO>("Manufacturer");
                    manufacturerarDTO.Id = context.GetArgument<Guid>("Id");
                    if (!await manufacturerRepository.IfExists(manufacturerarDTO.Id))
                    {
                        throw new ExecutionError("Manufacturer with this id does not exist");
                    }
                    var manufacturerConverted = await manufacturerRepository.ConvertAlterDTO(manufacturerarDTO);
                    var manufacturer = await manufacturerRepository.Update(manufacturerConverted);
                    if (manufacturer == null)
                    {
                        throw new ExecutionError("Manufacturer contains not existing dependencies");
                    }
                    return manufacturer;
                }
                );
            FieldAsync<BooleanGraphType>(
                "DeleteManufacturer",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "Id" }),
                resolve: async context => {
                    var manufacturerGuid = context.GetArgument<Guid>("Id");
                    if (!await manufacturerRepository.Delete(manufacturerGuid))
                    {
                        throw new ExecutionError("Manufacturer with this id does not exist");
                    }
                    return true;
                }
                );
            FieldAsync<OwnerType>(
                "CreateOwner",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<OwnerInputType>> { Name = "Owner" }),
                resolve: async context => {
                    var ownerDTO = context.GetArgument<AlterOwnerDTO>("Owner");
                    ownerDTO.Id = Guid.Empty;
                    var ownerConverted = await ownerRepository.ConvertAlterDTO(ownerDTO);
                    var owner = await ownerRepository.Create(ownerConverted);
                    if (owner == null)
                    {
                        throw new ExecutionError("Owner contains not existing dependencies");
                    }
                    return owner;
                }
                );
            FieldAsync<OwnerType>(
                "UpdateOwner",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "Id" }, new QueryArgument<NonNullGraphType<OwnerInputType>> { Name = "Owner" }),
                resolve: async context => {
                    var ownerDTO = context.GetArgument<AlterOwnerDTO>("Owner");
                    ownerDTO.Id = context.GetArgument<Guid>("Id");
                    if (!await ownerRepository.IfExists(ownerDTO.Id))
                    {
                        throw new ExecutionError("Owner with this id does not exist");
                    }
                    var ownerConverted = await ownerRepository.ConvertAlterDTO(ownerDTO);
                    var owner = await ownerRepository.Update(ownerConverted);
                    if (owner == null)
                    {
                        throw new ExecutionError("Owner contains not existing dependencies");
                    }
                    return owner;
                }
                );
            FieldAsync<BooleanGraphType>(
                "DeleteOwner",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "Id" }),
                resolve: async context => {
                    var ownerGuid = context.GetArgument<Guid>("Id");
                    if (!await ownerRepository.Delete(ownerGuid))
                    {
                        throw new ExecutionError("Owner with this id does not exist");
                    }
                    return true;
                }
                );
        }
    }
}
