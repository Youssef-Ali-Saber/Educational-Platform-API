using Core.Entities;
using Core.Repositories;
using Repository.Data;

namespace Repository.Repositories
{
    public class CourseRepository(AppDbContext db) : GenaricRepository<Course>(db), ICourseRepository
    {
        public void Update(Course course)
        {
            db.Update(course);
        }
    }
}
