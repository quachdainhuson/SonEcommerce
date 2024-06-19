using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Identity;

namespace SonEcommerce.Users
{
    public class AppUser : IdentityUser
    {
        public static string UserAddress { get; } = "UserAddress";
        public static string UserCity { get; } = "UserCity";

        public static string UserDistrict { get; } = "UserDistrict";
        public static string UserWard { get; } = "UserWard";
        // OTP and OTP Expire
        public static string OTP { get; } = "OTP";
        public static string OTPExpire { get; } = "OTPExpire";
    }
}
