using Core.Entities;
using Core.Repositories;
using Repository.Data;

namespace Repository.Repositories
{
    public class SubmissionRepository(AppDbContext db) : GenaricRepository<Submission>(db), ISubmissionRepository
    {
        public void Update(Submission submission)
        {
            db.Update(submission);
        }
    }
}
