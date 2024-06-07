using AutoMapper.Internal.Mappers;
using Microsoft.AspNetCore.Authorization;
using SonEcommerce.Admin.Permissions;
using SonEcommerce.Admin.ProductCategories;
using SonEcommerce.ProductCategories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace SonEcommerce.Admin.ProductCategories
{
    [Authorize(SonEcommercePermissions.ProductCategory.Default, Policy = "AdminOnly")]
    public class ProductCategoriesAppService : CrudAppService<
        ProductCategory,
        ProductCategoryDto,
        Guid,
        PagedResultRequestDto,
        CreateUpdateProductCategoryDto,
        CreateUpdateProductCategoryDto>, IProductCategoriesAppService
    {
        private readonly ProductCategoryManager _productCategoryManager;
        private readonly ProductCategoryCodeGenerator _productCategoryCodeGenerator;
        public ProductCategoriesAppService(IRepository<ProductCategory, Guid> repository, ProductCategoryManager productCategoryManager, ProductCategoryCodeGenerator productCategoryCodeGenerator)
            : base(repository)
        {
            GetPolicyName = SonEcommercePermissions.ProductCategory.Default;
            GetListPolicyName = SonEcommercePermissions.ProductCategory.Default;
            CreatePolicyName = SonEcommercePermissions.ProductCategory.Create;
            UpdatePolicyName = SonEcommercePermissions.ProductCategory.Update;
            DeletePolicyName = SonEcommercePermissions.ProductCategory.Delete;
            _productCategoryManager = productCategoryManager;
            _productCategoryCodeGenerator = productCategoryCodeGenerator;
        }
        [Authorize(SonEcommercePermissions.ProductCategory.Delete)]
        public async Task DeleteMultipleAsync(IEnumerable<Guid> ids)
        {
            await Repository.DeleteManyAsync(ids);
            await UnitOfWorkManager.Current.SaveChangesAsync();
        }

        [Authorize(SonEcommercePermissions.ProductCategory.Create)]
        public override async Task<ProductCategoryDto> CreateAsync(CreateUpdateProductCategoryDto input)
        {
            var category = await _productCategoryManager.CreateAsync(
                input.Name,
                input.Code,
                input.Slug,
                input.SortOrder,
                input.CoverPicture,
                input.Visibility,
                input.IsActive,
                input.ParentId,
                input.SeoMetaDescription
             );
            await Repository.InsertAsync(category);
            return ObjectMapper.Map<ProductCategory, ProductCategoryDto>(category);
        }
        [Authorize(SonEcommercePermissions.ProductCategory.Default)]
        public async Task<List<ProductCategoryInListDto>> GetListAllAsync()
        {
            var query = await Repository.GetQueryableAsync();
            query = query.Where(x => x.IsActive == true);
            var data = await AsyncExecuter.ToListAsync(query);

            return ObjectMapper.Map<List<ProductCategory>, List<ProductCategoryInListDto>>(data);

        }
        [Authorize(SonEcommercePermissions.ProductCategory.Default)]
        public async Task<PagedResultDto<ProductCategoryInListDto>> GetListFilterAsync(BaseListFilterDto input)
        {
            var query = await Repository.GetQueryableAsync();
            query = query.WhereIf(!string.IsNullOrWhiteSpace(input.Keyword), x => x.Name.Contains(input.Keyword));

            var totalCount = await AsyncExecuter.LongCountAsync(query);
            var data = await AsyncExecuter.ToListAsync(query.Skip(input.SkipCount).Take(input.MaxResultCount));

            return new PagedResultDto<ProductCategoryInListDto>(totalCount, ObjectMapper.Map<List<ProductCategory>, List<ProductCategoryInListDto>>(data));
        }

        public async Task<string> GetSuggestNewCodeAsync()
        {
            return await _productCategoryCodeGenerator.GenerateAsync();
        }
    }
}
