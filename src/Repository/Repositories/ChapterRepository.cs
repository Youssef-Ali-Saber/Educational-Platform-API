using Core.Entities;
using Core.Repositories;
using Repository.Data;

namespace Repository.Repositories
{
    public class ChapterRepository(AppDbContext db) : GenaricRepository<Chapter>(db), IChapterRepository
    {
        public void Update(Chapter chapter)
        {
            db.Update(chapter);
        }
    }
}
