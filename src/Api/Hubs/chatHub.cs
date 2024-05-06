using Core.Entities;
using Core.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Repository.Data;

namespace Api.Hubs
{
    public class chatHub(AppDbContext db , IChatService chatService) : Hub
    {
        [HubMethodName("sendMessage")]
        public async Task SendMessage(string message, string type)
        {

            var userId = Context.GetHttpContext()?.Request.Headers.Values.FirstOrDefault("userId");
            var courseId = int.Parse(Context.GetHttpContext()?.Request.Headers.Values.FirstOrDefault("courseId"));
            var course = db.Courses.Include(b=>b.AppUsers).FirstOrDefault(c => c.Id == courseId);
            if (course == null)
            {
                return;
            }
            foreach (var user0 in course.AppUsers)
            {
                var connectionIds = chatService.GetConnections(user0.Id);
                foreach (var connectionId in connectionIds)
                {
                    await Clients.Client(connectionId).SendAsync("newMessage", message, type);
                }
            }
            var chat = new Chat { UserId = userId, CourseId = courseId, Message = message, Type = type, Date = DateTime.Now };
            db.Chats.Add(chat);
            await db.SaveChangesAsync();
        }
        public override async Task OnConnectedAsync()
        {
            var userId = Context.GetHttpContext()?.Request.Headers.Values.FirstOrDefault("userId");
            var connectionId = Context.ConnectionId;
            chatService.AddConnectedUder(userId, connectionId);
            var groups = chatService.AddToGroups(userId);
            foreach (var group in groups)
            {
                await Groups.AddToGroupAsync(connectionId, group);
            }
        }
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            chatService.RemoveConnectedUser(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
    }
}
