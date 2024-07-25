using AutoMapper.Internal.Mappers;
using Microsoft.AspNetCore.Authorization;
using SonEcommerce.Attributes;
using SonEcommerce.Manufacturers;
using SonEcommerce.ProductCategories;
using SonEcommerce.Products;
using System;
using System.Collections;
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

namespace SonEcommerce.Public.Products
{
    public class ProductsAppService : ReadOnlyAppService<
       Product,
       ProductDto,
       Guid,
       PagedResultRequestDto>, IProductsAppService
    {
        private readonly IBlobContainer<ProductThumbnailPictureContainer> _fileContainer;
        private readonly IRepository<ProductCategory> _productCategoryRepository;

        private readonly IRepository<ProductAttribute> _productAttributeRepository;
        private readonly IRepository<ProductAttributeDateTime> _productAttributeDateTimeRepository;
        private readonly IRepository<ProductAttributeInt> _productAttributeIntRepository;
        private readonly IRepository<ProductAttributeDecimal> _productAttributeDecimalRepository;
        private readonly IRepository<ProductAttributeVarchar> _productAttributeVarcharRepository;
        private readonly IRepository<ProductAttributeText> _productAttributeTextRepository;
        private readonly IRepository<Product, Guid> _productRepository;
        private readonly IRepository<Manufacturer> _manufacturerRepository;


        public ProductsAppService(IRepository<Product, Guid> repository,
            IBlobContainer<ProductThumbnailPictureContainer> fileContainer,
            IRepository<ProductAttribute> productAttributeRepository,
            IRepository<ProductCategory> productCategoryRepository,
            IRepository<ProductAttributeDateTime> productAttributeDateTimeRepository,
              IRepository<ProductAttributeInt> productAttributeIntRepository,
              IRepository<ProductAttributeDecimal> productAttributeDecimalRepository,
              IRepository<ProductAttributeVarchar> productAttributeVarcharRepository,
              IRepository<ProductAttributeText> productAttributeTextRepository,
              IRepository<Manufacturer> manufacturerRepository,

              IRepository<Product, Guid> productRepository
              )
            : base(repository)
        {
            _fileContainer = fileContainer;
            _productAttributeRepository = productAttributeRepository;
            _productAttributeDateTimeRepository = productAttributeDateTimeRepository;
            _productAttributeIntRepository = productAttributeIntRepository;
            _productAttributeDecimalRepository = productAttributeDecimalRepository;
            _productAttributeVarcharRepository = productAttributeVarcharRepository;
            _productAttributeTextRepository = productAttributeTextRepository;
            _productRepository = productRepository;
            _manufacturerRepository = manufacturerRepository;
            _productCategoryRepository = productCategoryRepository;

        }



        public async Task<List<ProductInListDto>> GetListAllAsync()
        {
            var query = await Repository.GetQueryableAsync();
            var data = await AsyncExecuter.ToListAsync(query);

            return ObjectMapper.Map<List<Product>, List<ProductInListDto>>(data);
        }

        public async Task<PagedResult<ProductInListDto>> GetListFilterAsync(ProductListFilterDto input)
        {
            var categoryQuery = await _productCategoryRepository.GetQueryableAsync();
            var manufacturerQuery = await _manufacturerRepository.GetQueryableAsync();
            var query = await Repository.GetQueryableAsync();
            query = query.Where(x => x.Visibility == true);
            query = query.WhereIf(!string.IsNullOrWhiteSpace(input.Keyword), x => x.Name.Contains(input.Keyword) || x.CategoryName.Contains(input.Keyword));
            query = query.WhereIf(input.CategoryId.HasValue, x => x.CategoryId == input.CategoryId);
            query = query.WhereIf(input.ManufacturerId.HasValue, x => x.ManufacturerId == input.ManufacturerId);
            query = query.WhereIf(input.MinPrice.HasValue, x => x.SellPrice >= input.MinPrice);
            query = query.WhereIf(input.MaxPrice.HasValue, x => x.SellPrice <= input.MaxPrice);
            var joinQuery = from product in query
                            join category in categoryQuery on product.CategoryId equals category.Id
                            join manufacturer in manufacturerQuery on product.ManufacturerId equals manufacturer.Id
                            select new
                            {
                                Product = product,
                                CategoryName = category.Name,
                                CategorySlug = category.Slug,
                                ManufacturerName = manufacturer.Name,
                                ManufacturerSlug = manufacturer.Slug

                            };

            var totalCount = await AsyncExecuter.LongCountAsync(joinQuery);
            var data = await AsyncExecuter
               .ToListAsync(
                  joinQuery
                  .OrderByDescending(x => x.Product.CreationTime)
                  .Skip((input.CurrentPage - 1) * input.PageSize)
               .Take(input.PageSize));
            var result = data.Select(x => new ProductInListDto
            {
                Id = x.Product.Id,
                ManufacturerId = x.Product.ManufacturerId,
                Name = x.Product.Name,
                Code = x.Product.Code,
                Slug = x.Product.Slug,
                ProductType = x.Product.ProductType,
                SKU = x.Product.SKU,
                SellPrice = x.Product.SellPrice,
                SortOrder = x.Product.SortOrder,
                Visibility = x.Product.Visibility,
                IsActive = x.Product.IsActive,
                CategoryId = x.Product.CategoryId,
                CreationTime = x.Product.CreationTime,
                ThumbnailPicture = x.Product.ThumbnailPicture,
                CategoryName = x.CategoryName,
                CategorySlug = x.CategorySlug,
                ManufacturerName = x.ManufacturerName,
                ManufacturerSlug = x.ManufacturerSlug



            }).ToList();
            return new PagedResult<ProductInListDto>(result, totalCount, input.CurrentPage, input.PageSize);
            
        }



        public async Task<string> GetThumbnailImageAsync(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return null;
            }
            var thumbnailContent = await _fileContainer.GetAllBytesOrNullAsync(fileName);

            if (thumbnailContent is null)
            {
                return null;
            }
            var result = Convert.ToBase64String(thumbnailContent);
            return result;
        }


