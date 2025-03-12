using A_LIÊM_SHOP.Models;

namespace A_LIÊM_SHOP.Repositories
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllCategoryAsync();
    }
}
