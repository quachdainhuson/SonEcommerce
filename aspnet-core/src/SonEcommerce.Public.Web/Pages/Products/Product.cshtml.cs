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
        public ProductCategoryDto Category { set; get; }

        public List<ProductCategoryInListDto> Categories { set; get; }
        public PagedResult<ProductInListDto> ProductData { set; get; }

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

        public async Task OnGetAsync(string code, int page = 1)
        {
            Categories = await _productCategoriesAppService.GetListAllAsync();
            if (int.TryParse(Request.Query["page"], out int currentPage))
            {
                // Nếu thành công, sử dụng giá trị trang từ query string
                ProductData = await _productsAppService.GetListFilterAsync(new ProductListFilterDto()
                {
                    CurrentPage = currentPage,
                    MinPrice= MinPrice,
                    MaxPrice= MaxPrice
                });
            }
            else
            {
                // Nếu không thành công, sử dụng trang mặc định (trang 1)
                ProductData = await _productsAppService.GetListFilterAsync(new ProductListFilterDto()
                {
                    CurrentPage = 1,
                    MinPrice = MinPrice,
                    MaxPrice = MaxPrice
                });
            }
        }
        public async Task OnPostAsync(string code, int page = 1)
        {
            //kiểm tra nếu giá trị min và max price không hợp lệ thì gán giá trị mặc định
            if (MinPrice == null || MinPrice < 0)
            {
                MinPrice = 0;
            }
            //kiểm tra nếu giá trị max price không hợp lệ thì gán giá trị mặc định
            if (MaxPrice == null || MaxPrice < 0)
            {
                MaxPrice = 0;
            }
            //kiểm tra nếu giá trị min price lớn hơn max price thì gán giá trị mặc định và hiển thị thông báo
            if (MinPrice > MaxPrice)
            {
                MinPrice = 0;
                MaxPrice = 0;
                TempData["Message"] = "Giá min hiện tại đang cao hơn giá max";
            }
            
            Categories = await _productCategoriesAppService.GetListAllAsync();
            if (int.TryParse(Request.Query["page"], out int currentPage))
            {
                // Nếu thành công, sử dụng giá trị trang từ query string
                ProductData = await _productsAppService.GetListFilterAsync(new ProductListFilterDto()
                {
                    CurrentPage = currentPage,
                    MinPrice = MinPrice,
                    MaxPrice = MaxPrice
                });
            }
            else
            {
                // Nếu không thành công, sử dụng trang mặc định (trang 1)
                ProductData = await _productsAppService.GetListFilterAsync(new ProductListFilterDto()
                {
                    CurrentPage = 1,
                    MinPrice = MinPrice,
                    MaxPrice = MaxPrice
                });
            }
        }

    }
}
