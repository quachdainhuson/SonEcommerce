using SonEcommerce.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace SonEcommerce.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class SonEcommerceController : AbpControllerBase
{
    protected SonEcommerceController()
    {
        LocalizationResource = typeof(SonEcommerceResource);
    }
}
