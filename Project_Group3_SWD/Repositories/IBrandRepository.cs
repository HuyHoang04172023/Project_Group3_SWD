using A_LIÊM_SHOP.Models;

namespace A_LIÊM_SHOP.Repositories
{
    public interface IBrandRepository
    {
        Task<List<Brand>> GetAllBrandAsync();
    }
}
