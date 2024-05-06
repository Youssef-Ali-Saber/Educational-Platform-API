using Core.Entities;
using Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Repository.Data;

namespace Services
{
    public class UserService(UserManager<AppUser> userManager,AppDbContext db) : IUserService
    {
        public object GetChat(string userId, int courseId)
        {
            return new
            {
                myMasseges = db.Chats.Where(m => (m.CourseId == courseId) && (m.UserId == userId)),
                others = db.Chats.Where(m => (m.CourseId == courseId) && (m.UserId != userId))
            };
        }

        public async Task<object> GetProfile(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            return new
            {
                name = user.UserName,
                email = user.Email,
                phoneNumber = user.PhoneNumber,
            };
        }

        public async Task DeleteAccount(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            await userManager.DeleteAsync(user);   
        }

        public async Task UpdateProfile(string userId, string? email, string? name, string? phone)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return;
            }
            if (!email.IsNullOrEmpty())
                user.Email = email;
            if(!name.IsNullOrEmpty())
                user.UserName = name;
            if(!phone.IsNullOrEmpty())
                user.PhoneNumber = phone;
        }
    }
}
