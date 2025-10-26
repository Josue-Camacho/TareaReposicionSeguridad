using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TareaReposicionSecure.Models;
using TareaReposicionSecure.Models.DTOS;
using TareaReposicionSecure.Repositories;
using TareaReposicionSecure.Services;

namespace TareaReposicionSecure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _users;
        private readonly IConfiguration _configuration;
        public AuthService(IUserRepository users, IConfiguration configuration)
        {
            _users = users;
            _configuration = configuration;
        }

        public async Task<(bool ok, string? token)> LoginAsync(LoginDto dto)
        {
            var user = await _users.GetByEmailAddress(dto.Email);
            if (user == null) return (false, null);
            var ok = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
            if (!ok) return (false, null);
            var token = GenerateJwtToken(user);
            return (true, token);
        }

        public async Task<string> RegisterAsync(RegisterDto dto)
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            var user = new User
            {
                Email = dto.Email,
                PasswordHash = hashedPassword,
                Username = dto.Username,
                Role = dto.Role,
            };
            await _users.AddAsync(user);
            return user.Id.ToString();
        }


        public async Task<bool> VerifyCredentials(LoginDto dto)
        {
            var user = await _users.GetByEmailAddress(dto.Email);

            return BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
        }

        private string GenerateJwtToken(User user)
        {
            var jwtSection = _configuration.GetSection("Jwt");
            var key = jwtSection["Key"];
            var issuer = jwtSection["Issuer"];
            var audience = jwtSection["Audience"];
            var expireMinutes = int.Parse(jwtSection["ExpiresMinutes"] ?? "60");
            var claims = new List<Claim> {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
            };
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var creds = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
            (
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expireMinutes),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
