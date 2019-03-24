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
        /// ��������ڰ����ݿ�����������Ǩ�Ƶ��µ����ݿ�ܹ�
        /// </summary>
        /// <param name="context"></param>
        protected override void Seed(User.Infrastructure.AppIdentityDbContext context)
        {
            //  This method will be called after migrating to the latest version.
            //  �˷�������Ǩ�Ƶ����°汾�ǵ���

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            //  �����ʹ��DbSet<T>.AddOrUpdate()���������������ⴴ���ظ���������

            //��ȡ��ɫ���������û�������
            AppUserManager userMgr = new AppUserManager(new UserStore<AppUser>(context));
            AppRoleManager roleMgr = new AppRoleManager(new RoleStore<AppRole>(context));

            string roleName = "Administrators";
            string userName = "XgHao";
            string password = "zxhzxh";
            string email = "957553851@qq.com";

            //��ǰ��ɫ�������ڣ��򴴽�
            if (!roleMgr.RoleExists(roleName))
            {
                roleMgr.Create(new AppRole(roleName));
            }
            
            //�����û��������û�����
            AppUser user = userMgr.FindByName(userName);
            //�����ڣ��򴴽�
            if (user == null)
            {
                userMgr.Create(new AppUser
                {
                    UserName = userName,
                    Email = email
                }, password);
            }

            //�û��Ƿ���ڵ�ǰ��ɫ�������������
            if (!userMgr.IsInRole(user.Id, roleName))
            {
                userMgr.AddToRole(user.Id, roleName);
            }

            //���������û������µ�����ֵ��ֵ
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
