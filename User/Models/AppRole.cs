using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace User.Models
{
    /// <summary>
    /// ASP.NET Identity为访问和管理角色提供了一个强类型的基类，叫做RoleManager<T>，
    /// 其中T是IRole接口的实现，该实现得到了用来表示角色的存储机制的支持
    /// Entity Framework实现了IRole接口，使用的是一个名称为IdentityRole的类
    /// 
    /// 应用程序专用的角色类派生于IdentityROle
    /// </summary>
    public class AppRole : IdentityRole
    {
        public AppRole() : base() { }

        public AppRole(string name) : base(name) { }
    }
}