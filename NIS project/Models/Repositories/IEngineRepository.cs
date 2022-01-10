using NIS_project.Models.AlterObjectDTOs;
using NIS_project.Models.QueryObjectDTOs;
using System.Collections;

namespace NIS_project.Models.Repositories
{
    public interface IEngineRepository
    {
        public Task<IEnumerable<QueryEngineDTO>> GetAll();
        public Task<QueryEngineDTO> GetById(Guid id);
        public Task<QueryEngineDTO> Create(Engine engine);
        public Task<QueryEngineDTO> Update(Engine engine);
        public Task<bool> Delete(Guid engineGuid);
        public Task<bool> IfExists(Guid id);
        public Task<Engine> ConvertAlterDTO(AlterEngineDTO engineDTO);
    }
}
