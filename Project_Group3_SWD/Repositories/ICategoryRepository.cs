using Project_Group3_SWD.Models;

namespace Project_Group3_SWD.Repositories
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllCategoryAsync();
    }
}
