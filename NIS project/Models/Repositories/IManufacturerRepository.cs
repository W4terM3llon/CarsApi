using NIS_project.Models.AlterObjectDTOs;
using NIS_project.Models.QueryObjectDTOs;
using System.Collections;

namespace NIS_project.Models.Repositories
{
    public interface IManufacturerRepository
    {
        public Task<IEnumerable<QueryManufacturerDTO>> GetAll();
        public Task<QueryManufacturerDTO> GetById(Guid id);
        public Task<QueryManufacturerDTO> Create(Manufacturer manufacture);
        public Task<QueryManufacturerDTO> Update(Manufacturer manufacturer);
        public Task<bool> Delete(Guid manufacturerGuid);
        public Task<bool> IfExists(Guid id);
        public Task<Manufacturer> ConvertAlterDTO(AlterManufacturerDTO manufacturerDTO);
    }
}
