using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System.Threading.Tasks;
using Volo.Abp;

namespace SonEcommerce.Public.Web.Pages.Auth
{
    public class LoginModel : PageModel
    {
        /*private readonly ICustomerAppService _customerAppService;
        [BindProperty]
        public string Username { get; set; }
        [BindProperty]
        public string Password { get; set; }

        public LoginModel(ICustomerAppService customerAppService)
        {
            _customerAppService = customerAppService;
        }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            // Kiểm tra xem tên đăng nhập và mật khẩu có hợp lệ không
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                ModelState.AddModelError("Username", "Tên đăng nhập hoặc mật khẩu không được để trống");
                return Page();
            }

            try
            {
                // Await the login task
                var customer = await _customerAppService.LoginAsync(Username, Password);
                //add session
                HttpContext.Session.SetString(SonEcommerceConsts.Customer, JsonSerializer.Serialize(customer));
                return RedirectToPage("/");
            }
            catch (UserFriendlyException ex)
            {
                ModelState.AddModelError("LoginError", ex.Message);
                return Page();
            }
        }*/
        public IActionResult OnGet()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Challenge(new AuthenticationProperties { RedirectUri = "/" },
                    OpenIdConnectDefaults.AuthenticationScheme);

            }
            return RedirectToPage("/");
        }

    }
}