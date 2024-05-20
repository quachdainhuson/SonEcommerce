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
        public ProductCategoriesAppService(IRepository<ProductCategory, Guid> repository)
            : base(repository)
        {
            GetPolicyName = SonEcommercePermissions.ProductCategory.Default;
            GetListPolicyName = SonEcommercePermissions.ProductCategory.Default;
            CreatePolicyName = SonEcommercePermissions.ProductCategory.Create;
            UpdatePolicyName = SonEcommercePermissions.ProductCategory.Update;
            DeletePolicyName = SonEcommercePermissions.ProductCategory.Delete;
        }
        [Authorize(SonEcommercePermissions.ProductCategory.Delete)]
        public async Task DeleteMultipleAsync(IEnumerable<Guid> ids)
        {
            await Repository.DeleteManyAsync(ids);
            await UnitOfWorkManager.Current.SaveChangesAsync();
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
    }
}
