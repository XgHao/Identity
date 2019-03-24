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
    public class AppRoleManager : RoleManager<AppRole>, IDisposable
    {
        public AppRoleManager(RoleStore<AppRole> store) : base(store)
        {
        }

        /// <summary>
        /// Create方法，他让OWIN启动类能够为每一个访问Identity数据的请求创建实例
        /// 这意味着在整个应用程序中，我不必散步如何存储角色数据的细节，却能获取APPRoleManager类的实例
        /// </summary>
        /// <param name="options"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static AppRoleManager Create(IdentityFactoryOptions<AppRoleManager> options,IOwinContext context)
        {
            return new AppRoleManager(new RoleStore<AppRole>(context.Get<AppIdentityDbContext>()));
        }
    }
}