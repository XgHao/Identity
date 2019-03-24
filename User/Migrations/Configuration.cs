using System.Data.Entity.Migrations;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using User.Infrastructure;
using User.Models;

namespace User.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<User.Infrastructure.AppIdentityDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "User.Infrastructure.AppIdentityDbContext";
        }

        /// <summary>
        /// 这个类用于把数据库中先有内用迁移到新的数据库架构
        /// </summary>
        /// <param name="context"></param>
        protected override void Seed(User.Infrastructure.AppIdentityDbContext context)
        {
            //  This method will be called after migrating to the latest version.
            //  此方法会在迁移到最新版本是调用

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            //  你可以使用DbSet<T>.AddOrUpdate()辅助器方法来避免创建重复种子数据

            //获取角色管理器和用户管理器
            AppUserManager userMgr = new AppUserManager(new UserStore<AppUser>(context));
            AppRoleManager roleMgr = new AppRoleManager(new RoleStore<AppRole>(context));

            string roleName = "Administrators";
            string userName = "XgHao";
            string password = "zxhzxh";
            string email = "957553851@qq.com";

            //当前角色名不存在，则创建
            if (!roleMgr.RoleExists(roleName))
            {
                roleMgr.Create(new AppRole(roleName));
            }
            
            //根据用户名查找用户对象
            AppUser user = userMgr.FindByName(userName);
            //不存在，则创建
            if (user == null)
            {
                userMgr.Create(new AppUser
                {
                    UserName = userName,
                    Email = email
                }, password);
            }

            //用户是否存在当前角色，不存在则加入
            if (!userMgr.IsInRole(user.Id, roleName))
            {
                userMgr.AddToRole(user.Id, roleName);
            }

            //遍历所有用户，将新的属性值赋值
            foreach(AppUser dbUser in userMgr.Users)
            {
                if (dbUser.Country == Countries.NONE)
                {
                    dbUser.SetCountryFromCity(dbUser.City);
                }
            }
            context.SaveChanges();
        }
    }
}
