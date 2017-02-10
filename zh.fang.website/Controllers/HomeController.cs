namespace zh.fang.website.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Newtonsoft.Json.Linq;
    using zh.fang.common;

    public class HomeController : PrewarnController
    {
        public override ActionResult Index()
        {
            var module = new module.ConfigModule();
            var title = module.FetchHomeTitle()?.Data ?? "";
            ViewBag.HomeTitle = title.Replace("\r\n", "<br />");

            var model = GetTableHeader();
            return View(model);
        }

        protected override Models.ClsTotalTableHeaderModel GetTableHeader()
        {
            return base.GetTableHeader();
        }

        public ActionResult Admin()
        {
            return RedirectToAction("index", "admin");
        }

        [HttpGet]
        public override JObject GetData()
        {
            var module = new module.StatisticsModule();
            var header = GetTableHeader();
            var model = new Models.OrgClsTotalModel();
            const int weekinterval = 7;
            const int weeks = 5;
            var seedTime = System.DateTime.Now.FirstDayCurrentweeek().AddDays(0 - weeks * weekinterval);
            var vitems = module.OrgClassTotalOnWeeks(weeks, seedTime).Select(t => model.GetData(t, header)).ToArray();
            var items =
                module.OrgClassTotalOnWeeks(weeks).Select(
                    t =>
                    {
                        var d = model.GetData(t, header);
                        var orgId = d.GetValue(header.rawId.field).Value<string>();
                        d[header.verif.field] = vitems.FirstOrDefault(x => orgId == x.GetValue(header.rawId.field).Value<string>()) ?? new JObject();
                        return d;
                    }).ToArray();
            var data = new JArray(items);
            var json = new JObject();
            json["total"] = items.Count();
            json["rows"] = data;
            json["footer"] = new JArray();
            return json;
        }

        [HttpGet]
        public JObject GetDataOnYesterday()
        {
            var module = new module.StatisticsModule();
            var header = GetTableHeader();
            var model = new Models.OrgClsTotalModel();
            var items = module.OrgClassTotalOnYesterday().Select(t => model.GetData(t, header)).ToArray();
            var data = new JArray(items);
            var json = new JObject();
            json["total"] = items.Count();
            json["rows"] = data;
            json["footer"] = new JArray();
            return json;
        }

        [HttpGet]
        public JObject GetDataOnCurrentWeek()
        {
            var module = new module.StatisticsModule();
            var header = GetTableHeader();
            var model = new Models.OrgClsTotalModel();
            var items = module.OrgClassTotalOnCurrentweek().Select(t => model.GetData(t, header)).ToArray();
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