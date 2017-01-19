namespace zh.fang.website.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var model = GetTableHeader();
            return View(model);
        }

        private Models.ClsTotalTableHeaderModel GetTableHeader()
        {
            var clsModule = new module.ClassesModule();
            var clsItems = clsModule.FetchAll();
            var model = new Models.ClsTotalTableHeaderModel
            {
                items = clsItems.Where(t => t.ParentId == null).Select(t => new Models.ClsTotalTableCellModel
                {
                    field = t.Id,
                    title = t.Name,
                    items = clsItems.Where(x => x.ParentId == t.Id).Select(x => new Models.ClsTotalTableCellModel
                    {
                        field = x.Id,
                        title = x.Name,
                        items = new Models.ClsTotalTableCellModel[0]
                    }).ToArray()
                }).ToArray()
            };
            return model;
        }

        public ActionResult Admin()
        {
            return RedirectToAction("index", "admin");
        }

        public JsonResult GetData()
        {
            var items = new List<object>();
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ClsTotalOnToday()
        {
            var data = new Models.ClsTotalOnTodayModel
            {
                itotals = new Models.ClsTotalValue[] {
                    new Models.ClsTotalValue { name="itotal1", value=0 },
               new Models.ClsTotalValue { name="itotal2", value=0 },
                    new Models.ClsTotalValue { name="itotal3", value=0 }
                },
                ototals = new Models.ClsTotalValue[] {
                    new Models.ClsTotalValue { name="ototal1", value=0 },
                    new Models.ClsTotalValue { name="ototal2", value=0 },
                    new Models.ClsTotalValue { name="ototal3", value=0 },
                    new Models.ClsTotalValue { name="ototal4", value=0 },
                    new Models.ClsTotalValue { name="ototal5", value=0 },
                    new Models.ClsTotalValue { name="ototal6", value=0 }
                }
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ClsTotalCompareOnWeek()
        {
            var data = new Models.ClsTotalCompareModel
            {
                title = "周环比",
                categories = new string[] { "刑事", "治安", "其它", "杀人", "两抢", "交通", "火灾" },
                item1 = new Models.ClsTotalCompareValue<int> { name = "本周", totals = new int[] {10,2,5,2,7,4,3 } },
                item2 = new Models.ClsTotalCompareValue<int> { name = "上周", totals = new int[] {4,6,9,0,3,7,0 } }
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ClsTotalCompareOnMonth()
        {
            var data = new Models.ClsTotalCompareModel
            {
                title = "月环比",
                categories = new string[] { "刑事", "治安", "其它", "杀人", "两抢", "交通", "火灾" },
                item1 = new Models.ClsTotalCompareValue<int> { name = "本月", totals = new int[] { 10, 20, 5, 25, 7, 47, 3 } },
                item2 = new Models.ClsTotalCompareValue<int> { name = "上月", totals = new int[] { 4, 6, 90, 0, 3, 7, 20 } }
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}