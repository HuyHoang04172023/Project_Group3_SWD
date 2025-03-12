using A_LIÊM_SHOP.Models;

namespace A_LIÊM_SHOP.Services
{
    public interface IBrandService
    {
        public Task<List<Brand>> GetAllBrandAsync();
    }
}
