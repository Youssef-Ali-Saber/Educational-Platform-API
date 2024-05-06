

using Core.Entities;

namespace Core.Repositories
{
    public interface IChapterRepository : IGenaricRepository<Chapter>
    {
        void Update(Chapter chapter);
    }
}
