using SonEcommerce.IdentitySettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace SonEcommerce.Manufacturers
{
    public class ManufacturerCodeGenerator : ITransientDependency
    {
        private readonly IRepository<IdentitySetting, string> _identitySettingRepository;
        public ManufacturerCodeGenerator(IRepository<IdentitySetting, string> identitySettingRepository)
        {
            _identitySettingRepository = identitySettingRepository;
        }
        public async Task<string> GenerateAsync()
        {
            string newCode;
            var identitySetting = await _identitySettingRepository.FindAsync(SonEcommerceConsts.ManufacturerIdentitySettingId);
            if (identitySetting == null)
            {
                identitySetting = await _identitySettingRepository.InsertAsync(new IdentitySetting(SonEcommerceConsts.ManufacturerIdentitySettingId, "Sản phẩm", SonEcommerceConsts.ManufacturerIdentitySettingPrefix, 1, 1));
                newCode = identitySetting.Prefix + identitySetting.CurrentNumber;

            }
            else
            {
                identitySetting.CurrentNumber += identitySetting.StepNumber;
                newCode = identitySetting.Prefix + identitySetting.CurrentNumber;

                await _identitySettingRepository.UpdateAsync(identitySetting);
            }
            return newCode;
        }
    }
}
