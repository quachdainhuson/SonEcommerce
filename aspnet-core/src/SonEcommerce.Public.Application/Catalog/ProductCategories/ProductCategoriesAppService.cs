using AutoMapper.Internal.Mappers;
using Microsoft.AspNetCore.Authorization;
using SonEcommerce.ProductCategories;
using SonEcommerce.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.BlobStoring;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Uow;

namespace SonEcommerce.Public.ProductCategories
{
    public class ProductCategoriesAppService : ReadOnlyAppService<
        ProductCategory,
        ProductCategoryDto,
        Guid,
        PagedResultRequestDto>, IProductCategoriesAppService
    {
        private readonly IRepository<ProductCategory, Guid> _productCategoryRepository;
        private readonly IRepository<Product, Guid> _productRepository;
        private readonly IBlobContainer<CategoryCoverPictureContainer> _fileContainer;
        public ProductCategoriesAppService(IRepository<ProductCategory, Guid> repository,
            IBlobContainer<CategoryCoverPictureContainer> fileContainer,
            IRepository<Product, Guid> productRepository)
            : base(repository)
        {
            _productCategoryRepository = repository;
            _fileContainer = fileContainer;
            _productRepository = productRepository;
        }

        public async Task<ProductCategoryDto> GetByCodeAsync(string code)
        {
            var category = await _productCategoryRepository.GetAsync(x => x.Code == code);

            return ObjectMapper.Map<ProductCategory, ProductCategoryDto>(category);
        }

        public async Task<ProductCategoryDto> GetBySlugAsync(string slug)
        {
            var productCategory = await _productCategoryRepository.GetAsync(x => x.Slug == slug);
            return ObjectMapper.Map<ProductCategory, ProductCategoryDto>(productCategory);
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

        public async Task<List<ProductCategoryInListDto>> GetListAllAsync()
        {
            var query = await Repository.GetQueryableAsync();
            query = query.Where(x => x.IsActive == true);
            var data = await AsyncExecuter.ToListAsync(query);

            return ObjectMapper.Map<List<ProductCategory>, List<ProductCategoryInListDto>>(data);

        }
        public async Task<PagedResult<ProductCategoryInListDto>> GetListFilterAsync(BaseListFilterDto input)
        {
            var query = await Repository.GetQueryableAsync();
            query = query.WhereIf(!string.IsNullOrWhiteSpace(input.Keyword), x => x.Name.Contains(input.Keyword));

            var totalCount = await AsyncExecuter.LongCountAsync(query);
            var data = await AsyncExecuter
               .ToListAsync(
                  query.Skip((input.CurrentPage - 1) * input.PageSize)
               .Take(input.PageSize));

            return new PagedResult<ProductCategoryInListDto>(
                ObjectMapper.Map<List<ProductCategory>, List<ProductCategoryInListDto>>(data),
                totalCount,
                input.CurrentPage,
                input.PageSize
            );
        }

        public async Task<int> GetProductCountAsync(Guid categoryId)
        {
            return await _productRepository.CountAsync(p => p.CategoryId == categoryId);
        }
    }
}
