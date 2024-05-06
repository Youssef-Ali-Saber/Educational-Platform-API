using Core.Repositories;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using System.Linq.Expressions;

namespace Repository.Repositories
{
    public class GenaricRepository<T>(AppDbContext db) : IGenaricRepository<T> where T : class
    {
        private readonly DbSet<T> _dbSet = db.Set<T>();

        public void Add(T item)
        {
            _dbSet.Add(item);
        }

        public void Delete(int Id)
        {
            var item = _dbSet.Find(Id);
            _dbSet.Remove(item);
        }

        public IEnumerable<T> Get(Expression<Func<T, bool>> filter)
        {
            return _dbSet.Where(filter);
        }

        public IEnumerable<T> GetAll()
        {
            return _dbSet;
        }
    }
}
