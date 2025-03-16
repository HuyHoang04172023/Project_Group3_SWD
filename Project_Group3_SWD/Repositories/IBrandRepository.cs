using Project_Group3_SWD.Models;

namespace Project_Group3_SWD.Repositories
{
    public interface IBrandRepository
    {
        Task<List<Brand>> GetAllBrandAsync();
    }
}
