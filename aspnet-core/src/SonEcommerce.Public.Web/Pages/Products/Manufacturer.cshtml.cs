using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SonEcommerce.Public.Manufacturers;
using SonEcommerce.Public.ProductCategories;
using SonEcommerce.Public.Products;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SonEcommerce.Public.Web.Pages.Products
{
    public class ManufacturerModel : PageModel
    {
        public ManufacturerDto Manufacturer { set; get; }

        public List<ProductCategoryInListDto> Categories { set; get; }
        public PagedResult<ProductInListDto> ProductData { set; get; }

        private readonly IProductCategoriesAppService _productCategoriesAppService;
        private readonly IManufacturersAppService _manufacturersAppService;
        private readonly IProductsAppService _productsAppService;
        [BindProperty]
        public double? MinPrice { get; set; }
        [BindProperty]
        public string? ManufacturerCode { get; set; }
        [BindProperty]
        public double? MaxPrice { get; set; }
        public ManufacturerModel(IProductCategoriesAppService productCategoriesAppService,
            IProductsAppService productsAppService,
            IManufacturersAppService manufacturersAppService)
        {
            _productCategoriesAppService = productCategoriesAppService;
            _productsAppService = productsAppService;
            _manufacturersAppService = manufacturersAppService;
        }

        public async Task OnGetAsync(string code, int page = 1)
        {
            Manufacturer = await _manufacturersAppService.GetByCodeAsync(code);
            Categories = await _productCategoriesAppService.GetListAllAsync();
            if (int.TryParse(Request.Query["page"], out int currentPage))
            {
                // Nếu thành công, sử dụng giá trị trang từ query string
                ProductData = await _productsAppService.GetListFilterAsync(new ProductListFilterDto()
                {
                    CurrentPage = currentPage,
                    ManufacturerId = Manufacturer.Id
                });
            }
            else
            {
                // Nếu không thành công, sử dụng trang mặc định (trang 1)
                ProductData = await _productsAppService.GetListFilterAsync(new ProductListFilterDto()
                {
                    CurrentPage = 1,
                    ManufacturerId = Manufacturer.Id
                });
            }
        }
        public async Task OnPostAsync( int page = 1)
        {
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
                MinPrice = null;
                MaxPrice = null;
                TempData["MessageError"] = "Giá min hiện tại đang cao hơn giá max";
            }
            Manufacturer = await _manufacturersAppService.GetByCodeAsync(ManufacturerCode);
            Categories = await _productCategoriesAppService.GetListAllAsync();
            if (int.TryParse(Request.Query["page"], out int currentPage))
            {
                // Nếu thành công, sử dụng giá trị trang từ query string
                ProductData = await _productsAppService.GetListFilterAsync(new ProductListFilterDto()
                {
                    CurrentPage = currentPage,
                    ManufacturerId = Manufacturer.Id,
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
                    ManufacturerId = Manufacturer.Id,
                    MinPrice = MinPrice,
                    MaxPrice = MaxPrice
                });
            }
        }
    }
}
