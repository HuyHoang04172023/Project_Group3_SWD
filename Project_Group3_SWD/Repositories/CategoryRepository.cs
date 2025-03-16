using Project_Group3_SWD.Models;
using Microsoft.EntityFrameworkCore;

namespace Project_Group3_SWD.Repositories
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
