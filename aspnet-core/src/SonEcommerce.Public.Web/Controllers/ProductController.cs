using Microsoft.AspNetCore.Mvc;
using SonEcommerce.Public.Products;
using System.Threading.Tasks;

namespace SonEcommerce.Public.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductsAppService _productAppService;
        public ProductController(ProductsAppService productAppService)
        {
            _productAppService = productAppService;
        }
        public async Task<IActionResult> Search(ProductListFilterDto keyword)
        {
            var model = await _productAppService.GetListFilterAsync(new ProductListFilterDto()
            {
                Keyword = keyword.Keyword,
            }
            );
            //trả về view '/product.html'
            return RedirectToAction("Index", "Home");

        }
    }
}
