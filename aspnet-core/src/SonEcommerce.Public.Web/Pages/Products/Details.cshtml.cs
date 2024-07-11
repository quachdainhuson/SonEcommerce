using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SonEcommerce.Public.Manufacturers;
using SonEcommerce.Public.ProductCategories;
using SonEcommerce.Public.Products;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SonEcommerce.Public.Web.Pages.Products
{
    public class DetailsModel : PageModel
    {
        private readonly IProductsAppService _productsAppService;
        private readonly IProductCategoriesAppService _productCategoriesAppService;
        private readonly IManufacturersAppService _manufacturersAppService;
        public DetailsModel(IProductsAppService productsAppService,
            IProductCategoriesAppService productCategoriesAppService,
            IManufacturersAppService manufacturersAppService
            )
        {
            _productsAppService = productsAppService;
            _productCategoriesAppService = productCategoriesAppService;
            _manufacturersAppService = manufacturersAppService;
        }
        public ProductCategoryDto Category { get; set; }
        public ProductDto Product { get; set; }
        public List<ProductAttributeValueDto> ProductAttribute { get; set; }
        public async Task OnGetAsync(string categorySlug, string slug)
        {
            //nếu không có thể hiện trang 404
            if (string.IsNullOrEmpty(categorySlug) || string.IsNullOrEmpty(slug))
            {
                Response.Redirect("/404");
            }
            Category = await _productCategoriesAppService.GetBySlugAsync(categorySlug);
            Product = await _productsAppService.GetBySlugAsync(slug);
            ProductAttribute = await _productsAppService.GetListProductAttributeAllAsync(Product.Id);
        }
    }
}
