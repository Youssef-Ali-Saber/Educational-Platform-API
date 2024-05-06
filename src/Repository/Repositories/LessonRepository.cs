using Core.Entities;
using Core.Repositories;
using Repository.Data;

namespace Repository.Repositories
{
    public class LessonRepository(AppDbContext db) : GenaricRepository<Lesson>(db), ILessonRepository
    {
        public void Update(Lesson lesson)
        {
            db.Update(lesson);
        }
    }
}
