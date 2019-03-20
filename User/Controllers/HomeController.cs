using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace User.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        [Authorize]
        public ActionResult Index()
        {
            //新建字典
            Dictionary<string, object> data = new Dictionary<string, object>();

            //添加一组数据
            data.Add("Placeholder", "Placeholder");
            return View(data);
        }
    }
}