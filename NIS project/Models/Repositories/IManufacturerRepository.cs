using NIS_project.Models.AlterObjectDTOs;
using System.Collections;

namespace NIS_project.Models.Repositories
{
    public interface IManufacturerRepository
    {
        public Task<IEnumerable> GetAll();
        public Task<Manufacturer> GetById(Guid id);
        public Task<Manufacturer> Create(Manufacturer manufacture);
        public Task<Manufacturer> Update(Manufacturer manufacturer);
        public Task<bool> Delete(Guid manufacturerGuid);
        public Task<bool> IfExists(Guid id);
        public Task<Manufacturer> ConvertAlterDTO(AlterManufacturerDTO manufacturerDTO);
    }
}
