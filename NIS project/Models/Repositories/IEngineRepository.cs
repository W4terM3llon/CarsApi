using NIS_project.Models.AlterObjectDTOs;
using System.Collections;

namespace NIS_project.Models.Repositories
{
    public interface IEngineRepository
    {
        public Task<IEnumerable> GetAll();
        public Task<Engine> GetById(Guid id);
        public Task<Engine> Create(Engine engine);
        public Task<Engine> Update(Engine engine);
        public Task<bool> Delete(Guid engineGuid);
        public Task<bool> IfExists(Guid id);
        public Task<Engine> ConvertAlterDTO(AlterEngineDTO engineDTO);
    }
}
