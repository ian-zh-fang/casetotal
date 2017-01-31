namespace zh.fang.website.Controllers
{
    using System.Web.Http;
    using System.Web.Mvc;

    public class AdminController : Controller
    {
        // GET: Admin
        [Filters.AuthFilter()]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult Signin([FromBody]string userid, [FromBody]string passwd)
        {
            var module = new module.UserModule();
            var usr = module.Signin(userid, passwd);
            var data = false;
            if (usr != null)
            {
                data = true;
                EnsureUser(usr);
            }
            return Json(new { data = data, code = 0, msg = "Ok", url="/admin" });
        }

        [Filters.AuthFilter()]
        public ActionResult SetPasswd()
        {
            return View();
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult ChangePasswd([FromBody]string oldpwd, [FromBody]string newpwd, [FromBody]string cfmpwd)
        {
            var data = false;
            var code = 0;
            var usr = EnsureUser(null);
            if ( usr != null && !string.IsNullOrWhiteSpace(cfmpwd) && newpwd == cfmpwd)
            {
                var module = new module.UserModule();
                data = module.UpgPasswd(usr.Id, oldpwd, newpwd);
            }
            return Json(new { data = data, code = code, msg = "Ok" });
        }

        [Filters.AuthFilter()]
        [System.Web.Mvc.HttpGet]
        public ActionResult Signout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login");
        }

        private data.entity.User EnsureUser(data.entity.User usr)
        {
            var user = Session[GloableConfig.UserKey] as data.entity.User;
            if (user == null && usr != null)
            {
                Session[GloableConfig.UserKey] = usr;
                user = usr;
            }

            return user;
        }
    }
}