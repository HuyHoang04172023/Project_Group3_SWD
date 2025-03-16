using Project_Group3_SWD.Models;

namespace Project_Group3_SWD.Services
{
    public interface IBrandService
    {
        public Task<List<Brand>> GetAllBrandAsync();
    }
}
