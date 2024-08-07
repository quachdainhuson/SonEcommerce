﻿using AutoMapper.Internal.Mappers;
using Microsoft.AspNetCore.Authorization;
using SonEcommerce.Admin.Permissions;
using SonEcommerce.Admin.ProductAttributes;
using SonEcommerce.Admin.Products;
using SonEcommerce.Attributes;
using SonEcommerce.Manufacturers;
using SonEcommerce.ProductCategories;
using SonEcommerce.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.BlobStoring;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace SonEcommerce.Admin.Products
{
    public class ProductsAppService : CrudAppService<
       Product,
       ProductDto,
       Guid,
       PagedResultRequestDto,
       CreateUpdateProductDto,
       CreateUpdateProductDto>, IProductsAppService
    {
        private readonly ProductManager _productManager;
        private readonly IRepository<ProductCategory> _productCategoryRepository;
        private readonly IBlobContainer<ProductThumbnailPictureContainer> _fileContainer;
        private readonly ProductCodeGenerator _productCodeGenerator;
        private readonly IRepository<ProductAttribute> _productAttributeRepository;
        private readonly IRepository<ProductAttributeDateTime> _productAttributeDateTimeRepository;
        private readonly IRepository<ProductAttributeInt> _productAttributeIntRepository;
        private readonly IRepository<ProductAttributeDecimal> _productAttributeDecimalRepository;
        private readonly IRepository<ProductAttributeVarchar> _productAttributeVarcharRepository;
        private readonly IRepository<ProductAttributeText> _productAttributeTextRepository;
        private readonly IRepository<Manufacturer> _manufacturerRepository;


        public ProductsAppService(IRepository<Product, Guid> repository,
            IRepository<ProductCategory> productCategoryRepository,
            ProductManager productManager,
            IBlobContainer<ProductThumbnailPictureContainer> fileContainer,
            ProductCodeGenerator productCodeGenerator,
            IRepository<ProductAttribute> productAttributeRepository,
            IRepository<ProductAttributeDateTime> productAttributeDateTimeRepository,
              IRepository<ProductAttributeInt> productAttributeIntRepository,
              IRepository<ProductAttributeDecimal> productAttributeDecimalRepository,
              IRepository<ProductAttributeVarchar> productAttributeVarcharRepository,
              IRepository<ProductAttributeText> productAttributeTextRepository,
              IRepository<Manufacturer> manufacturerRepository
              )
            : base(repository)
        {
            _productManager = productManager;
            _productCategoryRepository = productCategoryRepository;
            _fileContainer = fileContainer;
            _productCodeGenerator = productCodeGenerator;
            _productAttributeRepository = productAttributeRepository;
            _productAttributeDateTimeRepository = productAttributeDateTimeRepository;
            _productAttributeIntRepository = productAttributeIntRepository;
            _productAttributeDecimalRepository = productAttributeDecimalRepository;
            _productAttributeVarcharRepository = productAttributeVarcharRepository;
            _productAttributeTextRepository = productAttributeTextRepository;
            _manufacturerRepository = manufacturerRepository;

            GetPolicyName = SonEcommercePermissions.Product.Default;
            GetListPolicyName = SonEcommercePermissions.Product.Default;
            CreatePolicyName = SonEcommercePermissions.Product.Create;
            UpdatePolicyName = SonEcommercePermissions.Product.Update;
            DeletePolicyName = SonEcommercePermissions.Product.Delete;
        }

        [Authorize(SonEcommercePermissions.Product.Create)]
        public override async Task<ProductDto> CreateAsync(CreateUpdateProductDto input)
        {
            var product = await _productManager.CreateAsync(
                input.ManufacturerId,
                input.Name,
                input.Code,
                input.Slug,
                input.SKU,
                input.Visibility,
                input.IsActive,
                input.CategoryId,
                input.Description,
                input.sellPrice);

            if (input.ThumbnailPictureContent != null && input.ThumbnailPictureContent.Length > 0)
            {
                await SaveThumbnailImageAsync(input.ThumbnailPictureName, input.ThumbnailPictureContent);
                product.ThumbnailPicture = input.ThumbnailPictureName;
            }

            var result = await Repository.InsertAsync(product);

            return ObjectMapper.Map<Product, ProductDto>(result);
        }

        [Authorize(SonEcommercePermissions.Product.Update)]
        public override async Task<ProductDto> UpdateAsync(Guid id, CreateUpdateProductDto input)
        {
            var product = await Repository.GetAsync(id);
            if (product == null)
                throw new BusinessException(SonEcommerceDomainErrorCodes.ProductIsNotExists);
            product.ManufacturerId = input.ManufacturerId;
            product.Name = input.Name;
            product.Code = input.Code;
            product.Slug = input.Slug;
            product.SKU = input.SKU;
            product.Visibility = input.Visibility;
            product.IsActive = input.IsActive;

            if (product.CategoryId != input.CategoryId)
            {
                product.CategoryId = input.CategoryId;
                var category = await _productCategoryRepository.GetAsync(x => x.Id == input.CategoryId);
                product.CategoryName = category.Name;
                product.CategorySlug = category.Slug;
            }
            product.Description = input.Description;

            if (input.ThumbnailPictureContent != null && input.ThumbnailPictureContent.Length > 0)
            {
                await SaveThumbnailImageAsync(input.ThumbnailPictureName, input.ThumbnailPictureContent);
                product.ThumbnailPicture = input.ThumbnailPictureName;  

            }
            product.SellPrice = input.sellPrice;
            await Repository.UpdateAsync(product);

            return ObjectMapper.Map<Product, ProductDto>(product);
        }

        [Authorize(SonEcommercePermissions.Product.Delete)]
        public async Task DeleteMultipleAsync(IEnumerable<Guid> ids)
        {
            await Repository.DeleteManyAsync(ids);
            await UnitOfWorkManager.Current.SaveChangesAsync();
        }

        [Authorize(SonEcommercePermissions.Product.Default)]
        public async Task<List<ProductInListDto>> GetListAllAsync()
        {
            var query = await Repository.GetQueryableAsync();
            query = query.Where(x => x.IsActive == true);
            var data = await AsyncExecuter.ToListAsync(query);

            return ObjectMapper.Map<List<Product>, List<ProductInListDto>>(data);
        }

        [Authorize(SonEcommercePermissions.Product.Default)]
        public async Task<PagedResultDto<ProductInListDto>> GetListFilterAsync(ProductListFilterDto input)
        {
            var categoryQuery = await _productCategoryRepository.GetQueryableAsync();
            var manufacturerQuery = await _manufacturerRepository.GetQueryableAsync();
            var query = await Repository.GetQueryableAsync();
            query = query.WhereIf(!string.IsNullOrWhiteSpace(input.Keyword), x => x.Name.Contains(input.Keyword));
            query = query.WhereIf(input.CategoryId.HasValue, x => x.CategoryId == input.CategoryId);
            query = query.WhereIf(input.ManufacturerId.HasValue, x => x.ManufacturerId == input.ManufacturerId);


            // Join with Category table
            var joinQuery = from product in query
                            join category in categoryQuery on product.CategoryId equals category.Id
                            join manufacturer in manufacturerQuery on product.ManufacturerId equals manufacturer.Id
                            select new
                            {
                                Product = product,
                                CategoryName = category.Name,
                                CategorySlug = category.Slug,
                                ManufacturerName = manufacturer.Name
                            };

            var totalCount = await AsyncExecuter.LongCountAsync(joinQuery);

            var data = await AsyncExecuter.ToListAsync(
                joinQuery.OrderByDescending(x => x.Product.CreationTime)
                         .Skip(input.SkipCount)
                         .Take(input.MaxResultCount)
            );

            var result = data.Select(x => new ProductInListDto
            {
                Id = x.Product.Id,
                ManufacturerId = x.Product.ManufacturerId,
                Name = x.Product.Name,
                Code = x.Product.Code,
                Slug = x.Product.Slug,
                ProductType = x.Product.ProductType,
                SKU = x.Product.SKU,
                SortOrder = x.Product.SortOrder,
                Visibility = x.Product.Visibility,
                IsActive = x.Product.IsActive,
                CategoryId = x.Product.CategoryId,
                CreationTime = x.Product.CreationTime,
                ThumbnailPicture = x.Product.ThumbnailPicture,
                CategoryName = x.CategoryName,
                CategorySlug = x.CategorySlug,
                ManufacturerName = x.ManufacturerName
            }).ToList();

            return new PagedResultDto<ProductInListDto>(totalCount, result);
        }


        [Authorize(SonEcommercePermissions.Product.Update)]
        private async Task SaveThumbnailImageAsync(string fileName, string base64)
        {
            Regex regex = new Regex(@"^[\w/\:.-]+;base64,");
            base64 = regex.Replace(base64, string.Empty);
            byte[] bytes = Convert.FromBase64String(base64);
            await _fileContainer.SaveAsync(fileName, bytes, overrideExisting: true);
        }
        [Authorize(SonEcommercePermissions.Product.Default)]

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

        public async Task<string> GetSuggestNewCodeAsync()
        {
            return await _productCodeGenerator.GenerateAsync();
        }

        [Authorize(SonEcommercePermissions.Product.Update)]
        public async Task<ProductAttributeValueDto> AddProductAttributeAsync(AddUpdateProductAttributeDto input)
        {
            var product = await Repository.GetAsync(input.ProductId);
            if (product == null)
                throw new BusinessException(SonEcommerceDomainErrorCodes.ProductIsNotExists);

            var attribute = await _productAttributeRepository.GetAsync(x => x.Id == input.AttributeId);
            if (attribute == null)
                throw new BusinessException(SonEcommerceDomainErrorCodes.ProductAttributeIdIsNotExists);
            var newAttributeId = Guid.NewGuid();
            switch (attribute.DataType)
            {
                case AttributeType.Date:
                    if (input.DateTimeValue == null)
                    {
                        throw new BusinessException(SonEcommerceDomainErrorCodes.ProductAttributeValueIsNotValid);
                    }
                    var productAttributeDateTime = new ProductAttributeDateTime(newAttributeId, input.AttributeId, input.ProductId, input.DateTimeValue);
                    await _productAttributeDateTimeRepository.InsertAsync(productAttributeDateTime);
                    break;
                case AttributeType.Int:
                    if (input.IntValue == null)
                    {
                        throw new BusinessException(SonEcommerceDomainErrorCodes.ProductAttributeValueIsNotValid);
                    }
                    var productAttributeInt = new ProductAttributeInt(newAttributeId, input.AttributeId, input.ProductId, input.IntValue.Value);
                    await _productAttributeIntRepository.InsertAsync(productAttributeInt);
                    break;
                case AttributeType.Decimal:
                    if (input.DecimalValue == null)
                    {
                        throw new BusinessException(SonEcommerceDomainErrorCodes.ProductAttributeValueIsNotValid);
                    }
                    var productAttributeDecimal = new ProductAttributeDecimal(newAttributeId, input.AttributeId, input.ProductId, input.DecimalValue.Value);
                    await _productAttributeDecimalRepository.InsertAsync(productAttributeDecimal);
                    break;
                case AttributeType.Varchar:
                    if (input.VarcharValue == null)
                    {
                        throw new BusinessException(SonEcommerceDomainErrorCodes.ProductAttributeValueIsNotValid);
                    }
                    var productAttributeVarchar = new ProductAttributeVarchar(newAttributeId, input.AttributeId, input.ProductId, input.VarcharValue);
                    await _productAttributeVarcharRepository.InsertAsync(productAttributeVarchar);
                    break;
                case AttributeType.Text:
                    if (input.TextValue == null)
                    {
                        throw new BusinessException(SonEcommerceDomainErrorCodes.ProductAttributeValueIsNotValid);
                    }
                    var productAttributeText = new ProductAttributeText(newAttributeId, input.AttributeId, input.ProductId, input.TextValue);
                    await _productAttributeTextRepository.InsertAsync(productAttributeText);
                    break;
            }
            await UnitOfWorkManager.Current.SaveChangesAsync();
            return new ProductAttributeValueDto()
            {
                AttributeId = input.AttributeId,
                Code = attribute.Code,
                DataType = attribute.DataType,
                DateTimeValue = input.DateTimeValue,
                DecimalValue = input.DecimalValue,
                Id = newAttributeId,
                IntValue = input.IntValue,
                Label = attribute.Label,
                ProductId = input.ProductId,
                TextValue = input.TextValue
            };
        }

        [Authorize(SonEcommercePermissions.Product.Update)]
        public async Task RemoveProductAttributeAsync(Guid attributeId, Guid id)
        {
            var attribute = await _productAttributeRepository.GetAsync(x => x.Id == attributeId);
            if (attribute == null)
                throw new BusinessException(SonEcommerceDomainErrorCodes.ProductAttributeIdIsNotExists);
            switch (attribute.DataType)
            {
                case AttributeType.Date:
                    var productAttributeDateTime = await _productAttributeDateTimeRepository.GetAsync(x => x.Id == id);
                    if (productAttributeDateTime == null)
                    {
                        throw new BusinessException(SonEcommerceDomainErrorCodes.ProductAttributeIdIsNotExists);
                    }
                    await _productAttributeDateTimeRepository.DeleteAsync(productAttributeDateTime);
                    break;
                case AttributeType.Int:

                    var productAttributeInt = await _productAttributeIntRepository.GetAsync(x => x.Id == id);
                    if (productAttributeInt == null)
                    {
                        throw new BusinessException(SonEcommerceDomainErrorCodes.ProductAttributeIdIsNotExists);
                    }
                    await _productAttributeIntRepository.DeleteAsync(productAttributeInt);
                    break;
                case AttributeType.Decimal:
                    var productAttributeDecimal = await _productAttributeDecimalRepository.GetAsync(x => x.Id == id);
                    if (productAttributeDecimal == null)
                    {
                        throw new BusinessException(SonEcommerceDomainErrorCodes.ProductAttributeIdIsNotExists);
                    }
                    await _productAttributeDecimalRepository.DeleteAsync(productAttributeDecimal);
                    break;
                case AttributeType.Varchar:
                    var productAttributeVarchar = await _productAttributeVarcharRepository.GetAsync(x => x.Id == id);
                    if (productAttributeVarchar == null)
                    {
                        throw new BusinessException(SonEcommerceDomainErrorCodes.ProductAttributeIdIsNotExists);
                    }
                    await _productAttributeVarcharRepository.DeleteAsync(productAttributeVarchar);
                    break;
                case AttributeType.Text:
                    var productAttributeText = await _productAttributeTextRepository.GetAsync(x => x.Id == id);
                    if (productAttributeText == null)
                    {
                        throw new BusinessException(SonEcommerceDomainErrorCodes.ProductAttributeIdIsNotExists);
                    }
                    await _productAttributeTextRepository.DeleteAsync(productAttributeText);
                    break;
            }
            await UnitOfWorkManager.Current.SaveChangesAsync();
        }

        [Authorize(SonEcommercePermissions.Product.Default)]
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

        [Authorize(SonEcommercePermissions.Product.Default)]
        public async Task<PagedResultDto<ProductAttributeValueDto>> GetListProductAttributesAsync(ProductAttributeListFilterDto input)
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
            var data = await AsyncExecuter.ToListAsync(
                query.OrderByDescending(x => x.Label)
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                );
            return new PagedResultDto<ProductAttributeValueDto>(totalCount, data);
        }

        [Authorize(SonEcommercePermissions.Product.Update)]
        public async Task<ProductAttributeValueDto> UpdateProductAttributeAsync(Guid id, AddUpdateProductAttributeDto input)
        {
            var product = await Repository.GetAsync(input.ProductId);
            if (product == null)
                throw new BusinessException(SonEcommerceDomainErrorCodes.ProductIsNotExists);

            var attribute = await _productAttributeRepository.GetAsync(x => x.Id == input.AttributeId);
            if (attribute == null)
                throw new BusinessException(SonEcommerceDomainErrorCodes.ProductAttributeIdIsNotExists);

            switch (attribute.DataType)
            {
                case AttributeType.Date:
                    if (input.DateTimeValue == null)
                    {
                        throw new BusinessException(SonEcommerceDomainErrorCodes.ProductAttributeValueIsNotValid);
                    }
                    var productAttributeDateTime = await _productAttributeDateTimeRepository.GetAsync(x => x.Id == id);
                    if (productAttributeDateTime == null)
                    {
                        throw new BusinessException(SonEcommerceDomainErrorCodes.ProductAttributeIdIsNotExists);
                    }
                    productAttributeDateTime.Value = input.DateTimeValue.Value;
                    await _productAttributeDateTimeRepository.UpdateAsync(productAttributeDateTime);
                    break;
                case AttributeType.Int:
                    if (input.IntValue == null)
                    {
                        throw new BusinessException(SonEcommerceDomainErrorCodes.ProductAttributeValueIsNotValid);
                    }
                    var productAttributeInt = await _productAttributeIntRepository.GetAsync(x => x.Id == id);
                    if (productAttributeInt == null)
                    {
                        throw new BusinessException(SonEcommerceDomainErrorCodes.ProductAttributeIdIsNotExists);
                    }
                    productAttributeInt.Value = input.IntValue.Value;
                    await _productAttributeIntRepository.UpdateAsync(productAttributeInt);
                    break;
                case AttributeType.Decimal:
                    if (input.DecimalValue == null)
                    {
                        throw new BusinessException(SonEcommerceDomainErrorCodes.ProductAttributeValueIsNotValid);
                    }
                    var productAttributeDecimal = await _productAttributeDecimalRepository.GetAsync(x => x.Id == id);
                    if (productAttributeDecimal == null)
                    {
                        throw new BusinessException(SonEcommerceDomainErrorCodes.ProductAttributeIdIsNotExists);
                    }
                    productAttributeDecimal.Value = input.DecimalValue.Value;
                    await _productAttributeDecimalRepository.UpdateAsync(productAttributeDecimal);
                    break;
                case AttributeType.Varchar:
                    if (input.VarcharValue == null)
                    {
                        throw new BusinessException(SonEcommerceDomainErrorCodes.ProductAttributeValueIsNotValid);
                    }
                    var productAttributeVarchar = await _productAttributeVarcharRepository.GetAsync(x => x.Id == id);
                    if (productAttributeVarchar == null)
                    {
                        throw new BusinessException(SonEcommerceDomainErrorCodes.ProductAttributeIdIsNotExists);
                    }
                    productAttributeVarchar.Value = input.VarcharValue;
                    await _productAttributeVarcharRepository.UpdateAsync(productAttributeVarchar);
                    break;
                case AttributeType.Text:
                    if (input.TextValue == null)
                    {
                        throw new BusinessException(SonEcommerceDomainErrorCodes.ProductAttributeValueIsNotValid);
                    }
                    var productAttributeText = await _productAttributeTextRepository.GetAsync(x => x.Id == id);
                    if (productAttributeText == null)
                    {
                        throw new BusinessException(SonEcommerceDomainErrorCodes.ProductAttributeIdIsNotExists);
                    }
                    productAttributeText.Value = input.TextValue;
                    await _productAttributeTextRepository.UpdateAsync(productAttributeText);
                    break;
            }
            await UnitOfWorkManager.Current.SaveChangesAsync();
            return new ProductAttributeValueDto()
            {
                AttributeId = input.AttributeId,
                Code = attribute.Code,
                DataType = attribute.DataType,
                DateTimeValue = input.DateTimeValue,
                DecimalValue = input.DecimalValue,
                Id = id,
                IntValue = input.IntValue,
                Label = attribute.Label,
                ProductId = input.ProductId,
                TextValue = input.TextValue
            };
        }

        public async Task<List<ProductInListDto>> GetListAllByCategoryId(Guid categoryId)
        {
            var products = await Repository.GetListAsync(x => x.CategoryId == categoryId);
            return ObjectMapper.Map<List<Product>, List<ProductInListDto>>(products);

        }

        public async Task<bool> CheckProductCategoryHasProduct(Guid id)
        {
            var hasProduct = await Repository.AnyAsync(x => x.CategoryId == id);
            return hasProduct;
            
        }

        public async Task<bool> CheckProductHasManufacturer(Guid id)
        {
            var hasProduct = await Repository.AnyAsync(x => x.ManufacturerId == id);
            return hasProduct;
            
        }
    }
}
