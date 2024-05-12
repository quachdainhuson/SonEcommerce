using System;
using System.Collections.Generic;
using System.Text;
using SonEcommerce.Localization;
using Volo.Abp.Application.Services;

namespace SonEcommerce.Admin;

/* Inherit your application services from this class.
 */
public abstract class SonEcommerceAdminAppService : ApplicationService
{
    protected SonEcommerceAdminAppService()
    {
        LocalizationResource = typeof(SonEcommerceResource);
    }
}
