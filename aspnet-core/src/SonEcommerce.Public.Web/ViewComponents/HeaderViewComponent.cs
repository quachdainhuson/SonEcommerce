using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using SonEcommerce.Public.ProductCategories;
using SonEcommerce.Public.Web.Models;
using System.Linq;
using System;
using System.Threading.Tasks;
using Volo.Abp.Caching;

namespace SonEcommerce.Public.Web.ViewComponents
{
    public class HeaderViewComponent : ViewComponent
    {
        private readonly IProductCategoriesAppService _productCategoriesAppService;

        public HeaderViewComponent(IProductCategoriesAppService productCategoriesAppService)
        {
            _productCategoriesAppService = productCategoriesAppService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = await _productCategoriesAppService.GetListAllAsync();
            return View(model);
        }
    }

}
