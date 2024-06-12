using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SonEcommerce.Public.Users
{
    public interface IUsersAppService : ICrudAppService
        <UserDto,
        Guid,
        PagedResultRequestDto,
        CreateUserDto,
        UpdateUserDto>
    {
        Task DeleteMultipleAsync(IEnumerable<Guid> ids);

        //get user by id
        Task<UserDto> GetUserByIdAsync(string id);
        Task SetPasswordAsync(Guid userId, SetPasswordDto input);
        //GetUserIdByUsernameAsync
        Task<string> GetUserIdByUsernameAsync(string username);



    }
}
