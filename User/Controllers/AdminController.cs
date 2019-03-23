using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using User.Infrastructure;
using User.Models;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace User.Controllers
{
    [Authorize(Roles = "Administrators")]
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View(UserManager.Users);
        }

        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// 提交表单数据调用的方法[HttpPost]
        /// </summary>
        /// <param name="model">创建用户的验证模型</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Create(CreateModel model)
        {
            //验证模型无误
            if (ModelState.IsValid)
            {
                //从model中传递数据到AppUser中
                AppUser user = new AppUser
                {
                    UserName = model.Name,
                    Email = model.Email,
                };
                //创建用户，返回的家
                IdentityResult result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //成功后重定向到Index
                    return RedirectToAction("Index");
                }
                else
                {
                    //失败添加错误描述
                    AddErrorsFromResult(result);
                }
            }
            return View(model);
        }

        /// <summary>
        /// 编辑动作
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns></returns>
        public async Task<ActionResult> Edit(string id)
        {
            //根据用户ID获取操作对象
            AppUser user = await UserManager.FindByIdAsync(id);
            if(user != null)
            {
                return View(user);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// 编辑动作[HttpPost]
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <param name="email">邮箱</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Edit(string id, string email, string password)
        {
            //根据用户ID获取操作的对象
            AppUser user = await UserManager.FindByIdAsync(id);

            //对象不为空
            if (user != null)
            {
                //验证邮箱
                user.Email = email;
                IdentityResult validEmail = await UserManager.UserValidator.ValidateAsync(user);
                //验证失败
                if (!validEmail.Succeeded)
                {
                    AddErrorsFromResult(validEmail);
                }

                //验证密码
                IdentityResult validPass = null;
                if (password != string.Empty)
                {
                    validPass = await UserManager.PasswordValidator.ValidateAsync(password);
                    //验证成功
                    if (validPass.Succeeded)
                    {
                        //存储密码的hash值
                        user.PasswordHash = UserManager.PasswordHasher.HashPassword(password);
                    }
                    //验证失败
                    else
                    {
                        AddErrorsFromResult(validPass);
                    }
                }

                //所有验证通过时
                if ((validEmail.Succeeded && validPass == null) || (validEmail.Succeeded && password != string.Empty && validPass.Succeeded))
                {
                    //更新用户信息
                    IdentityResult result = await UserManager.UpdateAsync(user);
                    //更新成功
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        AddErrorsFromResult(result);
                    }
                }
            }
            //对象为空
            else
            {
                ModelState.AddModelError("", "User Not Found");
            }
            return View(user);
        }

        /// <summary>
        /// 删除用户方法[HttpPost]
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            //根据id找到用户对象
            AppUser user = await UserManager.FindByIdAsync(id);
            //对象不为空
            if (user != null)
            {
                //删除操作
                IdentityResult result = await UserManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("Error", result.Errors);
                }
            }
            //对象为空
            else
            {
                return View("Error", new string[] { "User Not Found" });
            }
        }

        /// <summary>
        /// 添加错误描述
        /// </summary>
        /// <param name="result">身份操作的结果</param>
        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach(string error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        /// <summary>
        /// 因为要反复使用AppUserManager类，所以定义UserManager属性，以便简化代码
        /// </summary>
        private AppUserManager UserManager
        {
            get
            {
                //扩展方法GetUserManager<T>，可以用来得到用户管理器类的实例
                return HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            }
        }
    }
}