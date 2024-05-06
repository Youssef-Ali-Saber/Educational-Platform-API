using Core.Services;
using Core.SignalREntities;
using Microsoft.EntityFrameworkCore;
using Repository.Data;

namespace Utility
{
    public class ChatService(AppDbContext db) : IChatService
    {
        public void AddConnectedUder(string userId, string connectionId)
        {
            db.ConnectedUsers.Add(new ConnectedUser { ConnectionId = connectionId, UserId = userId });
        }

        public List<string> AddToGroups(string userId)
        {
            var user = db.Users.Include("Courses").FirstOrDefault(u=>u.Id==userId);

            foreach (var course in user.Courses)
            {
                var group = db.Groups.FirstOrDefault(g => g.CourseId == course.Id);
                if (group == null)
                {
                    db.Groups.Add(new Group { Name = course.Title, CourseId = course.Id });
                }
            }
            return user.Courses.Select(c => c.Title).ToList();
        }

        public List<string> GetConnections(string userId)
        {
            return db.ConnectedUsers.Where(cu => cu.UserId == userId).Select(cu => cu.ConnectionId).ToList();
        }

        public void RemoveConnectedUser(string connectionId)
        {
            var user = db.ConnectedUsers.FirstOrDefault(cu => cu.ConnectionId == connectionId);
            db.ConnectedUsers.Remove(user);
        }
    }
}
