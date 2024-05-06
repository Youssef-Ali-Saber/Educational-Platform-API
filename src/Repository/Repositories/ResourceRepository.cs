using Core.Entities;
using Core.Repositories;
using Repository.Data;

namespace Repository.Repositories
{
    public class ResourceRepository(AppDbContext db) : GenaricRepository<Resource>(db), IResourceRepository
    {
        public void update(Resource resource)
        {
            db.Resources.Update(resource);
        }
    }
}
