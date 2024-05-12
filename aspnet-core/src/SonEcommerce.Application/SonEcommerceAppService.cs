using System;
using System.Collections.Generic;
using System.Text;
using SonEcommerce.Localization;
using Volo.Abp.Application.Services;

namespace SonEcommerce;

/* Inherit your application services from this class.
 */
public abstract class SonEcommerceAppService : ApplicationService
{
    protected SonEcommerceAppService()
    {
        LocalizationResource = typeof(SonEcommerceResource);
    }
}
