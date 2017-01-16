namespace zh.fang.website.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetData()
        {
            var data = new object[] {
                new {itemid=1,
                    productid =1,
                    listprice =1.8D,
                    unitcost = .2D,
                    attr1 = "attribute",
                    status = 0x0f
                }
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}