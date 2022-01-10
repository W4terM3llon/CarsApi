using Microsoft.EntityFrameworkCore;
using NIS_project.Data;
using NIS_project.Models.AlterObjectDTOs;
using NIS_project.Models.QueryObjectDTOs;
using NIS_project.Services;
using System.Collections;

namespace NIS_project.Models.Repositories
{
    public class CarRepository : ICarRepository
    {
        private readonly IDbContextFactory<NIS_projectContext> _contextFactory;
        private readonly IRedisCacheService _cache;
        public CarRepository(IDbContextFactory<NIS_projectContext> contextFactory, IRedisCacheService cache) {
            _contextFactory = contextFactory;
            _cache = cache;
        }

        public async Task<QueryCarDTO> Create(Car car)
        {
            var context = _contextFactory.CreateDbContext();
            car.Id = Guid.NewGuid();
            if (!await AttachDependenciesFromIds(car, context)) 
            {
                return null;
            }
            await context.Car.AddAsync(car);
            await context.SaveChangesAsync();
            await _cache.SetAsync<QueryCarDTO>(car.Id.ToString(), (QueryCarDTO)car);
            return (QueryCarDTO)car;
        }

        public async Task<bool> Delete(Guid carGuid)
        {
            var context = _contextFactory.CreateDbContext();
            var car = context.Car.FirstOrDefault(x => x.Id == carGuid);
            if (car != null)
            {
                context.Car.Remove(car);
                await context.SaveChangesAsync();
                await _cache.RemoveAsync(car.Id.ToString());
                return true;
            }
            else
            {
                return false;
            }

        }

        public async Task<IEnumerable<QueryCarDTO>> GetAll()
        {
            var context = _contextFactory.CreateDbContext();
            var cars = await context.Car.Include(x => x.Engine).Include(x => x.Owners).Include(x => x.Manufacturer).ToListAsync();
            return cars.Select(x => (QueryCarDTO)x).ToList();
        }

        public async Task<QueryCarDTO> GetById(Guid id)
        {
            var carCache = await _cache.GetAsync<QueryCarDTO>(id.ToString());
            if (carCache != null)
            {
                return carCache;
            }

            var context = _contextFactory.CreateDbContext();
            var car = await context.Car.Include(x => x.Manufacturer).Include(x => x.Engine).FirstOrDefaultAsync(x => x.Id == id);
            await _cache.SetAsync<QueryCarDTO>(car.Id.ToString(), (QueryCarDTO)car);
            return (QueryCarDTO)car;
        }

        public async Task<QueryCarDTO> Update(Car car)
        {
            var context = _contextFactory.CreateDbContext();
            var dbCar = await context.Car.FirstOrDefaultAsync(x => x.Id == car.Id);
            dbCar.Manufacturer = car.Manufacturer;
            dbCar.Name = car.Name;
            dbCar.Engine = car.Engine;
            dbCar.Owners = car.Owners;
            dbCar.Price = car.Price;
            if (!await AttachDependenciesFromIds(dbCar, context))
            {
                return null;
            }
            context.Update(dbCar);
            await context.SaveChangesAsync();
            await _cache.SetAsync<QueryCarDTO>(dbCar.Id.ToString(), (QueryCarDTO)dbCar);
            return (QueryCarDTO)dbCar;
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
