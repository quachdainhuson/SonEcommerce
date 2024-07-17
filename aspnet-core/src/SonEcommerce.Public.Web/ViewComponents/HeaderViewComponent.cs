using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using SonEcommerce.Public.ProductCategories;
using SonEcommerce.Public.Web.Models;
using System.Linq;
using System;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using SonEcommerce.Public.Manufacturers;

namespace SonEcommerce.Public.Web.ViewComponents
{
    public class HeaderViewComponent : ViewComponent
    {
        private readonly IProductCategoriesAppService _productCategoriesAppService;
        private readonly IManufacturersAppService _manufacturersAppService;

        public HeaderViewComponent(IProductCategoriesAppService productCategoriesAppService, IManufacturersAppService manufacturersAppService)
        {
            _productCategoriesAppService = productCategoriesAppService;
            _manufacturersAppService = manufacturersAppService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var productCategories = await _productCategoriesAppService.GetListAllAsync();
            var manufacturers = await _manufacturersAppService.GetListAllAsync();

            var viewModel = new HeaderCacheItem
            {
                Categories = productCategories,
                Manufacturer = manufacturers
            };

            return View(viewModel);
        }
    }

}
