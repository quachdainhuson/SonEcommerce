using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SonEcommerce.Admin.System.Users
{
    public interface IUsersAppService : ICrudAppService
        <UserDto,
        Guid,
        PagedResultRequestDto,
        CreateUserDto,
        UpdateUserDto>
    {
        Task DeleteMultipleAsync(IEnumerable<Guid> ids);

        Task<PagedResultDto<UserInListDto>> GetListWithFilterAsync(BaseListFilterDto input);

        Task<List<UserInListDto>> GetListAllAsync(string filterKeyword);
        Task AssignRolesAsync(Guid userId, string[] roleNames);
        Task SetPasswordAsync(Guid userId, SetPasswordDto input);
        // kiểm tra permission guid id = A9224B56-EF6B-401A-B910-1D248F2C6773
        Task<bool> CheckPermissionAsync(Guid userId);
        //GetUserIdByUsernameAsync
        Task<string> GetUserIdByUsernameAsync(string username);
        // lấy user không có roles với base filter
        Task<PagedResultDto<UserInListDto>> GetListWithoutRolesAsync(BaseListFilterDto input);
        Task<PagedResultDto<UserInListDto>> GetListWithRolesAsync(BaseListFilterDto input);


    }
}
