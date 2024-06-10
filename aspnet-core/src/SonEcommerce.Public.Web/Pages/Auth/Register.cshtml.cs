using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Volo.Abp;

namespace SonEcommerce.Public.Web.Pages.Auth
{
    public class RegisterModel : PageModel
    {
        /*private readonly ICustomerAppService _customerAppService;
        [BindProperty]
        public CreateUpdateCustomerDto CustomerInput { get; set; }
        [BindProperty]
        public string repassword { get; set; }
        public RegisterModel(ICustomerAppService customerAppService)
        {
            _customerAppService = customerAppService;
        }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (CustomerInput == null)
            {
                throw new UserFriendlyException("Xin nhập đầy đủ thông tin");
            }
            if (CustomerInput.Password != repassword)
            {
                ModelState.AddModelError("repassword", "Mật khẩu không khớp");
            }

            // Kiểm tra xem tên đăng nhập, số điện thoại và email đã tồn tại chưa
            var isUsernameExist = await _customerAppService.IsUsernameExistAsync(CustomerInput.Username);
            var isPhoneNumberExist = await _customerAppService.IsPhoneExistAsync(CustomerInput.Phone);
            var isEmailExist = await _customerAppService.IsEmailExistAsync(CustomerInput.Email);

            if (isUsernameExist)
            {
                ModelState.AddModelError("CustomerInput.Username", "Tên đăng nhập đã tồn tại");
            }
            if (isPhoneNumberExist)
            {
                ModelState.AddModelError("CustomerInput.Phone", "Số điện thoại đã tồn tại");
            }
            if (isEmailExist)
            {
                ModelState.AddModelError("CustomerInput.Email", "Email đã tồn tại");
            }

            // Nếu có bất kỳ thông điệp lỗi nào trong ModelState, trả về trang với thông điệp lỗi
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Nếu không có lỗi, tạo tài khoản mới
            var customer = await _customerAppService.CreateAsync(CustomerInput);
            return RedirectToPage("/");


        }*/
        private readonly IConfiguration _configuration;
        public RegisterModel(IConfiguration configuraiton)
        {
            _configuration = configuraiton;
        }
        public IActionResult OnGet()
        {
            return Redirect(_configuration["AuthServer:Authority"] + "/" + "Account/Register");
        }
    }
}
