using AutoMapper.Internal.Mappers;
using Microsoft.AspNetCore.Authorization;
using SonEcommerce.Admin.Manufacturers;
using SonEcommerce.Admin.Permissions;
using SonEcommerce.Manufacturers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace SonEcommerce.Admin.Manufacturers
{
    public class ManufacturersAppService : CrudAppService<
        Manufacturer,
        ManufacturerDto,
        Guid,
        PagedResultRequestDto,
        CreateUpdateManufacturerDto,
        CreateUpdateManufacturerDto>, IManufacturersAppService
    {
        private readonly ManufacturerManager _manufacturerManager;
        private readonly ManufacturerCodeGenerator _manufacturerCodeGenerator;
        public ManufacturersAppService(IRepository<Manufacturer, Guid> repository, ManufacturerManager manufacturerManager, ManufacturerCodeGenerator manufacturerCodeGenerator)
            : base(repository)
        {
            GetPolicyName = SonEcommercePermissions.Manufacturer.Default;
            GetListPolicyName = SonEcommercePermissions.Manufacturer.Default;
            CreatePolicyName = SonEcommercePermissions.Manufacturer.Create;
            UpdatePolicyName = SonEcommercePermissions.Manufacturer.Update;
            DeletePolicyName = SonEcommercePermissions.Manufacturer.Delete;
            _manufacturerManager = manufacturerManager;
            _manufacturerCodeGenerator = manufacturerCodeGenerator;
        }
        [Authorize(SonEcommercePermissions.Manufacturer.Delete)]
        public async Task DeleteMultipleAsync(IEnumerable<Guid> ids)
        {
            await Repository.DeleteManyAsync(ids);
            await UnitOfWorkManager.Current.SaveChangesAsync();
        }
        [Authorize(SonEcommercePermissions.Manufacturer.Create)]
        public override async Task<ManufacturerDto> CreateAsync(CreateUpdateManufacturerDto input)
        {
            var manufacturer = await _manufacturerManager.CreateAsync(
                input.Name,
                input.Code,
                input.Slug,
                input.CoverPicture,
                input.Visibility,
                input.IsActive,
                input.Country
        );
            await Repository.InsertAsync(manufacturer);
            return ObjectMapper.Map<Manufacturer, ManufacturerDto>(manufacturer);
        }

        [Authorize(SonEcommercePermissions.Manufacturer.Default)]
        public async Task<List<ManufacturerInListDto>> GetListAllAsync()
        {
            var query = await Repository.GetQueryableAsync();
            query = query.Where(x => x.IsActive == true);
            var data = await AsyncExecuter.ToListAsync(query);

            return ObjectMapper.Map<List<Manufacturer>, List<ManufacturerInListDto>>(data);

        }
        [Authorize(SonEcommercePermissions.Manufacturer.Default)]
        public async Task<PagedResultDto<ManufacturerInListDto>> GetListFilterAsync(BaseListFilterDto input)
        {
            var query = await Repository.GetQueryableAsync();
            query = query.WhereIf(!string.IsNullOrWhiteSpace(input.Keyword), x => x.Name.Contains(input.Keyword));

            var totalCount = await AsyncExecuter.LongCountAsync(query);
            var data = await AsyncExecuter.ToListAsync(query.Skip(input.SkipCount).Take(input.MaxResultCount));

            return new PagedResultDto<ManufacturerInListDto>(totalCount, ObjectMapper.Map<List<Manufacturer>, List<ManufacturerInListDto>>(data));
        }

        public async Task<string> GetSuggestNewCodeAsync()
        {
            return await _manufacturerCodeGenerator.GenerateAsync();
        }
    }
}
