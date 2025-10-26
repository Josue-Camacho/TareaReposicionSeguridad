using TareaReposicionSecure.Models.DTOS;

namespace TareaReposicionSecure.Services
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterDto dto);
        Task<bool> VerifyCredentials(LoginDto dto);
        Task<(bool ok, string? token)> LoginAsync(LoginDto dto);
    }
}
