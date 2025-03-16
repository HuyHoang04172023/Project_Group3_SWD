using Project_Group3_SWD.Models;

namespace Project_Group3_SWD.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserByEmailAndPassAsync(string email, string pass);

		Task<User> GetUserByEmailAsync(string email);

		Task<bool> CreateUserAsync(User user);
	}
}
