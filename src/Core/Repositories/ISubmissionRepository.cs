using Core.Entities;

namespace Core.Repositories
{
    public interface ISubmissionRepository : IGenaricRepository<Submission>
    {
        void Update(Submission submission);
    }
}
