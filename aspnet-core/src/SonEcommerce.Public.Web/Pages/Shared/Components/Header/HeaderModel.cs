using Microsoft.AspNetCore.Mvc.RazorPages;
using SonEcommerce.Public.ProductCategories;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace SonEcommerce.Public.Web.Pages.Shared.Components.Header
{
    public class HeaderModel : PageModel
    {
        public List<ProductCategoryInListDto> Categories { set; get; }
        private readonly IProductCategoriesAppService _productCategoriesAppService;
        public HeaderModel(IProductCategoriesAppService productCategoriesAppService)
        {
            _productCategoriesAppService = productCategoriesAppService;
        }
        public async Task OnGetAsync()
        {
            Categories = await _productCategoriesAppService.GetListAllAsync();
        }
    }
}
