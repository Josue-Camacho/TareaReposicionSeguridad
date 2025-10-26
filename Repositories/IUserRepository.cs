using TareaReposicionSecure.Models;

namespace TareaReposicionSecure.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetByEmailAddress(string emailAddress);
        Task AddAsync(User user);
    }
}
