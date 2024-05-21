using System;
using System.Collections.Generic;
using System.Text;
using SonEcommerce.Localization;
using Volo.Abp.Application.Services;

namespace SonEcommerce.Public;

/* Inherit your application services from this class.
 */
public abstract class SonEcommercePublicAppService : ApplicationService
{
    protected SonEcommercePublicAppService()
    {
        LocalizationResource = typeof(SonEcommerceResource);
    }
}