        public async Task<List<ProductAttributeValueDto>> GetListProductAttributeAllAsync(Guid productId)
        {
            var attributeQuery = await _productAttributeRepository.GetQueryableAsync();

            var attributeDateTimeQuery = await _productAttributeDateTimeRepository.GetQueryableAsync();
            var attributeDecimalQuery = await _productAttributeDecimalRepository.GetQueryableAsync();
            var attributeIntQuery = await _productAttributeIntRepository.GetQueryableAsync();
            var attributeVarcharQuery = await _productAttributeVarcharRepository.GetQueryableAsync();
            var attributeTextQuery = await _productAttributeTextRepository.GetQueryableAsync();

            var query = from a in attributeQuery
                        join adate in attributeDateTimeQuery on a.Id equals adate.AttributeId into aDateTimeTable
                        from adate in aDateTimeTable.DefaultIfEmpty()
                        join adecimal in attributeDecimalQuery on a.Id equals adecimal.AttributeId into aDecimalTable
                        from adecimal in aDecimalTable.DefaultIfEmpty()
                        join aint in attributeIntQuery on a.Id equals aint.AttributeId into aIntTable
                        from aint in aIntTable.DefaultIfEmpty()
                        join aVarchar in attributeVarcharQuery on a.Id equals aVarchar.AttributeId into aVarcharTable
                        from aVarchar in aVarcharTable.DefaultIfEmpty()
                        join aText in attributeTextQuery on a.Id equals aText.AttributeId into aTextTable
                        from aText in aTextTable.DefaultIfEmpty()
                        where (adate == null || adate.ProductId == productId)
                        && (adecimal == null || adecimal.ProductId == productId)
                         && (aint == null || aint.ProductId == productId)
                          && (aVarchar == null || aVarchar.ProductId == productId)
                           && (aText == null || aText.ProductId == productId)
                        select new ProductAttributeValueDto()
                        {
                            Label = a.Label,
                            AttributeId = a.Id,
                            DataType = a.DataType,
                            Code = a.Code,
                            ProductId = productId,
                            DateTimeValue = adate != null ? adate.Value : null,
                            DecimalValue = adecimal != null ? adecimal.Value : null,
                            IntValue = aint != null ? aint.Value : null,
                            TextValue = aText != null ? aText.Value : null,
                            VarcharValue = aVarchar != null ? aVarchar.Value : null,
                            DateTimeId = adate != null ? adate.Id : null,
                            DecimalId = adecimal != null ? adecimal.Id : null,
                            IntId = aint != null ? aint.Id : null,
                            TextId = aText != null ? aText.Id : null,
                            VarcharId = aVarchar != null ? aVarchar.Id : null,
                        };
            query = query.Where(x => x.DateTimeId != null
                           || x.DecimalId != null
                           || x.IntValue != null
                           || x.TextId != null
                           || x.VarcharId != null);
            return await AsyncExecuter.ToListAsync(query);
        }

