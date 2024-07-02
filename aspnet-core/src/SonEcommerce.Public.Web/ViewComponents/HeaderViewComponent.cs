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
        private readonly IDistributedCache<HeaderCacheItem> _distributedCache;
        public HeaderViewComponent(IProductCategoriesAppService productCategoriesAppService,
            IDistributedCache<HeaderCacheItem> distributedCache
            )
        {
            _productCategoriesAppService = productCategoriesAppService;
            _distributedCache = distributedCache;
        }
        public async Task<IViewComponentResult> InvokeAsync() 
        {
            //tìm kiếm

            var cacheItem = await _distributedCache.GetOrAddAsync(SonEcommercePublicConsts.CacheKeys.HeaderData, async () =>
            {
                var model = await _productCategoriesAppService.GetListAllAsync();
                return new HeaderCacheItem
                {
                    Categories = model
                };
            }, () => new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddHours(12)
            });
           return View(cacheItem.Categories);
        }
        
    }
}
