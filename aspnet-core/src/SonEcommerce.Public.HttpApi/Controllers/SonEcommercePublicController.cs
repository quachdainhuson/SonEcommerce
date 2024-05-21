using SonEcommerce.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace SonEcommerce.Public.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class SonEcommercePublicController : AbpControllerBase
{
    protected SonEcommercePublicController()
    {
        LocalizationResource = typeof(SonEcommerceResource);
    }
}