        public async Task<PagedResult<ProductAttributeValueDto>> GetListProductAttributesAsync(ProductAttributeListFilterDto input)
        {
            var attributeQuery = await _productAttributeRepository.GetQueryableAsync();

            var attributeDateTimeQuery = await _productAttributeDateTimeRepository.GetQueryableAsync();
            var attributeDecimalQuery = await _productAttributeDecimalRepository.GetQueryableAsync();
            var attributeIntQuery = await _productAttributeIntRepository.GetQueryableAsync();
            var attributeVarcharQuery = await _productAttributeVarcharRepository.GetQueryableAsync();
            var attributeTextQuery = await _productAttributeTextRepository.GetQueryableAsync();

            var query = from a in attributeQuery
                        join adate in attributeDateTimeQuery on a.Id equals adate.AttributeId into aDateTimeTable
                        from adate in aDateTimeTable.DefaultIfEmpty()
                        join adecimal in attributeDecimalQuery on a.Id equals adecimal.AttributeId into aDecimalTable
                        from adecimal in aDecimalTable.DefaultIfEmpty()
                        join aint in attributeIntQuery on a.Id equals aint.AttributeId into aIntTable
                        from aint in aIntTable.DefaultIfEmpty()
                        join aVarchar in attributeVarcharQuery on a.Id equals aVarchar.AttributeId into aVarcharTable
                        from aVarchar in aVarcharTable.DefaultIfEmpty()
                        join aText in attributeTextQuery on a.Id equals aText.AttributeId into aTextTable
                        from aText in aTextTable.DefaultIfEmpty()
                        where (adate == null || adate.ProductId == input.ProductId)
                        && (adecimal == null || adecimal.ProductId == input.ProductId)
                         && (aint == null || aint.ProductId == input.ProductId)
                          && (aVarchar == null || aVarchar.ProductId == input.ProductId)
                           && (aText == null || aText.ProductId == input.ProductId)
                        select new ProductAttributeValueDto()
                        {
                            Label = a.Label,
                            AttributeId = a.Id,
                            DataType = a.DataType,
                            Code = a.Code,
                            ProductId = input.ProductId,
                            DateTimeValue = adate != null ? adate.Value : null,
                            DecimalValue = adecimal != null ? adecimal.Value : null,
                            IntValue = aint != null ? aint.Value : null,
                            TextValue = aText != null ? aText.Value : null,
                            VarcharValue = aVarchar != null ? aVarchar.Value : null,
                            DateTimeId = adate != null ? adate.Id : null,
                            DecimalId = adecimal != null ? adecimal.Id : null,
                            IntId = aint != null ? aint.Id : null,
                            TextId = aText != null ? aText.Id : null,
                            VarcharId = aVarchar != null ? aVarchar.Id : null,
                        };
            query = query.Where(x => x.DateTimeId != null
            || x.DecimalId != null
            || x.IntValue != null
            || x.TextId != null
            || x.VarcharId != null);
            var totalCount = await AsyncExecuter.LongCountAsync(query);
            var data = await AsyncExecuter
               .ToListAsync(
                  query.Skip((input.CurrentPage - 1) * input.PageSize)
               .Take(input.PageSize));

            return new PagedResult<ProductAttributeValueDto>(data,
                totalCount,
                input.CurrentPage,
                input.PageSize
            );
        }

