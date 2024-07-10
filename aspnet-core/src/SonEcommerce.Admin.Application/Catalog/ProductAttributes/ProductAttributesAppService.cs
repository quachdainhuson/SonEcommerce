using Microsoft.AspNetCore.Authorization;
using SonEcommerce.Admin.Permissions;
using SonEcommerce.Admin.ProductAttributes;
using SonEcommerce.Attributes;
using SonEcommerce.ProductAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace SonEcommerce.Admin.ProductAttributes
{
    public class ProductAttributesAppService : CrudAppService<
        ProductAttribute,
        ProductAttributeDto,
        Guid,
        PagedResultRequestDto,
        CreateUpdateProductAttributeDto,
        CreateUpdateProductAttributeDto>, IProductAttributesAppService
    {
        private readonly ProductAttributeCodeGenerator _productAttributeCodeGenerator;
        public ProductAttributesAppService(IRepository<ProductAttribute, Guid> repository, ProductAttributeCodeGenerator productAttributeCodeGenerator)
            : base(repository)
        {
            GetPolicyName = SonEcommercePermissions.Attribute.Default;
            GetListPolicyName = SonEcommercePermissions.Attribute.Default;
            CreatePolicyName = SonEcommercePermissions.Attribute.Create;
            UpdatePolicyName = SonEcommercePermissions.Attribute.Update;
            DeletePolicyName = SonEcommercePermissions.Attribute.Delete;
            _productAttributeCodeGenerator = productAttributeCodeGenerator;
        }
        [Authorize(SonEcommercePermissions.Attribute.Delete)]
        public async Task DeleteMultipleAsync(IEnumerable<Guid> ids)
        {
            await Repository.DeleteManyAsync(ids);
            await UnitOfWorkManager.Current.SaveChangesAsync();
        }

        [Authorize(SonEcommercePermissions.Attribute.Default)]
        public async Task<List<ProductAttributeInListDto>> GetListAllAsync()
        {
            var query = await Repository.GetQueryableAsync();
            query = query.Where(x => x.IsActive == true);
            var data = await AsyncExecuter.ToListAsync(query);

            return ObjectMapper.Map<List<ProductAttribute>, List<ProductAttributeInListDto>>(data);

        }

        [Authorize(SonEcommercePermissions.Attribute.Default)]
        public async Task<PagedResultDto<ProductAttributeInListDto>> GetListFilterAsync(BaseListFilterDto input)
        {
            var query = await Repository.GetQueryableAsync();
            query = query.WhereIf(!string.IsNullOrWhiteSpace(input.Keyword), x => x.Label.Contains(input.Keyword));

            var totalCount = await AsyncExecuter.LongCountAsync(query);
            var data = await AsyncExecuter.ToListAsync(query.Skip(input.SkipCount).Take(input.MaxResultCount));

            return new PagedResultDto<ProductAttributeInListDto>(totalCount, ObjectMapper.Map<List<ProductAttribute>, List<ProductAttributeInListDto>>(data));
        }

        public async Task<string> GetSuggestNewCodeAsync()
        {
            return await _productAttributeCodeGenerator.GenerateAsync();
        }
    }
}
