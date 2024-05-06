
using Core.Entities;
using Core.Repositories;
using Repository.Data;

namespace Repository.Repositories
{
    public class AssignmentRepository(AppDbContext db) : GenaricRepository<Assignment>(db), IAssignmentRepository
    {
        public void Update(Assignment assignment)
        {
            db.Update(assignment);
        }
    }
}
