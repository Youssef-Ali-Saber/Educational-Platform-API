using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public interface IResourceRepository : IGenaricRepository<Resource>
    {
        void update(Resource resource);
    }
}
