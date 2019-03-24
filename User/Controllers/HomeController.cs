using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Principal;
using System.Threading.Tasks;
using User.Infrastructure;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using User.Models;

namespace User.Controllers
{
    /// <summary>
    /// 该控制器用来描述用户帐号的细节和数据
    /// </summary>
    public class HomeController : Controller
    {
        // GET: Home
        [Authorize]
        public ActionResult Index()
        {
            return View(GetData("Index"));
        }

        [Authorize(Roles = "Users")]
        public ActionResult OtherAction()
        {
            return View("Index", GetData("OtherAction"));
        }

        /// <summary>
        /// 字典集添加一些关于用户标识的基本信息
        /// 这是通过HttpContext对象可用的属性获取
        /// </summary>
        /// <param name="actionName"></param>
        /// <returns></returns>
        private Dictionary<string,object> GetData(string actionName)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("Action", actionName);
            dict.Add("User", HttpContext.User.Identity.Name);
            dict.Add("Authenticated", HttpContext.User.Identity.IsAuthenticated);
            dict.Add("Auth Type", HttpContext.User.Identity.AuthenticationType);
            dict.Add("In Users Role", HttpContext.User.IsInRole("Users"));
            return dict;
        }

        /// <summary>
        /// 用户自定义属性
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult UserProps()
        {
            return View(CurrentUser);
        }

        /// <summary>
        /// 用户自定义属性[HttpPost]
        /// </summary>
        /// <param name="city">城市</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> UserProps(Cities city)
        {
            AppUser user = CurrentUser;
            user.City = city;
            user.SetCountryFromCity(city);
            //更新数据
            await UserManager.UpdateAsync(user);
            return View(user);
        }

        /// <summary>
        /// 当前用户
        /// </summary>
        private AppUser CurrentUser
        {
            get
            {
                //根据用户姓名获取对象
                return UserManager.FindByName(HttpContext.User.Identity.Name);
            }
        }

        /// <summary>
        /// 当前用户管理器
        /// </summary>
        private AppUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            }
        }
    }
}