        public async Task<List<ProductInListDto>> GetListTopSellerAsync(int numberOfRecords)
        {
            var query = await Repository.GetQueryableAsync();
            query = query.Where(x => x.Visibility == true)
                         .OrderByDescending(x => x.SortOrder) // Sắp xếp theo thứ tự giảm dần của SortOrder (hoặc một trường phù hợp khác)
                         .Take(numberOfRecords);

            var data = await AsyncExecuter.ToListAsync(query);

            var categoryQuery = await _productCategoryRepository.GetQueryableAsync();
            var manufacturerQuery = await _manufacturerRepository.GetQueryableAsync();

            var result = data.Select(x => new ProductInListDto
            {
                Id = x.Id,
                ManufacturerId = x.ManufacturerId,
                Name = x.Name,
                Code = x.Code,
                Slug = x.Slug,
                ProductType = x.ProductType,
                SKU = x.SKU,
                SellPrice = x.SellPrice,
                SortOrder = x.SortOrder,
                Visibility = x.Visibility,
                IsActive = x.IsActive,
                CategoryId = x.CategoryId,
                CreationTime = x.CreationTime,
                ThumbnailPicture = x.ThumbnailPicture,
                CategoryName = categoryQuery.FirstOrDefault(c => c.Id == x.CategoryId)?.Name,
                CategorySlug = categoryQuery.FirstOrDefault(c => c.Id == x.CategoryId)?.Slug,
                ManufacturerName = manufacturerQuery.FirstOrDefault(m => m.Id == x.ManufacturerId)?.Name,
                ManufacturerSlug = manufacturerQuery.FirstOrDefault(m => m.Id == x.ManufacturerId)?.Slug

            }).ToList();

            return result;
        }


        public async Task<ProductInListDto> GetBySlugAsync(string slug)
        {
            var query = await Repository.GetQueryableAsync();
            query = query.Where(x => x.Slug == slug);
                         

            var data = await AsyncExecuter.ToListAsync(query);

            var categoryQuery = await _productCategoryRepository.GetQueryableAsync();
            var manufacturerQuery = await _manufacturerRepository.GetQueryableAsync();

            var result = data.Select(x => new ProductInListDto
            {
                Id = x.Id,
                ManufacturerId = x.ManufacturerId,
                Name = x.Name,
                Code = x.Code,
                Slug = x.Slug,
                ProductType = x.ProductType,
                SKU = x.SKU,
                SellPrice = x.SellPrice,
                SortOrder = x.SortOrder,
                Visibility = x.Visibility,
                IsActive = x.IsActive,
                CategoryId = x.CategoryId,
                CreationTime = x.CreationTime,
                Description = x.Description,
                ThumbnailPicture = x.ThumbnailPicture,
                CategoryName = categoryQuery.FirstOrDefault(c => c.Id == x.CategoryId)?.Name,
                CategorySlug = categoryQuery.FirstOrDefault(c => c.Id == x.CategoryId)?.Slug,
                ManufacturerName = manufacturerQuery.FirstOrDefault(m => m.Id == x.ManufacturerId)?.Name,
                ManufacturerSlug = manufacturerQuery.FirstOrDefault(m => m.Id == x.ManufacturerId)?.Slug
            }).ToList();

            return result.FirstOrDefault();
        }

        public async Task<ProductInListDto> GetProductByIdsAsync(Guid ids)
        {
            var query = await Repository.GetQueryableAsync();
            query = query.Where(x => x.Id == ids);
            var data = await AsyncExecuter.ToListAsync(query);
            var categoryQuery = await _productCategoryRepository.GetQueryableAsync();
            var manufacturerQuery = await _manufacturerRepository.GetQueryableAsync();

            var result = data.Select(x => new ProductInListDto
            {
                Id = x.Id,
                ManufacturerId = x.ManufacturerId,
                Name = x.Name,
                Code = x.Code,
                Slug = x.Slug,
                ProductType = x.ProductType,
                SKU = x.SKU,
                SellPrice = x.SellPrice,
                SortOrder = x.SortOrder,
                Visibility = x.Visibility,
                IsActive = x.IsActive,
                CategoryId = x.CategoryId,
                CreationTime = x.CreationTime,
                Description = x.Description,
                ThumbnailPicture = x.ThumbnailPicture,
                CategoryName = categoryQuery.FirstOrDefault(c => c.Id == x.CategoryId)?.Name,
                CategorySlug = categoryQuery.FirstOrDefault(c => c.Id == x.CategoryId)?.Slug,
                ManufacturerName = manufacturerQuery.FirstOrDefault(m => m.Id == x.ManufacturerId)?.Name,
                ManufacturerSlug = manufacturerQuery.FirstOrDefault(m => m.Id == x.ManufacturerId)?.Slug
            }).ToList();

            return result.FirstOrDefault();
        }
    }
}
