using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using User.Models;
using Microsoft.Owin.Security;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using User.Infrastructure;

namespace User.Controllers
{
    [Authorize]//管理用户账号的控制器只能由已认证用户才能使用
    public class AccountController : Controller
    {
        /// <summary>
        /// 登录操作
        /// </summary>
        /// <param name="returnUrl">返回Url(当用户请求一个受限的URL时，他们被重定向到/Account/Login URL上带有查询字符串)</param>
        /// <returns></returns>
        [AllowAnonymous]//默认限制到已认证用户，但又能允许未认证用户登录到应用程序
        public ActionResult Login(string returnUrl)
        {
            //检查用户是否已经认证
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return View("Error", new string[] { "Access Denied" });
            }
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        /// <summary>
        /// 登录操作[HttpPost]
        /// </summary>
        /// <param name="deteils">登录模型</param>
        /// <param name="returnUrl">返回Url</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]//该属性与视图中的Html.AntiForgeryToken辅助器方法联合工作，防止Cross-Site Request Forgery(CSRF,跨网站请求伪造)
        public async Task<ActionResult> Login(LoginModel deteils,string returnUrl)
        {
            //模型验证通过
            if (ModelState.IsValid)
            {
                //根据用户名与密码获取操作对象
                AppUser user = await UserManager.FindAsync(deteils.Name, deteils.Password);
                //对象为空
                if (user == null)
                {
                    ModelState.AddModelError("", "Invalid name or password.");
                }
                //对象存在
                else
                {
                    //创建Cookie，浏览器会在后继的请求中发送这个Cookie，表明他们是已认证的
                    //创建一个标识该用户的ClaimsIdentity对象，该实例是调用用户管理器的CreateIdentityAsync方法创建的
                    //传递一个用户对象和DefaultAuthenticationTypes枚举中的一个值
                    ClaimsIdentity ident = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
                    ident.AddClaims(LocationClaimsProvider.GetClaims(ident));
                    
                    //签出用户，这通常意味着使标识已认证用户的Cookie失效
                    AuthManager.SignOut();

                    //签入用户，这意味着要创建用来标识已认证请求的Cookie
                    AuthManager.SignIn(
                            new AuthenticationProperties    //该类用来配置认证过程以及ClaimsIdentity对象
                            {
                                IsPersistent = false    //使认证Cookie在浏览器中是持久化的，意即用户在开始新会话时不必再次认证
                            },
                            ident
                        );
                    return Redirect(returnUrl);
                }
            }
            ViewBag.returnUrl = returnUrl;
            return View(deteils);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult GoogleLogin(string returnUrl)
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleLoginCallback", new { returnUrl = returnUrl })
            };
            HttpContext.GetOwinContext().Authentication.Challenge(properties, "Google");
            return new HttpUnauthorizedResult();
        }

        [AllowAnonymous]
        public async Task<ActionResult> GoogleLoginCallback(string returnUrl)
        {
            ExternalLoginInfo loginInfo = await AuthManager.GetExternalLoginInfoAsync();
            AppUser user = await UserManager.FindAsync(loginInfo.Login);
            if (user == null)
            {
                user = new AppUser
                {
                    Email = loginInfo.Email,
                    UserName = loginInfo.DefaultUserName,
                    City = Cities.LONDON,
                    Country = Countries.UK
                };
                IdentityResult result = await UserManager.CreateAsync(user);
                if (!result.Succeeded)
                {
                    return View("Error", result.Errors);
                }
                else
                {
                    result = await UserManager.AddLoginAsync(user.Id, loginInfo.Login);
                    if (!result.Succeeded)
                    {
                        return View("Error", result.Errors);
                    }
                }
            }

            ClaimsIdentity ident = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            ident.AddClaims(loginInfo.ExternalIdentity.Claims);
            AuthManager.SignIn(new AuthenticationProperties
            {
                IsPersistent = false
            }, ident);
            return Redirect(returnUrl ?? "/");
        }

        /// <summary>
        /// 注销用户
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Logout()
        {
            AuthManager.SignOut();
            return RedirectToAction("Index", "Home");
        }


        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 因为在控制器中会反复使用AuthManager属性
        /// </summary>
        private IAuthenticationManager AuthManager
        {
            get
            {
                //返回的IAuthenticationManager接口的实现
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        /// <summary>
        /// 因为在控制器中会反复使用AppUserManager类，因此定义UserManager属性
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