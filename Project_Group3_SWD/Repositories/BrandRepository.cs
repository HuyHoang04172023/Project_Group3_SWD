using Project_Group3_SWD.Models;
using Microsoft.EntityFrameworkCore;

namespace Project_Group3_SWD.Repositories
{
    public class BrandRepository : IBrandRepository
    {
        private readonly WebkinhdoanhquanaoContext _context;

        public BrandRepository(WebkinhdoanhquanaoContext context)
        {
            _context = context;
        }
        public async Task<List<Brand>> GetAllBrandAsync()
        {
            return await _context.Brands.ToListAsync();
        }
    }
}
