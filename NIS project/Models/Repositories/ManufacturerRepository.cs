using Microsoft.EntityFrameworkCore;
using NIS_project.Data;
using NIS_project.Models.AlterObjectDTOs;
using System.Collections;

namespace NIS_project.Models.Repositories
{
    public class ManufacturerRepository : IManufacturerRepository
    {
        private readonly IDbContextFactory<NIS_projectContext> _contextFactory;
        public ManufacturerRepository(IDbContextFactory<NIS_projectContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<Manufacturer> Create(Manufacturer manufacturer)
        {
            var context = _contextFactory.CreateDbContext();
            manufacturer.Id = Guid.NewGuid();
            if (!await AttachDependenciesFromIds(manufacturer, context))
            {
                return null;
            }
            await context.Manufacturer.AddAsync(manufacturer);
            await context.SaveChangesAsync();
            return manufacturer;
        }

        public async Task<bool> Delete(Guid manufacturerGuid)
        {
            var context = _contextFactory.CreateDbContext();
            var manufacturer = await context.Manufacturer.FirstOrDefaultAsync(x => x.Id == manufacturerGuid);
            if (manufacturer != null)
            {
                context.Manufacturer.Remove(manufacturer);
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
            var manufacturers = await context.Manufacturer.ToListAsync();
            await context.SaveChangesAsync();
            return manufacturers;
        }

        public async Task<Manufacturer> GetById(Guid id)
        {
            var context = _contextFactory.CreateDbContext();
            var manufacturer = await context.Manufacturer.FirstOrDefaultAsync(x => x.Id == id);
            await context.SaveChangesAsync();
            return manufacturer;
        }

        public async Task<Manufacturer> Update(Manufacturer manufacturer)
        {
            var context = _contextFactory.CreateDbContext();
            if (!await AttachDependenciesFromIds(manufacturer, context))
            {
                return null;
            }
            context.Update(manufacturer);
            await context.SaveChangesAsync();
            return manufacturer;
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
