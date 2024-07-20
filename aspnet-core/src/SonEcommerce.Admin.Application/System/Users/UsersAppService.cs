using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using SonEcommerce.Admin.System.Users;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using static Volo.Abp.Identity.Settings.IdentitySettingNames;

namespace SonEcommerce.Admin.Users
{

    public class UsersAppService : CrudAppService<IdentityUser, UserDto, Guid, PagedResultRequestDto,
                        CreateUserDto, UpdateUserDto>, IUsersAppService
    {
        private readonly IdentityUserManager _identityUserManager;
        private readonly UserRoleFinder _userRoleFinder;
        public UsersAppService(IRepository<IdentityUser, Guid> repository,
            IdentityUserManager identityUserManager, UserRoleFinder userRoleFinder) : base(repository)
        {
            _identityUserManager = identityUserManager;

            GetPolicyName = IdentityPermissions.Users.Default;
            GetListPolicyName = IdentityPermissions.Users.Default;
            CreatePolicyName = IdentityPermissions.Users.Create;
            UpdatePolicyName = IdentityPermissions.Users.Update;
            DeletePolicyName = IdentityPermissions.Users.Delete;

            _userRoleFinder = userRoleFinder;

        }
        [Authorize(IdentityPermissions.Users.Delete)]
        public async Task DeleteMultipleAsync(IEnumerable<Guid> ids)
        {
            await Repository.DeleteManyAsync(ids);
            await UnitOfWorkManager.Current.SaveChangesAsync();
        }

        [Authorize(IdentityPermissions.Users.Default)]
        public async Task<List<UserInListDto>> GetListAllAsync(string filterKeyword)
        {
            var query = await Repository.GetQueryableAsync();
            if (!string.IsNullOrEmpty(filterKeyword))
            {
                query = query.Where(o => o.Name.ToLower().Contains(filterKeyword)
              || o.Email.ToLower().Contains(filterKeyword)
              || o.PhoneNumber.ToLower().Contains(filterKeyword));
            }

            var data = await AsyncExecuter.ToListAsync(query);
            return ObjectMapper.Map<List<IdentityUser>, List<UserInListDto>>(data);
        }

        [Authorize(IdentityPermissions.Users.Default)]
        public async Task<PagedResultDto<UserInListDto>> GetListWithFilterAsync(BaseListFilterDto input)
        {
            var query = await Repository.GetQueryableAsync();

            if (!input.Keyword.IsNullOrWhiteSpace())
            {
                input.Keyword = input.Keyword.ToLower();
                query = query.Where(o => o.Name.ToLower().Contains(input.Keyword)
                || o.Email.ToLower().Contains(input.Keyword)
                || o.PhoneNumber.ToLower().Contains(input.Keyword));
            }
            query = query.OrderByDescending(x => x.CreationTime);

            var totalCount = await AsyncExecuter.CountAsync(query);

            query = query.Skip(input.SkipCount).Take(input.MaxResultCount);
            var data = await AsyncExecuter.ToListAsync(query);
            var users = ObjectMapper.Map<List<IdentityUser>, List<UserInListDto>>(data);
            return new PagedResultDto<UserInListDto>(totalCount, users);
        }

        [Authorize(IdentityPermissions.Users.Create)]
        public async override Task<UserDto> CreateAsync(CreateUserDto input)
        {
            var query = await Repository.GetQueryableAsync();
            var isUserNameExisted = query.Any(x => x.UserName == input.UserName);
            if (isUserNameExisted)
            {
                throw new UserFriendlyException("Tài khoản đã tồn tại");
            }

            var isUserEmailExisted = query.Any(x => x.Email == input.Email);
            if (isUserEmailExisted)
            {
                throw new UserFriendlyException("Email đã tồn tại");
            }
            var userId = Guid.NewGuid();
            var user = new IdentityUser(userId, input.UserName, input.Email);
            user.Name = input.Name;
            user.Surname = input.Surname;
            var isUserPhoneNumberExisted = query.Any(x => x.PhoneNumber == input.PhoneNumber);
            if (isUserPhoneNumberExisted)
            {
                throw new UserFriendlyException("Số điện thoại đã tồn tại");
            }else
            {
                user.SetPhoneNumber(input.PhoneNumber, true);
            }


            var result = await _identityUserManager.CreateAsync(user, input.Password);
            if (result.Succeeded)
            {
                return ObjectMapper.Map<IdentityUser, UserDto>(user);
            }
            else
            {
                List<Microsoft.AspNetCore.Identity.IdentityError> errorList = result.Errors.ToList();
                string errors = "";

                foreach (var error in errorList)
                {
                    errors = errors + error.Description.ToString();
                }
                throw new UserFriendlyException(errors);
            }
        }

        [Authorize(IdentityPermissions.Users.Update)]
        public async override Task<UserDto> UpdateAsync(Guid id, UpdateUserDto input)
        {
            var user = await _identityUserManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                throw new UserFriendlyException("Không Tìm Thấy Người Dùng!");
            }
            user.Name = input.Name;
            //kiểm tra xem số điện thoại đã tồn tại chưa
            if (user.PhoneNumber != input.PhoneNumber)
            {
                if (await CheckPhoneNumberExistAsync(input.PhoneNumber))
                {
                    throw new UserFriendlyException("Số điện thoại đã tồn tại");
                }
                else
                {
                    user.SetPhoneNumber(input.PhoneNumber, true);

                }
            }
            if (user.Email != input.Email)
            {
                if (await _identityUserManager.FindByEmailAsync(input.Email) != null)
                {
                    throw new UserFriendlyException("Email đã tồn tại");
                }
                else
                {
                    var setEmailResult = await _identityUserManager.SetEmailAsync(user, input.Email);
                    if (!setEmailResult.Succeeded)
                    {
                        throw new UserFriendlyException(string.Join(", ", setEmailResult.Errors.Select(e => e.Description)));
                    }

                    var setUserNameResult = await _identityUserManager.SetUserNameAsync(user, input.Email);
                    if (!setUserNameResult.Succeeded)
                    {
                        throw new UserFriendlyException(string.Join(", ", setUserNameResult.Errors.Select(e => e.Description)));
                    }
                }
            }
            user.Surname = input.Surname;

            var result = await _identityUserManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return ObjectMapper.Map<IdentityUser, UserDto>(user);
            }
            else
            {
                List<Microsoft.AspNetCore.Identity.IdentityError> errorList = result.Errors.ToList();
                string errors = "";

                foreach (var error in errorList)
                {
                    errors = errors + error.Description.ToString();
                }
                throw new UserFriendlyException(errors);
            }
        }
        public async Task<bool> CheckPhoneNumberExistAsync(string phoneNumber)
        {
            var query = await Repository.GetQueryableAsync();
            var isPhoneNumberExisted = query.Any(x => x.PhoneNumber == phoneNumber);
            if (isPhoneNumberExisted)
            {
                return true;
            }
            return false;



        }
        public async override Task<UserDto> GetAsync(Guid id)
        {
            var user = await _identityUserManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                throw new EntityNotFoundException(typeof(IdentityUser), id);
            }
            var userDto = ObjectMapper.Map<IdentityUser, UserDto>(user);
            var roles = await _identityUserManager.GetRolesAsync(user);
            userDto.Roles = roles;
            return userDto;
        }

        [Authorize(IdentityPermissions.Users.Update)]
        public async Task AssignRolesAsync(Guid userId, string[] roleNames)
        {
            var user = await _identityUserManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                throw new EntityNotFoundException(typeof(IdentityUser), userId);
            }
            var currentRoles = await _identityUserManager.GetRolesAsync(user);
            var removedResult = await _identityUserManager.RemoveFromRolesAsync(user, currentRoles);
            var addedResult = await _identityUserManager.AddToRolesAsync(user, roleNames);
            if (!addedResult.Succeeded || !removedResult.Succeeded)
            {
                List<Microsoft.AspNetCore.Identity.IdentityError> addedErrorList = addedResult.Errors.ToList();
                List<Microsoft.AspNetCore.Identity.IdentityError> removedErrorList = removedResult.Errors.ToList();
                var errorList = new List<Microsoft.AspNetCore.Identity.IdentityError>();
                errorList.AddRange(addedErrorList);
                errorList.AddRange(removedErrorList);
                string errors = "";

                foreach (var error in errorList)
                {
                    errors = errors + error.Description.ToString();
                }
                throw new UserFriendlyException(errors);
            }
        }

        [Authorize(IdentityPermissions.Users.Update)]
        public async Task SetPasswordAsync(Guid userId, SetPasswordDto input)
        {
            var user = await _identityUserManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                throw new EntityNotFoundException(typeof(IdentityUser), userId);
            }
            var token = await _identityUserManager.GeneratePasswordResetTokenAsync(user);
            var result = await _identityUserManager.ResetPasswordAsync(user, token, input.NewPassword);
            if (!result.Succeeded)
            {
                List<Microsoft.AspNetCore.Identity.IdentityError> errorList = result.Errors.ToList();
                string errors = "";

                foreach (var error in errorList)
                {
                    errors = errors + error.Description.ToString();
                }
                throw new UserFriendlyException(errors);
            }
        }
        [AllowAnonymous]
        public async Task<bool> CheckPermissionAsync(Guid userId)
        {
            var user = await Repository.FindAsync(userId);
            if (user == null)
            {
                throw new EntityNotFoundException(typeof(IdentityUser), userId);
            }
            var roles = await _userRoleFinder.GetRolesAsync(userId);

            if (roles == null || roles.Length == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
            
            
            
        }
        [AllowAnonymous]
        public async Task<string> GetUserIdByUsernameAsync(string username)
        {
            //get user by username or email
            var user = await _identityUserManager.FindByNameAsync(username);
            if (user == null)
            {
                user = await _identityUserManager.FindByEmailAsync(username);
                if (user == null)
                {
                   throw new EntityNotFoundException(typeof(IdentityUser), username);
                }
            }
            if (user == null)
            {
                throw new EntityNotFoundException(typeof(IdentityUser), username);
            }
            return user.Id.ToString();
            
        }

        public async Task<PagedResultDto<UserInListDto>> GetListWithoutRolesAsync(BaseListFilterDto input)
        {
            var query = await Repository.GetQueryableAsync();

            // Apply keyword filter
            query = query.WhereIf(
                !input.Keyword.IsNullOrWhiteSpace(),
                o => o.Name.ToLower().Contains(input.Keyword.ToLower()) ||
                     o.Email.Contains(input.Keyword) ||
                     o.PhoneNumber.Contains(input.Keyword)
            );

            // Filter users without roles
            query = query.Where(o => o.Roles.Count == 0);

            // Get the total count of filtered records
            var totalCount = query.Count();

            // Apply pagination
            var data = await AsyncExecuter.ToListAsync(query.Skip(input.SkipCount).Take(input.MaxResultCount));

            // Map to DTO
            var userInListDtos = ObjectMapper.Map<List<IdentityUser>, List<UserInListDto>>(data);

            return new PagedResultDto<UserInListDto>(totalCount, userInListDtos);
        }


        public async Task<PagedResultDto<UserInListDto>> GetListWithRolesAsync(BaseListFilterDto input)
        {
            var query = await Repository.GetQueryableAsync();

            // Apply keyword filter
            query = query.WhereIf(
                !input.Keyword.IsNullOrWhiteSpace(),
                o => o.Name.ToLower().Contains(input.Keyword.ToLower()) ||
                     o.Email.Contains(input.Keyword) ||
                     o.PhoneNumber.Contains(input.Keyword)
            );

            // Filter users without roles
            query = query.Where(o => o.Roles.Count > 0);

            // Get the total count of filtered records
            var totalCount = query.Count();

            // Apply pagination
            var data = await AsyncExecuter.ToListAsync(query.Skip(input.SkipCount).Take(input.MaxResultCount));

            // Map to DTO
            var userInListDtos = ObjectMapper.Map<List<IdentityUser>, List<UserInListDto>>(data);

            return new PagedResultDto<UserInListDto>(totalCount, userInListDtos);

        }
        public async Task ChangePasswordAsync(Guid id, ChangePasswordDto input)
        {
            var user = await _identityUserManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                throw new EntityNotFoundException(typeof(IdentityUser), id);
            }
            var checkPassword = await _identityUserManager.CheckPasswordAsync(user, input.CurrentPassword);
            if (checkPassword)
            {
                if (input.NewPassword != input.ConfirmNewPassword)
                {
                    throw new UserFriendlyException("Mật khẩu mới không khớp");
                }
                await SetPasswordAsync(id, new SetPasswordDto { NewPassword = input.NewPassword });
            }
            else
            {
                throw new UserFriendlyException("Mật khẩu cũ không đúng");
            }

        }

        public async Task<UserDto> UpdateProfileAsync(Guid id, UpdateUserDto input)
        {
            var user = await _identityUserManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                throw new EntityNotFoundException(typeof(IdentityUser), id);
            }
            user.Name = input.Name;
            user.Surname = input.Surname;
            var isUserPhoneNumberExisted = await CheckPhoneNumberExistAsync(input.PhoneNumber);
            if (user.PhoneNumber != input.PhoneNumber)
            {
                if (isUserPhoneNumberExisted)
                {
                    throw new UserFriendlyException("Số điện thoại đã tồn tại");
                }
                else
                {
                    user.SetPhoneNumber(input.PhoneNumber, true);
                }
            }
            if (user.Email != input.Email)
            {
                if (await _identityUserManager.FindByEmailAsync(input.Email) != null)
                {
                    throw new UserFriendlyException("Email đã tồn tại");
                }
                else
                {
                    var setEmailResult = await _identityUserManager.SetEmailAsync(user, input.Email);
                    if (!setEmailResult.Succeeded)
                    {
                        throw new UserFriendlyException(string.Join(", ", setEmailResult.Errors.Select(e => e.Description)));
                    }

                    var setUserNameResult = await _identityUserManager.SetUserNameAsync(user, input.Email);
                    if (!setUserNameResult.Succeeded)
                    {
                        throw new UserFriendlyException(string.Join(", ", setUserNameResult.Errors.Select(e => e.Description)));
                    }
                }
            }
            var result = await _identityUserManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return ObjectMapper.Map<IdentityUser, UserDto>(user);
            }
            else
            {
                List<Microsoft.AspNetCore.Identity.IdentityError> errorList = result.Errors.ToList();
                string errors = "";

                foreach (var error in errorList)
                {
                    errors = errors + error.Description.ToString();
                }
                throw new UserFriendlyException(errors);
            }
            
        }
    }
}
