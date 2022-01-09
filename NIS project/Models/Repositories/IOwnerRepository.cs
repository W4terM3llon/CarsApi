using NIS_project.Models.AlterObjectDTOs;
using System.Collections;

namespace NIS_project.Models.Repositories
{
    public interface IOwnerRepository
    {
        public Task<IEnumerable> GetAll();
        public Task<Owner> GetById(Guid id);
        public Task<Owner> Create(Owner owner);
        public Task<Owner> Update(Owner owner);
        public Task<bool> Delete(Guid ownerGuid);
        public Task<bool> IfExists(Guid id);
        public Task<Owner> ConvertAlterDTO(AlterOwnerDTO ownerDTO);
    }
}
