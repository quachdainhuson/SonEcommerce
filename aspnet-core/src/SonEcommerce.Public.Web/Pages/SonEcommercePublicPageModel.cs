using SonEcommerce.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace SonEcommerce.Public.Web.Pages;

/* Inherit your PageModel classes from this class.
 */
public abstract class SonEcommercePublicPageModel : AbpPageModel
{
    protected SonEcommercePublicPageModel()
    {
        LocalizationResourceType = typeof(SonEcommerceResource);
    }
}
