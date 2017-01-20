namespace zh.fang.website.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Newtonsoft.Json.Linq;

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
            return RedirectToAction("login", "admin");
        }

        [HttpGet]
        public JObject GetData()
        {
            var module = new module.StatisticsModule();
            var header = GetTableHeader();
            var model = new Models.OrgClsTotalModel();
            var items = module.OrgClassTotalOnToday().Select(t => model.GetData(t, header)).ToArray();
            var data = new JArray(items);
            var json = new JObject();
            json["total"] = items.Count();
            json["rows"] = data;
            json["footer"] = new JArray();
            return json;
        }

        [HttpGet]
        public JsonResult ClsTotalOnToday()
        {
            var module = new module.StatisticsModule();
            var items = module.ClassesTotalOnToday();
            var data = new Models.ClsTotalOnTodayModel
            {
                itotals = items.Where(t => t.ParentId == null).Select(t => new Models.ClsTotalValue { name = t.ClassName, value = t.TotalCount }).ToArray(),
                ototals = items.Where(t => t.ParentId != null).Select(t => new Models.ClsTotalValue { name = t.ClassName, value = t.TotalCount }).ToArray()
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ClsTotalCompareOnWeek()
        {
            var module = new module.StatisticsModule();
            var items = module.ClassesTotalOnCurrentWeek();
            var preitems = module.ClassesTotalOnPreviousWeek();
            var data = new Models.ClsTotalCompareModel
            {
                title = "周环比",
                categories = items.Select(t => t.ClassName).ToArray(),
                item1 = new Models.ClsTotalCompareValue<int> { name = "本周", totals = items.Select(t => t.TotalCount).ToArray() },
                item2 = new Models.ClsTotalCompareValue<int> { name = "上周", totals = preitems.Select(t => t.TotalCount).ToArray() }
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ClsTotalCompareOnMonth()
        {
            var module = new module.StatisticsModule();
            var items = module.ClassesTotalOnCurrentMonth();
            var preitems = module.ClassesTotalOnPreMonth();
            var data = new Models.ClsTotalCompareModel
            {
                title = "月环比",
                categories = items.Select(t => t.ClassName).ToArray(),
                item1 = new Models.ClsTotalCompareValue<int> { name = "本月", totals = items.Select(t => t.TotalCount).ToArray() },
                item2 = new Models.ClsTotalCompareValue<int> { name = "上月", totals = preitems.Select(t => t.TotalCount).ToArray() }
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}