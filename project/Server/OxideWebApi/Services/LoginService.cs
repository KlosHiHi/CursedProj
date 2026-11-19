using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OxideWebApi.Contexts;
using OxideClassLibrary.DTOs;
using OxideClassLibrary.Models;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OxideWebApi.Services
{
    public interface ILoginService
    {
        Task<string?> AuthUserAsync(LoginRequest loginData);
        Task<User?> RegistrateUser(RegistrationRequest registationData);
    }

    public class LoginService(OxideDbContext context, IConfiguration configuration) : ILoginService
    {
        private readonly OxideDbContext _context = context;

        public async Task<string?> AuthUserAsync(LoginRequest loginData)
        {
            var login = loginData.Login;
            var password = loginData.Password;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == login);

            if (user is null)
                return null;

            user.LastActiveTime = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return VerifyPassword(password, user.Password)
                ? GenerateToken(user.Login, user.UserId)
                : null;
        }
        public async Task<User?> RegistrateUser(RegistrationRequest registationData)
        {
            var user = new User()
            {
                UserId = Guid.NewGuid(),
                Login = registationData.Login,
                Password = await HashPassword(registationData.Password),
                Email = registationData.Email,
            };

            try
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                return user;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        private bool VerifyPassword(string password, string passwordHash)
            => BCrypt.Net.BCrypt.EnhancedVerify(password, passwordHash);

        private async Task<string> HashPassword(string password)
            => BCrypt.Net.BCrypt.EnhancedHashPassword(password);

        private string GenerateToken(string login, Guid id)
        {
            var secretJwtKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(configuration["Jwt:Key"]!));

            var credentials = new SigningCredentials(secretJwtKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim> {
                new(ClaimTypes.Name, login),
                new(ClaimTypes.NameIdentifier, id.ToString()),
            };

            var jwt = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddSeconds(double.Parse(configuration["Jwt:LifeTimeDays"]!)),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
