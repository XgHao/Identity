using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using System.Reflection;
using System.Security.Claims;

namespace User.Infrastructure
{
    /// <summary>
    /// Entity Framework的IdentityRole类中定义了一个Users属性，它能够返回表示角色成员的IdentityUserRole用户对象集合、
    /// 每一个IdentityUserRole对象都有一个UserId属性，他返回一个用户的唯一ID
    /// 该方法为了让我们得到的是每个ID所对应的的用户名
    /// </summary>
    public static class IdentityHelpers
    {
        /// <summary>
        /// HTML方法(辅助器)，调用用户名根据ID
        /// </summary>
        /// <param name="html">当前的HTML</param>
        /// <param name="id">用户ID</param>
        /// <returns></returns>
        public static MvcHtmlString GetUserName(this HtmlHelper html,string id)
        {
            //获取当前用户管理器
            AppUserManager mgr = HttpContext.Current.GetOwinContext().GetUserManager<AppUserManager>();
            //根据ID获取用户名，并返回一个新的MvcHtmlString
            return new MvcHtmlString(mgr.FindByIdAsync(id).Result.UserName);
        }

        public static MvcHtmlString ClaimType(this HtmlHelper html,string claimType)
        {
            FieldInfo[] fields = typeof(ClaimTypes).GetFields();
            foreach(FieldInfo field in fields)
            {
                if (field.GetValue(null).ToString() == claimType)
                {
                    return new MvcHtmlString(field.Name);
                }
            }
            return new MvcHtmlString(string.Format("{0}", claimType.Split('/', '.').Last()));
        }
    }
}