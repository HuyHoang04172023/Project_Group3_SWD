using Project_Group3_SWD.Models;
using Project_Group3_SWD.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Project_Group3_SWD.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;

        public HomeController(IProductService productService)
        {
            _productService = productService;
        }
 

        public async Task<IActionResult> Index()
        {
            var products = await _productService.top8NewestProduct();
            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
