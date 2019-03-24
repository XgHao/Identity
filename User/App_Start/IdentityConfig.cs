using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using User.Infrastructure;

namespace User.App_Start
{
    /// <summary>
    /// Start Class(启动类)，为OWIN指定配置类 
    /// </summary>
    public class IdentityConfig
    {
        /// <summary>
        /// 该方法由OWIN技术架构进行调用，并为该方法传递一个Owin.IAppBuilder接口的实现，由它支持应用程序所需中间件的设置
        /// </summary>
        /// <param name="app"></param>
        public void Configuration(IAppBuilder app)
        {
            //CreatePerOwinContext用于创建AppUserManager的新实例
            //AppIdentityDbContext类用于每一个请求，能保证每一个请求对ASP.NET Identity数据有清晰的访问
            app.CreatePerOwinContext<AppIdentityDbContext>(AppIdentityDbContext.Create);
            //注册用户管理器类
            app.CreatePerOwinContext<AppUserManager>(AppUserManager.Create);
            //注册角色管理器类，这样能够确保，可以使用与AppUserManager类所用的用一个Entity Framework数据库上下文
            app.CreatePerOwinContext<AppRoleManager>(AppRoleManager.Create);

            //该方法告诉ASP.NET Identity如何用Cookie去标识已认证的用户
            //以及通过CookieAuthenticationOptions类来指定的选项
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
            });

            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
            //app.UseGoogleAuthentication(new Microsoft.Owin.Security.Google.GoogleOAuth2AuthenticationOptions());
        }
    }
}