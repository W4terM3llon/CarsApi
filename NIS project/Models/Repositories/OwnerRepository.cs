using Microsoft.EntityFrameworkCore;
using NIS_project.Data;
using NIS_project.Models.AlterObjectDTOs;
using System.Collections;

namespace NIS_project.Models.Repositories
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly IDbContextFactory<NIS_projectContext> _contextFactory;
        public OwnerRepository(IDbContextFactory<NIS_projectContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<Owner> Create(Owner owner)
        {
            var context = _contextFactory.CreateDbContext();
            owner.Id = Guid.NewGuid();
            if (!await AttachDependenciesFromIds(owner, context))
            {
                return null;
            }
            await context.Owner.AddAsync(owner);
            await context.SaveChangesAsync();
            return owner;
        }

        public async Task<bool> Delete(Guid ownerGuid)
        {
            var context = _contextFactory.CreateDbContext();
            var owner = await context.Owner.FirstOrDefaultAsync(x => x.Id == ownerGuid);
            if (owner != null)
            {
                context.Owner.Remove(owner);
                await context.SaveChangesAsync();
                return true;
            }
            else {
                return false;
            }

        }

        public async Task<IEnumerable> GetAll()
        {
            var context = _contextFactory.CreateDbContext();
            var owners = await context.Owner.ToListAsync();
            await context.SaveChangesAsync();
            return owners;
        }

        public async Task<Owner> GetById(Guid id)
        {
            var context = _contextFactory.CreateDbContext();
            var owner = await context.Owner.FirstOrDefaultAsync(x => x.Id == id);
            await context.SaveChangesAsync();
            return owner;
        }

        public async Task<Owner> Update(Owner owner)
        {
            var context = _contextFactory.CreateDbContext();
            if (!await AttachDependenciesFromIds(owner, context))
            {
                return null;
            }
            context.Update(owner);
            await context.SaveChangesAsync();
            return owner;
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
