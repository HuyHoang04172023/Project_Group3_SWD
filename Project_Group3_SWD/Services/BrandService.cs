using Project_Group3_SWD.Models;
using Project_Group3_SWD.Repositories;

namespace Project_Group3_SWD.Services
{
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _brandRepository;

        public BrandService(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }
        public async Task<List<Brand>> GetAllBrandAsync()
        {
            return await _brandRepository.GetAllBrandAsync();
        }
    }
}
