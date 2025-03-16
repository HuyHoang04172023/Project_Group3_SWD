using Project_Group3_SWD.Models;

namespace Project_Group3_SWD.Services
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAllCategoryAsync();
    }
}
