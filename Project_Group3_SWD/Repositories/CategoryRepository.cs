using A_LIÊM_SHOP.Models;
using Microsoft.EntityFrameworkCore;

namespace A_LIÊM_SHOP.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly WebkinhdoanhquanaoContext _context;

        public CategoryRepository(WebkinhdoanhquanaoContext context)
        {
            _context = context;
        }
        public async Task<List<Category>> GetAllCategoryAsync()
        {
            return await _context.Categories.ToListAsync();
        }
    }
}
