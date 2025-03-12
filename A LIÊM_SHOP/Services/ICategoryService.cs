using A_LIÊM_SHOP.Models;

namespace A_LIÊM_SHOP.Services
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAllCategoryAsync();
    }
}
