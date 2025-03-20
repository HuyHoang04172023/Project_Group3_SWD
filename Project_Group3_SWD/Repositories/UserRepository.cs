using Project_Group3_SWD.Models;
using Microsoft.EntityFrameworkCore;

namespace Project_Group3_SWD.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly WebkinhdoanhquanaoContext _context;

        public UserRepository(WebkinhdoanhquanaoContext context)
        {
            _context = context;
        }
        public async Task<User> GetUserByEmailAndPassAsync(string email, string pass)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.Password == pass);
        }

		public async Task<User> GetUserByEmailAsync(string email)
		{
			return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
		}
		public async Task<bool> CreateUserAsync(User user)
		{
			_context.Users.Add(user);
			await _context.SaveChangesAsync();
			return true;
		}
	}
}
