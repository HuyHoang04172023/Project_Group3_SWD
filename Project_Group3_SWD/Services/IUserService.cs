using A_LIÊM_SHOP.Models;

namespace A_LIÊM_SHOP.Services
{
    public interface IUserService
    {
        Task<User> GetUserByEmailAndPassAsync(string email, string password);
        Task<User> GetUserByEmailAsync(string email);

        Task<bool> CreateUserAsync(User user);

	}
}
