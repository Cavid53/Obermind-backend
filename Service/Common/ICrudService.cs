using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.Common
{
    public interface ICrudService<TResource> where TResource : IBaseResource
    {
        TResource GetById(int id);
        Task<IEnumerable<TResource>> GetAll();
        Task<UpsertReplyResource> Upsert(TResource resource);
        Task Delete(int id);
    }
}
