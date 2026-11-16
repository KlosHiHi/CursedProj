using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OxideClassLibrary.Contexts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OxideWebApi.Services
{
    public class LoginService(OxideDbContext context)
    {
        private readonly OxideDbContext _context = context;

        public async Task<string?> AuthUserAsync(string login, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == login);

            if (user is null)
                return null;

            user.LastActiveTime = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return VerifyPassword(password, user.Password)
                ? GenerateToken(user.Login, user.UserId)
                : null;
        }


        private bool VerifyPassword(string password, string passwordHash)
            => BCrypt.Net.BCrypt.EnhancedVerify(password, passwordHash);

        private string GenerateToken(string login, Guid? id = null, Guid? cartId = null)
        {
            var secretJwtKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(.Configuration["Jwt:Key"]!));

            var credentials = new SigningCredentials(JwtTokenOption.GetKey(), SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, login),
                new Claim(ClaimTypes.NameIdentifier, id.ToString() ?? ""),
                new Claim(ClaimTypes.SerialNumber, cartId.ToString() ?? ""),
            };

            var jwt = new JwtSecurityToken(
                audience: JwtTokenOption.Audience,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromDays(7)),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
