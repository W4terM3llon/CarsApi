using NIS_project.Models.AlterObjectDTOs;
using NIS_project.Models.QueryObjectDTOs;
using System.Collections;

namespace NIS_project.Models.Repositories
{
    public interface ICarRepository
    {
        public Task<IEnumerable<QueryCarDTO>> GetAll();
        public Task<QueryCarDTO> GetById(Guid id);
        public Task<QueryCarDTO> Create(Car car);
        public Task<QueryCarDTO> Update(Car car);
        public Task<bool> Delete(Guid carGuid);
        public Task<bool> IfExists(Guid id);
        public Task<Car> ConvertAlterDTO(AlterCarDTO carDTO);
    }
}
