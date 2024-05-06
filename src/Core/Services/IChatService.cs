using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public interface IChatService
    {
        List<string> AddToGroups(string userId);
        void AddConnectedUder(string userId, string connectionId);
        void RemoveConnectedUser(string connectionId);
        List<string> GetConnections(string userId);

    }
}
