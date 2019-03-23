using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using User.Models;

namespace User.Infrastructure
{
    /// <summary>
    /// 自定义的用户验证
    /// </summary>
    public class CustomUserValidator : UserValidator<AppUser>
    {
        /// <summary>
        /// 该派生类的构造器必须以用户管理器类实例为参数，并调用基实现，才能够执行内建的验证检查
        /// </summary>
        /// <param name="mgr"></param>
        public CustomUserValidator(AppUserManager mgr) : base(mgr)
        {
        }

        /// <summary>
        /// 重写验证方法
        /// </summary>
        /// <param name="user">用户</param>
        /// <returns></returns>
        public override async Task<IdentityResult> ValidateAsync(AppUser user)
        {
            IdentityResult result = await base.ValidateAsync(user);
            //验证结尾
            if (!user.Email.ToLower().EndsWith("@qq.com"))
            {
                var errors = result.Errors.ToList();
                errors.Add("只有qq.com的邮箱支持");
                result = new IdentityResult(errors);
            }
            return result;
        }
    }
}