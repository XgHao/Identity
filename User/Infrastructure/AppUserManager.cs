using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using User.Models;

namespace User.Infrastructure
{
    /// <summary>
    /// "User Manager(用户管理器)"，用来管理用户类实例，
    /// 用户管理器必须派生与UserManager<T>，其中T为用户类(AppUser.cs)
    /// </summary>
    public class AppUserManager : UserManager<AppUser>
    {
        public AppUserManager(IUserStore<AppUser> store) : base(store)
        {
        }

        /// <summary>
        /// 在Identity需要一个AppUserManager实例时，将会调用静态的Create方法
        /// 这种情况将在对用户数据执行操作时发生
        /// </summary>
        /// <param name="options"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static AppUserManager Create(IdentityFactoryOptions<AppUserManager> options, IOwinContext context)
        {
            //2.为了创建UserStore<T>，又需要AppIdentityDbContext类的实例
            AppIdentityDbContext db = context.Get<AppIdentityDbContext>();

            //1.为了创建AppUserManager类的实例，需要一个UserStore<AppUser>实例。
            //这个UserStore<T>类是IUserStore<T>接口的EntityFramework实现的，它提供了UserManager类所定义的存储方法实现
            AppUserManager manager = new AppUserManager(new UserStore<AppUser>(db));

            //密码验证
            manager.PasswordValidator = new CustomPasswordValidator
            {
                RequiredLength = 6,                 //最小长度
                RequireNonLetterOrDigit = false,    //是否必须含有非字母和数字的字符
                RequireLowercase = true,            //是否必须含有小写字母
                RequireUppercase = true,            //是否必须含有大写字母
                RequireDigit = false                //是否必须含有数字
            };

            //用户验证
            //manager.UserValidator = new CustomUserValidator(manager)
            //{
            //    AllowOnlyAlphanumericUserNames = true,        //是否用户名只能含有字母数字字符
            //    RequireUniqueEmail = true                     //邮件地址是否唯一
            //};



            return manager;
        }
    }
}