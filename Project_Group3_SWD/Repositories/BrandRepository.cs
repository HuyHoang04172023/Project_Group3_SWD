using A_LIÊM_SHOP.Models;
using Microsoft.EntityFrameworkCore;

namespace A_LIÊM_SHOP.Repositories
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
