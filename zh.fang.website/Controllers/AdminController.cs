namespace zh.fang.website.Controllers
{
    using System.Web.Http;
    using System.Web.Mvc;

    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult Signin([FromBody]string userid, [FromBody]string passwd)
        {
            return RedirectToAction("index");
        }

        public ActionResult SetPasswd()
        {
            return View();
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult ChangePasswd([FromBody]string oldpwd, [FromBody]string newpwd, [FromBody]string cfmpwd)
        {
            return RedirectToAction("index", "cls");
        }

        [System.Web.Mvc.HttpGet]
        public ActionResult Signout()
        {
            return RedirectToAction("Login");
        }
    }
}