using Microsoft.EntityFrameworkCore;
using NIS_project.Data;
using NIS_project.Models.AlterObjectDTOs;
using System.Collections;

namespace NIS_project.Models.Repositories
{
    public class EngineRepository : IEngineRepository
    {
        private readonly IDbContextFactory<NIS_projectContext> _contextFactory;
        public EngineRepository(IDbContextFactory<NIS_projectContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<Engine> Create(Engine engine)
        {
            var context = _contextFactory.CreateDbContext();
            engine.Id = Guid.NewGuid();
            if (!await AttachDependenciesFromIds(engine, context))
            {
                return null;
            }
            Console.WriteLine( " Just checking: " + context.Manufacturer.Any(x => x.DbId == engine.Manufacturer.DbId));
            Console.WriteLine(" Object 2 code: " + context.Manufacturer.FirstOrDefault(x => x.DbId == engine.Manufacturer.DbId).GetHashCode());
            await context.Engine.AddAsync(engine);
            await context.SaveChangesAsync();
            return engine;
        }

        public async Task<bool> Delete(Guid engineGuid)
        {
            var context = _contextFactory.CreateDbContext();
            var engine = await context.Engine.FirstOrDefaultAsync(x => x.Id == engineGuid);
            if (engine != null)
            {
                context.Engine.Remove(engine);
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
            var engines = await context.Engine.ToListAsync();
            await context.SaveChangesAsync();
            return engines;
        }

        public async Task<Engine> GetById(Guid id) 
        {
            var context = _contextFactory.CreateDbContext();
            var engine = await context.Engine.Include(x => x.Manufacturer).FirstOrDefaultAsync(x => x.Id == id);
            await context.SaveChangesAsync();
            return engine;
        }

        public async Task<Engine> Update(Engine engine)
        {
            var context = _contextFactory.CreateDbContext();
            if (!await AttachDependenciesFromIds(engine, context))
            {
                return null;
            }
            context.Update(engine);
            await context.SaveChangesAsync();
            return engine;
        }

        public async Task<bool> IfExists(Guid id)
        {
            var context = _contextFactory.CreateDbContext();
            return await context.Engine.AnyAsync(x => x.Id == id);
        }

        public async Task<Engine> ConvertAlterDTO(AlterEngineDTO engineDTO)
        {
            var manufacturer = new Manufacturer() { Id = engineDTO.Manufacturer };
            var engine = new Engine()
            {
                Id = engineDTO.Id == Guid.Empty ? Guid.NewGuid() : engineDTO.Id,
                Type = engineDTO.Type,
                HP = engineDTO.HP,
                Manufacturer = manufacturer
            };
            return engine;
        }

        //DTO object must contain correct Guid id if object already exists or Guid.Empty in other case
        private async Task<bool> AttachDependenciesFromIds(Engine engine, NIS_projectContext context)
        {
            var manufacturer = await context.Manufacturer.FirstOrDefaultAsync(x => x.Id == engine.Manufacturer.Id);
            Console.WriteLine(" Object 1 code: " + manufacturer.GetHashCode());
            if (manufacturer == null)
            {
                return false;
            }
            engine.Manufacturer = manufacturer;
            return true;
        }
    }
}
