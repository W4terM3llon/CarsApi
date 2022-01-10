using NIS_project.Models.AlterObjectDTOs;
using NIS_project.Models.QueryObjectDTOs;
using System.Collections;

namespace NIS_project.Models.Repositories
{
    public interface IOwnerRepository
    {
        public Task<IEnumerable<QueryOwnerDTO>> GetAll();
        public Task<QueryOwnerDTO> GetById(Guid id);
        public Task<QueryOwnerDTO> Create(Owner owner);
        public Task<QueryOwnerDTO> Update(Owner owner);
        public Task<bool> Delete(Guid ownerGuid);
        public Task<bool> IfExists(Guid id);
        public Task<Owner> ConvertAlterDTO(AlterOwnerDTO ownerDTO);
    }
}
