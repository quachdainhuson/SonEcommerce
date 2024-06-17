using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using SonEcommerce.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using static Volo.Abp.Identity.Settings.IdentitySettingNames;

namespace SonEcommerce.Public.Users
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
            _userRoleFinder = userRoleFinder;

        }
        public async Task DeleteMultipleAsync(IEnumerable<Guid> ids)
        {
            await Repository.DeleteManyAsync(ids);
            await UnitOfWorkManager.Current.SaveChangesAsync();
        }
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
            user.SetPhoneNumber(input.PhoneNumber, true);

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

        public async override Task<UserDto> UpdateAsync(Guid id, UpdateUserDto input)
        {

            var user = await _identityUserManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                throw new UserFriendlyException("Không Tìm Thấy Người Dùng!");
            }
            user.Name = input.Name;
            //kiểm tra xem số điện thoại đã tồn tại chưa
            if (user.PhoneNumber != input.PhoneNumber ) {
                if (await CheckPhoneNumberExistAsync(input.PhoneNumber)) {
                    throw new UserFriendlyException("Số điện thoại đã tồn tại");
                }
                else
                {
                    user.SetPhoneNumber(input.PhoneNumber, true);

                }
            }
            user.Surname = input.Surname;
            ((IHasExtraProperties)user).ExtraProperties[AppUser.UserAddress] = input.UserAddress;
            ((IHasExtraProperties)user).ExtraProperties[AppUser.UserCity] = input.UserCity;
            ((IHasExtraProperties)user).ExtraProperties[AppUser.UserDistrict] = input.UserDistrict;
            ((IHasExtraProperties)user).ExtraProperties[AppUser.UserWard] = input.UserWard;

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

        public async Task<UserDto> GetUserByIdAsync(string id)
        {
            var user = await _identityUserManager.FindByIdAsync(id.ToString());

            if (user == null)
            {
                throw new EntityNotFoundException(typeof(IdentityUser), id);
            }

            var userDto = ObjectMapper.Map<IdentityUser, UserDto>(user);
            var extraProperties = ((IHasExtraProperties)user).ExtraProperties;
            if (extraProperties.ContainsKey(AppUser.UserAddress))
            {
                userDto.UserAddress = extraProperties[AppUser.UserAddress] as string;
            }
            if (extraProperties.ContainsKey(AppUser.UserCity))
            {
                userDto.UserCity = extraProperties[AppUser.UserCity] as string;
            }
            if (extraProperties.ContainsKey(AppUser.UserDistrict))
            {
                userDto.UserDistrict = extraProperties[AppUser.UserDistrict] as string;
            }
            if (extraProperties.ContainsKey(AppUser.UserWard))
            {
                userDto.UserWard = extraProperties[AppUser.UserWard] as string;
            }

            return userDto;
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
    }
}
