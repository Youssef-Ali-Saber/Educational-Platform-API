
using Core.Entities;

namespace Core.Repositories
{
    public interface IAssignmentRepository : IGenaricRepository<Assignment>
    {
        void Update(Assignment assignment);
    }
}
