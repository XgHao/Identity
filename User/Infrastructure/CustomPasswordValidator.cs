using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace User.Infrastructure
{
    /// <summary>
    /// 自定义密码验证器，派生于PasswordValidator
    /// </summary>
    public class CustomPasswordValidator : PasswordValidator
    {
        /// <summary>
        /// 重写验证方法
        /// </summary>
        /// <param name="pass">密码</param>
        /// <returns></returns>
        public override async Task<IdentityResult> ValidateAsync(string pass)
        {
            //调用基实现，使之能够受益于内建的验证检查
            IdentityResult result = await base.ValidateAsync(pass);

            //自定义的验证
            if (pass.Contains("12345"))
            {
                var errors = result.Errors.ToList();
                errors.Add("密码不能包括连续数字");
                result = new IdentityResult(errors);
            }
            return result;
        }
    }
}