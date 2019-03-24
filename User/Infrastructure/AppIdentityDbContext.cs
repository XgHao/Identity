using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using User.Models;
using System.Data.Entity;
using Microsoft.AspNet.Identity;

namespace User.Infrastructure
{
    /// <summary>
    /// Entity Framework数据库的上下文类，用于对AppUser类进行操作。
    /// 这里用Code First特性来创建和管理数据库架构，并提供数据库数据的访问
    /// 这里上下文派生与IdentityDbContext<T>，这里的T就是用户类(AppUser.cs)
    /// </summary>
    public class AppIdentityDbContext : IdentityDbContext<AppUser>
    {
        //该类的构造器(函数)调用了他的基类，参数是连接字符串名称，用于连接数据库
        public AppIdentityDbContext() : base("IdentityDb") { }

        /// <summary>
        /// 静态构造器(函数)
        /// </summary>
        static AppIdentityDbContext()
        {
            //SetInitializer方法指定一个种植数据库类(向数据库中植入数据，即用数据对数据库初始化)
            Database.SetInitializer(new IdentityDbInit());
        }

        /// <summary>
        /// 这是由OWIN在必要时创建类实例的方法
        /// </summary>
        /// <returns></returns>
        public static AppIdentityDbContext Create()
        {
            return new AppIdentityDbContext();
        }
    }

    /// <summary>
    /// NullDatabaseInitializer<AppIdentityDbContext>可以防止架构修改
    /// </summary>
    public class IdentityDbInit : NullDatabaseInitializer<AppIdentityDbContext>
    {

    }

    /// <summary>
    /// 种植类（数据库初始化）当通过Entity Framework的Code First特性第一次创建数据库架构时，会使用到这个类
    /// DropCreateDatabaseIfModelChanges类会在Code First类改变时删除整个数据库，注释以防止他影响数据库
    /// </summary>
    //public class IdentityDbInit : DropCreateDatabaseIfModelChanges<AppIdentityDbContext>
    //{
    //    protected override void Seed(AppIdentityDbContext context)
    //    {
    //        PerformInitialSetup(context);
    //        base.Seed(context);
    //    }

    //    /// <summary>
    //    /// 放置初始化的配置
    //    /// </summary>
    //    /// <param name="context"></param>
    //    public void PerformInitialSetup(AppIdentityDbContext context)
    //    {
    //        AppUserManager userMgr = new AppUserManager(new UserStore<AppUser>(context));
    //        AppRoleManager roleMgr = new AppRoleManager(new RoleStore<AppRole>(context));

    //        string roleName = "Administrators";
    //        string userName = "XgHao";
    //        string password = "zxhzxh";
    //        string eamil = "957553851@qq.com";

    //        if (!roleMgr.RoleExists(roleName))
    //        {
    //            roleMgr.Create(new AppRole(roleName));
    //        }

    //        AppUser user = userMgr.FindByName(userName);
    //        if (user == null)
    //        {
    //            userMgr.Create(new AppUser
    //            {
    //                UserName = userName,
    //                Email = eamil,
    //            }, password);
    //            user = userMgr.FindByName(userName);
    //        }

    //        if (!userMgr.IsInRole(user.Id, roleName))
    //        {
    //            userMgr.AddToRole(user.Id, roleName);
    //        }
    //    }
    //}
}