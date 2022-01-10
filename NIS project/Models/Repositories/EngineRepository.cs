using Microsoft.EntityFrameworkCore;
using NIS_project.Data;
using NIS_project.Models.AlterObjectDTOs;
using NIS_project.Models.QueryObjectDTOs;
using NIS_project.Services;
using System.Collections;

namespace NIS_project.Models.Repositories
{
    public class EngineRepository : IEngineRepository
    {
        private readonly IDbContextFactory<NIS_projectContext> _contextFactory;
        private readonly IRedisCacheService _cache;
        public EngineRepository(IDbContextFactory<NIS_projectContext> contextFactory, IRedisCacheService cache)
        {
            _contextFactory = contextFactory;
            _cache = cache;
        }

        public async Task<QueryEngineDTO> Create(Engine engine)
        {
            var context = _contextFactory.CreateDbContext();
            engine.Id = Guid.NewGuid();
            if (!await AttachDependenciesFromIds(engine, context))
            {
                return null;
            }
            await context.Engine.AddAsync(engine);
            await context.SaveChangesAsync();
            await _cache.SetAsync<QueryEngineDTO>(engine.Id.ToString(), (QueryEngineDTO)engine);
            await _cache.RemoveAsync("AllEngines");
            return (QueryEngineDTO)engine;
        }

        public async Task<bool> Delete(Guid engineGuid)
        {
            var context = _contextFactory.CreateDbContext();
            var engine = await context.Engine.FirstOrDefaultAsync(x => x.Id == engineGuid);
            if (engine != null)
            {
                context.Engine.Remove(engine);
                await context.SaveChangesAsync();
                await _cache.RemoveAsync(engine.Id.ToString());
                await _cache.RemoveAsync("AllEngines");
                return true;
            }
            else
            {
                return false;
            }
            

        }

        public async Task<IEnumerable<QueryEngineDTO>> GetAll()
        {
            var enginesCache = await _cache.GetAsync<IEnumerable<QueryEngineDTO>>("AllEngines");
            if (enginesCache != null)
            {
                return enginesCache;
            }

            var context = _contextFactory.CreateDbContext();
            var engines = await context.Engine.ToListAsync();
            await context.SaveChangesAsync();
            await _cache.SetAsync<IEnumerable<QueryEngineDTO>>("AllEngines", engines.Select(x => (QueryEngineDTO)x).ToList());
            return engines.Select(x => (QueryEngineDTO)x).ToList();
        }

        public async Task<QueryEngineDTO> GetById(Guid id) 
        {
            var engineCache = await _cache.GetAsync<QueryEngineDTO>(id.ToString());
            if (engineCache != null)
            {
                return engineCache;
            }

            var context = _contextFactory.CreateDbContext();
            var engine = await context.Engine.Include(x => x.Manufacturer).FirstOrDefaultAsync(x => x.Id == id);
            await context.SaveChangesAsync();
            await _cache.SetAsync<QueryEngineDTO>(engine.Id.ToString(), (QueryEngineDTO)engine);
            return (QueryEngineDTO)engine;
        }

        public async Task<QueryEngineDTO> Update(Engine engine)
        {
            var context = _contextFactory.CreateDbContext();
            var dbEngine = await context.Engine.FirstOrDefaultAsync(x => x.Id == engine.Id);
            dbEngine.Manufacturer = dbEngine.Manufacturer;
            dbEngine.Type = dbEngine.Type;
            dbEngine.HP = dbEngine.HP;
            if (!await AttachDependenciesFromIds(dbEngine, context))
            {
                return null;
            }
            context.Update(dbEngine);
            await context.SaveChangesAsync();
            await _cache.SetAsync<QueryEngineDTO>(dbEngine.Id.ToString(), (QueryEngineDTO)dbEngine);
            await _cache.RemoveAsync("AllEngines");
            return (QueryEngineDTO)dbEngine;
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
