using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SonEcommerce.Public.ProductCategories;
using SonEcommerce.Public.Products;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SonEcommerce.Public.Web.Pages.Products
{
    public class ProductModel : PageModel
    {
        public ProductCategoryDto Category { get; set; }
        public List<ProductCategoryInListDto> Categories { get; set; }
        public PagedResult<ProductInListDto> ProductData { get; set; }

        private readonly IProductCategoriesAppService _productCategoriesAppService;
        private readonly IProductsAppService _productsAppService;

        [BindProperty]
        public double? MinPrice { get; set; }

        [BindProperty]
        public double? MaxPrice { get; set; }

        public ProductModel(IProductCategoriesAppService productCategoriesAppService,
            IProductsAppService productsAppService)
        {
            _productCategoriesAppService = productCategoriesAppService;
            _productsAppService = productsAppService;
        }

        public async Task OnGetAsync(string keyword, int page = 1)
        {
            Categories = await _productCategoriesAppService.GetListAllAsync();
            if (int.TryParse(Request.Query["page"], out int currentPage))
            {
                var filterDto = new ProductListFilterDto
                {
                    CurrentPage = currentPage,
                    MinPrice = MinPrice,
                    MaxPrice = MaxPrice,
                    Keyword = keyword
                };

                ProductData = await _productsAppService.GetListFilterAsync(filterDto);
            }
            else
            {
                var filterDto = new ProductListFilterDto
                {
                    CurrentPage = 1,
                    MinPrice = MinPrice,
                    MaxPrice = MaxPrice,
                    Keyword = keyword
                };

                ProductData = await _productsAppService.GetListFilterAsync(filterDto);
            }
        }

        public async Task OnPostAsync(string code, int page = 1)
        {
            if (MinPrice == null || MinPrice < 0)
            {
                MinPrice = 0;
            }

            if (MaxPrice == null || MaxPrice < 0)
            {
                MaxPrice = 0;
            }

            if (MinPrice > MaxPrice)
            {
                MinPrice = null;
                MaxPrice = null;
                TempData["MessageError"] = "Giá min hiện tại đang cao hơn giá max";
            }

            Categories = await _productCategoriesAppService.GetListAllAsync();

            var filterDto = new ProductListFilterDto
            {
                CurrentPage = page,
                MinPrice = MinPrice,
                MaxPrice = MaxPrice,
                Keyword = code // Sử dụng từ khóa trong post request
            };

            ProductData = await _productsAppService.GetListFilterAsync(filterDto);
        }
    }
}
