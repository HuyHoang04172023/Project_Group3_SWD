using A_LIÊM_SHOP.Models;
using A_LIÊM_SHOP.Repositories;

namespace A_LIÊM_SHOP.Services
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
