using A_LIÊM_SHOP.Models;

namespace A_LIÊM_SHOP.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserByEmailAndPassAsync(string email, string pass);

		Task<User> GetUserByEmailAsync(string email);

		Task<bool> CreateUserAsync(User user);
	}
}
