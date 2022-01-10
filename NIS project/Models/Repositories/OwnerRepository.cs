using Microsoft.EntityFrameworkCore;
using NIS_project.Data;
using NIS_project.Models.AlterObjectDTOs;
using NIS_project.Models.QueryObjectDTOs;
using NIS_project.Services;
using System.Collections;

namespace NIS_project.Models.Repositories
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly IDbContextFactory<NIS_projectContext> _contextFactory;
        private readonly IRedisCacheService _cache;
        public OwnerRepository(IDbContextFactory<NIS_projectContext> contextFactory, IRedisCacheService cache)
        {
            _contextFactory = contextFactory;
            _cache = cache;
        }

        public async Task<QueryOwnerDTO> Create(Owner owner)
        {
            var context = _contextFactory.CreateDbContext();
            owner.Id = Guid.NewGuid();
            if (!await AttachDependenciesFromIds(owner, context))
            {
                return null;
            }
            await context.Owner.AddAsync(owner);
            await context.SaveChangesAsync();
            await _cache.SetAsync<QueryOwnerDTO>(owner.Id.ToString(), (QueryOwnerDTO)owner);
            await _cache.RemoveAsync("AllOwners");
            return (QueryOwnerDTO)owner;
        }

        public async Task<bool> Delete(Guid ownerGuid)
        {
            var context = _contextFactory.CreateDbContext();
            var owner = await context.Owner.FirstOrDefaultAsync(x => x.Id == ownerGuid);
            if (owner != null)
            {
                context.Owner.Remove(owner);
                await context.SaveChangesAsync();
                await _cache.RemoveAsync(owner.Id.ToString());
                await _cache.RemoveAsync("AllOwners");
                return true;
            }
            else {
                return false;
            }

        }

        public async Task<IEnumerable<QueryOwnerDTO>> GetAll()
        {
            var ownerCache = await _cache.GetAsync<IEnumerable<QueryOwnerDTO>>("AllOwners");
            if (ownerCache != null)
            {
                return ownerCache;
            }

            var context = _contextFactory.CreateDbContext();
            var owners = await context.Owner.ToListAsync();
            await context.SaveChangesAsync();
            await _cache.SetAsync<IEnumerable<QueryOwnerDTO>>("AllOwners", owners.Select(x => (QueryOwnerDTO)x).ToList());
            return owners.Select(x => (QueryOwnerDTO)x).ToList();
        }

        public async Task<QueryOwnerDTO> GetById(Guid id)
        {
            var ownerCache = await _cache.GetAsync<QueryOwnerDTO>(id.ToString());
            if (ownerCache != null)
            {
                return ownerCache;
            }

            var context = _contextFactory.CreateDbContext();
            var owner = await context.Owner.FirstOrDefaultAsync(x => x.Id == id);
            await context.SaveChangesAsync();
            await _cache.SetAsync<QueryOwnerDTO>(owner.Id.ToString(), (QueryOwnerDTO)owner);
            return (QueryOwnerDTO)owner;
        }

        public async Task<QueryOwnerDTO> Update(Owner owner)
        {
            var context = _contextFactory.CreateDbContext();
            var dbOwner = await context.Owner.FirstOrDefaultAsync(x => x.Id == owner.Id);
            dbOwner.FirstName = owner.FirstName;
            dbOwner.LastName = owner.LastName;
            dbOwner.Age = owner.Age;
            dbOwner.Cars = owner.Cars;
            if (!await AttachDependenciesFromIds(dbOwner, context))
            {
                return null;
            }
            context.Update(dbOwner);
            await context.SaveChangesAsync();
            await _cache.SetAsync<QueryOwnerDTO>(dbOwner.Id.ToString(), (QueryOwnerDTO)dbOwner);
            await _cache.RemoveAsync("AllOwners");
            return (QueryOwnerDTO)dbOwner;
        }

        public async Task<bool> IfExists(Guid id)
        {
            var context = _contextFactory.CreateDbContext();
            return await context.Owner.AnyAsync(x => x.Id == id);
        }

        public async Task<Owner> ConvertAlterDTO(AlterOwnerDTO ownerDTO)
        {
            List<Car> cars = new List<Car>();
            if (ownerDTO.Cars != null)
            {
                foreach (var carGuid in ownerDTO.Cars)
                {
                    var car = new Car() { Id = carGuid };
                    cars.Add(car);
                }
            }
            var owner = new Owner()
            {
                Id = ownerDTO.Id,
                FirstName = ownerDTO.FirstName,
                LastName = ownerDTO.LastName,
                Age = ownerDTO.Age,
                Cars = cars
            };
            return owner;
        }

        //DTO object must contain correct Guid id if object already exists or Guid.Empty in other case
        private async Task<bool> AttachDependenciesFromIds(Owner owner, NIS_projectContext context)
        {
            List<Car> cars = new List<Car>();
            foreach (var car in owner.Cars)
            {
                var retrievedCar = await context.Car.FirstOrDefaultAsync(x => x.Id == car.Id);
                if (car == null)
                {
                    return false;
                }
                else
                {
                    cars.Add(retrievedCar);
                }
            }
            owner.Cars = cars;
            return true;
        }
    }
}
