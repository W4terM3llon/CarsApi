using Microsoft.EntityFrameworkCore;
using NIS_project.Data;
using NIS_project.Models.AlterObjectDTOs;
using System.Collections;

namespace NIS_project.Models.Repositories
{
    public class CarRepository : ICarRepository
    {
        private readonly IDbContextFactory<NIS_projectContext> _contextFactory;
        public CarRepository(IDbContextFactory<NIS_projectContext> contextFactory) {
            _contextFactory = contextFactory;
        }

        public async Task<Car> Create(Car car)
        {
            var context = _contextFactory.CreateDbContext();
            car.Id = Guid.NewGuid();
            if (!await AttachDependenciesFromIds(car, context)) 
            {
                return null;
            }
            await context.Car.AddAsync(car);
            await context.SaveChangesAsync();
            return car;
        }

        public async Task<bool> Delete(Guid carGuid)
        {
            var context = _contextFactory.CreateDbContext();
            var car = context.Car.FirstOrDefault(x => x.Id == carGuid);
            if (car != null)
            {
                context.Car.Remove(car);
                await context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }

        }

        public async Task<IEnumerable> GetAll()
        {
            var context = _contextFactory.CreateDbContext();
            var cars = await context.Car.Include(x => x.Manufacturer).Include(x => x.Engine).ToListAsync();
            await context.SaveChangesAsync();
            return cars;
        }

        public async Task<Car> GetById(Guid id)
        {
            var context = _contextFactory.CreateDbContext();
            var car = await context.Car.Include(x => x.Manufacturer).Include(x => x.Engine).FirstOrDefaultAsync(x => x.Id == id);
            await context.SaveChangesAsync();
            return car;
        }

        public async Task<Car> Update(Car car)
        {
            var context = _contextFactory.CreateDbContext();
            if (!await AttachDependenciesFromIds(car, context))
            {
                return null;
            }
            context.Update(car);
            await context.SaveChangesAsync();
            return car;
        }

        public async Task<bool> IfExists(Guid id)
        {
            var context = _contextFactory.CreateDbContext();
            return await context.Car.AnyAsync(x => x.Id == id);
        }

        public async Task<Car> ConvertAlterDTO(AlterCarDTO carDTO) 
        {
            List<Owner> owners = new List<Owner>();
            if (carDTO.Owners != null)
            {
                foreach (var guid in carDTO.Owners)
                {
                    owners.Add(new Owner() { Id = guid });
                }
            }
            var manufacturer = new Manufacturer() { Id = carDTO.Manufacturer };
            var engine = new Engine() { Id = carDTO.Engine };
            var car = new Car()
            {
                Id = carDTO.Id,
                Price = carDTO.Price,
                Name = carDTO.Name,
                Manufacturer = manufacturer,
                Owners = owners,
                Engine = engine
            };
            return car;
        }

        //DTO object must contain correct Guid id if object already exists or Guid.Empty in other case
        private async Task<bool> AttachDependenciesFromIds(Car car, NIS_projectContext context)
        {
            List<Owner> owners = new List<Owner>();
            foreach (var owner in car.Owners)
            {
                var retrievedOwner = await context.Owner.FirstOrDefaultAsync(x => x.Id == owner.Id);
                if (retrievedOwner == null)
                {
                    return false;
                }
                else
                {
                    owners.Add(owner);
                }
            }
            var manufacturer = await context.Manufacturer.FirstOrDefaultAsync(x => x.Id == car.Manufacturer.Id);
            var engine = await context.Engine.FirstOrDefaultAsync(x => x.Id == car.Engine.Id);
            if (manufacturer == null || engine == null)
            {
                return false;
            }

            car.Owners = owners;
            car.Manufacturer = manufacturer;
            car.Engine = engine;

            return true;
        }
    }
}
