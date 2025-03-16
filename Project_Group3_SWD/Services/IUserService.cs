using Project_Group3_SWD.Models;

namespace Project_Group3_SWD.Services
{
    public interface IUserService
    {
        Task<User> GetUserByEmailAndPassAsync(string email, string password);
        Task<User> GetUserByEmailAsync(string email);

        Task<bool> CreateUserAsync(User user);

	}
}
