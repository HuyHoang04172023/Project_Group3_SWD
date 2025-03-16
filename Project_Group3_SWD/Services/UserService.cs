using Project_Group3_SWD.Models;
using Project_Group3_SWD.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Project_Group3_SWD.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

		public UserService(IUserRepository userRepository)
        {
			_userRepository = userRepository;
        }

		public async Task<bool> CreateUserAsync(User user)
		{
			return await _userRepository.CreateUserAsync(user);
		}

		public async Task<User> GetUserByEmailAndPassAsync(string email, string password)
        {
            return await _userRepository.GetUserByEmailAndPassAsync(email, password);
        }

		public async Task<User> GetUserByEmailAsync(string email)
		{
			return await _userRepository.GetUserByEmailAsync(email);
		}

	}
}
