using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using User.Infrastructure;
using User.Models;

namespace User.Controllers
{
    /// <summary>
    /// 管理角色控制器
    /// </summary>
    [Authorize(Roles = "Administrators")]
    public class RoleAdminController : Controller
    {
        /// <summary>
        /// 主页Index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View(RoleManager.Roles);
        }

        /// <summary>
        /// 创建角色
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// 创建角色[HttpPost]
        /// </summary>
        /// <param name="name">名称(必须)</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Create([Required]string name)
        {
            //验证模型无误
            if (ModelState.IsValid)
            {
                //根据名称创建角色，并返回结果
                IdentityResult result = await RoleManager.CreateAsync(new AppRole(name));
                //创建成功，重定向URL
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                //创建失败，添加验证模型
                else
                {
                    AddErrorFromResult(result);
                }
            }
            return View(name);
        }

        /// <summary>
        /// 删除角色[HttpPost]
        /// </summary>
        /// <param name="id">删除角色的ID</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            //根据ID获取操作的对象
            AppRole role = await RoleManager.FindByIdAsync(id);
            //对象存在
            if (role != null)
            {
                //删除对象，返回结果
                IdentityResult result = await RoleManager.DeleteAsync(role);
                //成功
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                //失败
                else
                {
                    return View("Error", result.Errors);
                }
            }
            //不存在，返回错误
            else
            {
                return View("Error", new string[] { "Role Not Found" });
            }
        }

        /// <summary>
        /// 编辑信息
        /// </summary>
        /// <param name="id">角色ID</param>
        /// <returns></returns>
        public async Task<ActionResult> Edit(string id)
        {
            //根据ID获取操作的对象
            AppRole role = await RoleManager.FindByIdAsync(id);
            //获取当前角色的所有用户
            string[] memberIDs = role.Users.Select(x => x.UserId).ToArray();
            //ID相同
            IEnumerable<AppUser> members = UserManager.Users.Where(x => memberIDs.Any(y => y == x.Id));
            //ID不同
            IEnumerable<AppUser> nonMembers = UserManager.Users.Except(members);
            return View(new RoleEditModel
            {
                Role = role,
                Members = members,
                NonMembers = nonMembers
            });
        }

        /// <summary>
        /// 编辑[HttpPost]
        /// </summary>
        /// <param name="model">验证编辑模型</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Edit(RoleModificationModel model)
        {
            IdentityResult result;      //新建结果
            //验证模型无误
            if (ModelState.IsValid)
            {
                //遍历所有要添加到角色的用户，若为NULL则替换为空数组
                foreach(string userId in model.IdsToAdd ?? new string[] { })
                {
                    //添加用户到角色，userId=用户ID，model.RoleName=角色名，返回结果
                    result = await UserManager.AddToRoleAsync(userId, model.RoleName);
                    //添加失败，添加错误模型
                    if (!result.Succeeded)
                    {
                        return View("Error", result.Errors);
                    }
                }
                //遍历所有在角色中要删除用户，若为NULL则替换为空数组
                foreach (string userId in model.IdsToDelete ?? new string[] { })
                {
                    //删除用户从角色，userId=用户ID，model.RoleName=角色名，返回结果
                    result = await UserManager.RemoveFromRolesAsync(userId, model.RoleName);
                    //删除失败，添加错误模型
                    if (!result.Succeeded)
                    {
                        return View("Error", result.Errors);
                    }
                }
            }
            return View("Error", new string[] { "Role Not Found" });
        }

        /// <summary>
        /// 添加错误的验证
        /// </summary>
        /// <param name="result">错误验证结果</param>
        private void AddErrorFromResult(IdentityResult result)
        {
            foreach(string error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        /// <summary>
        /// 获取AppUserManager类的实例
        /// </summary>
        private AppUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            }
        }

        /// <summary>
        /// 获取APPRoleManager类的实例，在动作方法中用该实例获取并维护应用程序的角色
        /// </summary>
        private AppRoleManager RoleManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AppRoleManager>();
            }
        }
    }
}