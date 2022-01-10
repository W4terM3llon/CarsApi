using Microsoft.EntityFrameworkCore;
using NIS_project.Data;
using NIS_project.Models.AlterObjectDTOs;
using NIS_project.Models.QueryObjectDTOs;
using NIS_project.Services;
using System.Collections;

namespace NIS_project.Models.Repositories
{
    public class ManufacturerRepository : IManufacturerRepository
    {
        private readonly IDbContextFactory<NIS_projectContext> _contextFactory;
        private readonly IRedisCacheService _cache;
        public ManufacturerRepository(IDbContextFactory<NIS_projectContext> contextFactory, IRedisCacheService cache)
        {
            _contextFactory = contextFactory;
            _cache = cache;
        }

        public async Task<QueryManufacturerDTO> Create(Manufacturer manufacturer)
        {
            var context = _contextFactory.CreateDbContext();
            manufacturer.Id = Guid.NewGuid();
            if (!await AttachDependenciesFromIds(manufacturer, context))
            {
                return null;
            }
            await context.Manufacturer.AddAsync(manufacturer);
            await context.SaveChangesAsync();
            await _cache.SetAsync<QueryManufacturerDTO>(manufacturer.Id.ToString(), (QueryManufacturerDTO)manufacturer);
            return (QueryManufacturerDTO)manufacturer;
        }

        public async Task<bool> Delete(Guid manufacturerGuid)
        {
            var context = _contextFactory.CreateDbContext();
            var manufacturer = await context.Manufacturer.FirstOrDefaultAsync(x => x.Id == manufacturerGuid);
            if (manufacturer != null)
            {
                context.Manufacturer.Remove(manufacturer);
                await context.SaveChangesAsync();
                await _cache.RemoveAsync(manufacturer.Id.ToString());
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<IEnumerable<QueryManufacturerDTO>> GetAll()
        {
            var context = _contextFactory.CreateDbContext();
            var manufacturers = await context.Manufacturer.ToListAsync();
            return manufacturers.Select(x => (QueryManufacturerDTO)x).ToList();
        }

        public async Task<QueryManufacturerDTO> GetById(Guid id)
        {
            var manufacturerCache = await _cache.GetAsync<QueryManufacturerDTO>(id.ToString());
            if (manufacturerCache != null)
            {
                return manufacturerCache;
            }

            var context = _contextFactory.CreateDbContext();
            var manufacturer = await context.Manufacturer.FirstOrDefaultAsync(x => x.Id == id);
            await _cache.SetAsync<QueryManufacturerDTO>(manufacturer.Id.ToString(), (QueryManufacturerDTO)manufacturer);
            return (QueryManufacturerDTO)manufacturer;
        }

        public async Task<QueryManufacturerDTO> Update(Manufacturer manufacturer)
        {
            var context = _contextFactory.CreateDbContext();
            var dbManufacturer = await context.Manufacturer.FirstOrDefaultAsync(x => x.Id == manufacturer.Id);
            dbManufacturer.Engines = manufacturer.Engines;
            dbManufacturer.Name = manufacturer.Name;
            dbManufacturer.Cars = manufacturer.Cars;
            dbManufacturer.Since = manufacturer.Since;
            if (!await AttachDependenciesFromIds(dbManufacturer, context))
            {
                return null;
            }
            context.Update(dbManufacturer);
            await context.SaveChangesAsync();
            await _cache.SetAsync<QueryManufacturerDTO>(dbManufacturer.Id.ToString(), (QueryManufacturerDTO)dbManufacturer);
            return (QueryManufacturerDTO)dbManufacturer;
        }

        public async Task<bool> IfExists(Guid id) 
        {
            var context = _contextFactory.CreateDbContext();
            return await context.Manufacturer.AnyAsync(x => x.Id == id);
        }

        public async Task<Manufacturer> ConvertAlterDTO(AlterManufacturerDTO manufacturerDTO)
        {
            List<Car> cars = new List<Car>();
            if (manufacturerDTO.Cars != null)
            {
                foreach (var carGuid in manufacturerDTO.Cars)
                {
                    var car = new Car() { Id = carGuid };
                    cars.Add(car);
                }
            }
            List<Engine> engines = new List<Engine>();
            if (manufacturerDTO.Engines != null)
            {
                foreach (var carGuid in manufacturerDTO.Engines)
                {
                    var engine = new Engine() { Id = carGuid };
                    engines.Add(engine);
                }
            }
            var manufacturer = new Manufacturer()
            {
                Id = manufacturerDTO.Id == Guid.Empty ? Guid.NewGuid() : manufacturerDTO.Id,
                Name = manufacturerDTO.Name,
                Since = manufacturerDTO.Since,
                Cars = cars,
                Engines = engines
            };
            return manufacturer;
        }

        //DTO object must contain correct Guid id if object already exists or Guid.Empty in other case
        private async Task<bool> AttachDependenciesFromIds(Manufacturer manufacturer, NIS_projectContext context)
        {
            List<Car> cars = new List<Car>();
            foreach (var car in manufacturer.Cars)
            {
                var retrievedCar = await context.Car.FirstOrDefaultAsync(x => x.Id == car.Id);
                if (retrievedCar == null)
                {
                    return false;
                }
                else
                {
                    cars.Add(retrievedCar);
                }
            }
            List<Engine> engines = new List<Engine>();
            foreach (var engine in manufacturer.Engines)
            {
                var retrievedEngine = await context.Engine.FirstOrDefaultAsync(x => x.Id == engine.Id);
                if (retrievedEngine == null)
                {
                    return false;
                }
                else
                {
                    engines.Add(retrievedEngine);
                }
            }

            manufacturer.Cars = cars;
            manufacturer.Engines = engines;

            return true;
        }
    }
}
