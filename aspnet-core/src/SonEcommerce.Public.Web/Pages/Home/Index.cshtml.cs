
using Microsoft.Extensions.Caching.Distributed;
using SonEcommerce.Public.ProductCategories;
using SonEcommerce.Public.Products;
using SonEcommerce.Public.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Caching;

namespace SonEcommerce.Public.Web.Pages;

public class IndexModel : SonEcommercePublicPageModel
{
    private readonly IDistributedCache<HomeCacheItem> _distributedCache;
    private readonly IProductCategoriesAppService _productCategoriesAppService;
    private readonly IProductsAppService _productsAppService;
    public List<ProductCategoryInListDto> Categories { get; set; }
    public List<ProductInListDto> TopSellerProduct { get; set; }
    public IndexModel(IProductCategoriesAppService productCategoriesAppService,
        IProductsAppService productsAppService,
        IDistributedCache<HomeCacheItem> distributedCache) 
    {
        _productCategoriesAppService = productCategoriesAppService;
        _productsAppService = productsAppService;
        _distributedCache = distributedCache;
    }
    public async Task OnGetAsync()
    {
        var allCategories = await _productCategoriesAppService.GetListAllAsync();
        var rootCategories = allCategories.Where(x => x.ParentId == null).ToList();
        foreach (var category in rootCategories)
        {
            category.Children = rootCategories.Where(x => x.ParentId == category.Id).ToList();
        }

        var topSellerProduct = await _productsAppService.GetListTopSellerAsync(10);

        TopSellerProduct = topSellerProduct;
        Categories = rootCategories;
    }

}
