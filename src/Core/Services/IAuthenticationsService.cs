using Core.Dtos;
using Core.Entities;

namespace Core.Services
{
    public interface IAuthenticationsService
    {
        Task<string> Register(RegisterDto registerDto,string role);
        Task<List<string>> Login(LoginDto loginDto);
        Task<string> ChangePassword(string userId, string oldPassword, string newPassword);
        Task<string> ForgotPassword(string email);
        Task<string> ResetPassword(string email, string token, string newPassword);
        Task<string> CreateJwtToken(AppUser user);
    }
}
