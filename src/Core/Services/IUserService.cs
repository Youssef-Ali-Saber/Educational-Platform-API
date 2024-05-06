

namespace Core.Services
{
    public interface IUserService
    {
        Task<object> GetProfile(string userId);
        Task UpdateProfile(string userId, string? email, string? name, string? phone);
        Task DeleteAccount(string userId);
        object GetChat(string userId, int courseId);

    }
}
