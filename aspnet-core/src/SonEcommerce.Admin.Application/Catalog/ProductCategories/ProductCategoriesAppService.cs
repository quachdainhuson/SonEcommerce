using AutoMapper.Internal.Mappers;
using Microsoft.AspNetCore.Authorization;
using SonEcommerce.Admin.Permissions;
using SonEcommerce.Admin.ProductCategories;
using SonEcommerce.Admin.Products;
using SonEcommerce.ProductCategories;
using SonEcommerce.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.BlobStoring;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace SonEcommerce.Admin.ProductCategories
{
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
        private readonly IBlobContainer<CategoryCoverPictureContainer> _fileContainer;
        private readonly IProductsAppService _productsAppService;
        public ProductCategoriesAppService(IRepository<ProductCategory, 
            Guid> repository, ProductCategoryManager productCategoryManager, 
            ProductCategoryCodeGenerator productCategoryCodeGenerator,
            IBlobContainer<CategoryCoverPictureContainer> fileContainer,
            IProductsAppService productsAppService
            )
            : base(repository)
        {
            GetPolicyName = SonEcommercePermissions.ProductCategory.Default;
            GetListPolicyName = SonEcommercePermissions.ProductCategory.Default;
            CreatePolicyName = SonEcommercePermissions.ProductCategory.Create;
            UpdatePolicyName = SonEcommercePermissions.ProductCategory.Update;
            DeletePolicyName = SonEcommercePermissions.ProductCategory.Delete;
            _productCategoryManager = productCategoryManager;
            _productCategoryCodeGenerator = productCategoryCodeGenerator;
            _fileContainer = fileContainer;
            _productsAppService = productsAppService;
        }
        [Authorize(SonEcommercePermissions.ProductCategory.Delete)]
        public async Task DeleteMultipleAsync(IEnumerable<Guid> ids)
        {
            // Kiểm tra xem các danh mục có sản phẩm hay không
            foreach (var id in ids)
            {
                var hasProducts = await _productsAppService.CheckProductCategoryHasProduct(id);
                if (hasProducts)
                {
                    throw new UserFriendlyException("Không thể xóa danh mục vì có sản phẩm.");
                }
            }

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
                input.Visibility,
                input.IsActive,
                input.ParentId
             );

            if (input.CoverPictureContent != null && input.CoverPictureContent.Length > 0)
            {
                await SaveCoverPictureAsync(input.CoverPictureName, input.CoverPictureContent);
                category.CoverPicture = input.CoverPictureName;
            }

            var result = await Repository.InsertAsync(category);
            return ObjectMapper.Map<ProductCategory, ProductCategoryDto>(result);
        }

        [Authorize(SonEcommercePermissions.ProductCategory.Update)]
        private async Task SaveCoverPictureAsync(string fileName, string base64)
        {
            Regex regex = new Regex(@"^[\w/\:.-]+;base64,");
            base64 = regex.Replace(base64, string.Empty);
            byte[] bytes = Convert.FromBase64String(base64);
            await _fileContainer.SaveAsync(fileName, bytes, overrideExisting: true);
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

        public async Task<string> GetCoverPictureAsync(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return null;
            }
            var coverPictureContent = await _fileContainer.GetAllBytesOrNullAsync(fileName);

            if (coverPictureContent is null)
            {
                return null;
            }
            var result = Convert.ToBase64String(coverPictureContent);
            return result;
        }

        [Authorize(SonEcommercePermissions.ProductCategory.Update)]
        public override async Task<ProductCategoryDto> UpdateAsync(Guid id, CreateUpdateProductCategoryDto input)
        {
            var category = await Repository.GetAsync(id);
            if (category == null)
                throw new BusinessException(SonEcommerceDomainErrorCodes.ProductCategoryIsNotExists);
            category.Name = input.Name;
            category.Code = input.Code;
            category.Slug = input.Slug;
            category.ParentId = input.ParentId;
            category.SortOrder = input.SortOrder;
            category.Visibility = input.Visibility;
            category.IsActive = input.IsActive;


            if (input.CoverPictureContent != null && input.CoverPictureContent.Length > 0)
            {
                await SaveCoverPictureAsync(input.CoverPictureName, input.CoverPictureContent);
                category.CoverPicture = input.CoverPictureName;

            }
            await Repository.UpdateAsync(category);

            return ObjectMapper.Map<ProductCategory, ProductCategoryDto>(category);
        }

        
    }

}
