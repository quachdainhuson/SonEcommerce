using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace SonEcommerce.Manufacturers
{
    
    public class ManufacturerManager : DomainService
    {
        private readonly IRepository<Manufacturer, Guid> _manufacturerRepository;
        public ManufacturerManager(IRepository<Manufacturer, Guid> manufacturerRepository)
        {
            _manufacturerRepository = manufacturerRepository;
        }

        public async Task<Manufacturer> CreateAsync(
            string name,
            string code,
            string slug,
            string coverPicture,
            bool visibility,
            bool isActive,
            string country
        )
        {
            if (await _manufacturerRepository.AnyAsync(x => x.Name == name))
            {
                throw new UserFriendlyException("Tên nhà sản xuất đã tồn tại", SonEcommerceDomainErrorCodes.ManufacturerNameAlreadyExists);
            }
            if (await _manufacturerRepository.AnyAsync(x => x.Code == code))
            {
                throw new UserFriendlyException("Mã nhà sản xuất đã tồn tại", SonEcommerceDomainErrorCodes.ManufacturerCodeAlreadyExists);
            }
            if (await _manufacturerRepository.AnyAsync(x => x.Slug == slug))
            {
                throw new UserFriendlyException("Slug nhà sản xuất đã tồn tại", SonEcommerceDomainErrorCodes.ManufacturerSlugAlreadyExists);
            }
            return new Manufacturer(name, code, slug, coverPicture, visibility, isActive, country);
        }
    }
}
