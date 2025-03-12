using A_LIÊM_SHOP.Models;
using A_LIÊM_SHOP.Repositories;
using Microsoft.EntityFrameworkCore;

namespace A_LIÊM_SHOP.Services
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
