using NIS_project.Models.AlterObjectDTOs;
using System.Collections;

namespace NIS_project.Models.Repositories
{
    public interface ICarRepository
    {
        public Task<IEnumerable> GetAll();
        public Task<Car> GetById(Guid id);
        public Task<Car> Create(Car car);
        public Task<Car> Update(Car car);
        public Task<bool> Delete(Guid carGuid);
        public Task<bool> IfExists(Guid id);
        public Task<Car> ConvertAlterDTO(AlterCarDTO carDTO);
    }
}
