using Core.Dtos;
using Core.Entities;
using Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repository.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace Services
{
    public class AuthenticationService(AppDbContext db, UserManager<AppUser> userManager, IConfiguration configuration) : IAuthenticationsService
    {
       
        public async Task<string> Register(RegisterDto registerDto, string role)
        {
            try
            {
                var user0 = await db.Users.FirstOrDefaultAsync(u => u.Email == registerDto.Email);
                if (user0 != null)
                {
                    return "Sorry, this email has already been registered. Please use a different email address.";
                }
                var user = new AppUser
                {
                    Email = registerDto.Email,
                    UserName = registerDto.Email,
                    FullName = registerDto.FullName,
                    PhoneNumber = registerDto.PhoneNumber
                };
                var result = await userManager.CreateAsync(user, registerDto.Password);
                if (!result.Succeeded)
                {
                    return "Sorry, Failed Try Again";
                }
                var result01 = await userManager.AddToRoleAsync(user, role);
                if (!result01.Succeeded)
                {
                    await userManager.DeleteAsync(user);
                    return "Sorry, Failed Try Again";
                }
                return "Success";
            }
            catch (Exception e)
            {
                return $"Sorry, Failed Try Again ({e.Message})";
            }
           
        }

        public async Task<List<string>> Login(LoginDto loginDto)
        {
            try
            {
                var user = await db.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
                if (user is null)
                {
                    return ["Invalid email or password combination. Please check your credentials and try again."];
                }
                var result = await userManager.CheckPasswordAsync(user, loginDto.Password);
                if (!result)
                {
                    return ["Invalid email or password combination. Please check your credentials and try again."];
                }
                return
                    [
                        CreateJwtToken(user).Result,
                        userManager.GetRolesAsync(user).Result[0]
                    ];
            }
            catch(Exception e)
            {
                return [ "500" ,"Sorry, Failed Try Again" , e.Message ];
            }
            
                
        }
         
        public async Task<string> ChangePassword(string userId, string oldPassword, string newPassword)
        {
            var user = await db.Users.FindAsync(userId);
            var result = await userManager.CheckPasswordAsync(user, oldPassword);
            if (!result)
            {
                return "Invalid oldPassword";
            }
            var result0 = await userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            if (!result0.Succeeded)
            {
                return "Failed";
            }
            return "Success";
        }


        public async Task<string> ForgotPassword(string email)
        {
            var user = await db.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user is null)
            {
                return "Sorry, this email is not registered. Please use a different email address.";
            }
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            return token;
        }


        public async Task<string> ResetPassword(string email, string token, string newPassword)
        {
            var user = db.Users.FirstOrDefault(u => u.Email == email);
            if (user is null)
            {
                return "Sorry, this email is not registered. Please use a different email address.";
            }
            var result =  await userManager.ResetPasswordAsync(user, token, newPassword);
            if (!result.Succeeded)
            {
                return "Failed";
            }
            return "Success";
        }







        public async Task<string> CreateJwtToken(AppUser user)
        {
            var userClaims = await userManager.GetClaimsAsync(user);
            var roles = await userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();
            foreach (var role in roles)
                roleClaims.Add(new Claim("role", role));
            var claims = new[]
            {
                new Claim(ClaimTypes.Email,user.Email),
                new Claim (ClaimTypes.NameIdentifier,user.Id),
                new Claim (ClaimTypes.Name,user.UserName)
            }
            .Union(userClaims)
            .Union(roleClaims);
            var secureKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("Jwt")["Key"]));
            var signInCredentials = new SigningCredentials(secureKey, SecurityAlgorithms.HmacSha256);
            var tokenOptions = new JwtSecurityToken(
                issuer: configuration.GetSection("Jwt")["Issuer"],
                audience: configuration.GetSection("Jwt")["Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(5),
                signingCredentials: signInCredentials
                );
            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }
    }
}
