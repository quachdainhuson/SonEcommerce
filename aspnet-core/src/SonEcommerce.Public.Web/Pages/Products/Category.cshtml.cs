using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SonEcommerce.Public.ProductCategories;
using SonEcommerce.Public.Products;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SonEcommerce.Public.Web.Pages.Products
{
    public class CategoryModel : PageModel
    {
        public ProductCategoryDto Category { set; get; }

        public List<ProductCategoryInListDto> Categories { set; get; }
        public PagedResult<ProductInListDto> ProductData { set; get; }

        private readonly IProductCategoriesAppService _productCategoriesAppService;
        private readonly IProductsAppService _productsAppService;

        public CategoryModel(IProductCategoriesAppService productCategoriesAppService,
            IProductsAppService productsAppService)
        {
            _productCategoriesAppService = productCategoriesAppService;
            _productsAppService = productsAppService;
        }

        public async Task OnGetAsync(string code, int page = 1)
        {
            Category = await _productCategoriesAppService.GetByCodeAsync(code);
            Categories = await _productCategoriesAppService.GetListAllAsync();
            if (int.TryParse(Request.Query["page"], out int currentPage))
            {
                // Nếu thành công, sử dụng giá trị trang từ query string
                ProductData = await _productsAppService.GetListFilterAsync(new ProductListFilterDto()
                {
                    CurrentPage = currentPage,
                    CategoryId = Category.Id
                });
            }
            else
            {
                // Nếu không thành công, sử dụng trang mặc định (trang 1)
                ProductData = await _productsAppService.GetListFilterAsync(new ProductListFilterDto()
                {
                    CurrentPage = 1,
                    CategoryId = Category.Id
                });
            }
        }
    }
}
