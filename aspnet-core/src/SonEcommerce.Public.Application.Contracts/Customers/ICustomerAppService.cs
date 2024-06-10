using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SonEcommerce.Public
{
    public interface ICustomerAppService : ICrudAppService
        <CustomerDto,
        Guid,
        PagedResultRequestDto,
        CreateUpdateCustomerDto,
        CreateUpdateCustomerDto>
    {
        Task<PagedResultDto<CustomerInListDto>> GetListFilterAsync(BaseListFilterDto input);
        Task<List<CustomerInListDto>> GetListAllAsync();
        //kiểm tra username, email, phone đã tồn tại chưa
        Task<bool> IsUsernameExistAsync(string username);
        Task<bool> IsEmailExistAsync(string email);
        Task<bool> IsPhoneExistAsync(string phone);
        // đăng nhập bằng username, số điện thoại, email
        Task<CustomerDto> LoginAsync(string username, string password);
    }
}
