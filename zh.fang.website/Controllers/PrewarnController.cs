namespace zh.fang.website.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Newtonsoft.Json.Linq;
    using System.Web.Http;

    public class PrewarnController : Controller
    {
        // GET: Prewarm
        //[Filters.AuthFilter()]
        public virtual ActionResult Index()
        {
            var module = new module.ConfigModule();
            var title = module.FetchHomeTitle()?.Data;
            ViewBag.HomeTitle = title;

            var header = GetTableHeader();
            return View(header);
        }
        
        protected virtual Models.ClsTotalTableHeaderModel GetTableHeader()
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
        
        [System.Web.Mvc.HttpGet]
        public virtual JObject GetData()
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
        

        [System.Web.Mvc.HttpPost]
        public JsonResult Upgrade([FromBody]Models.OrgClsTotalSubmitModel changes)
        {
            var module = new module.StatisticsModule();
            var totals = changes.items.Select(t => new handle.Model.OrgClsTotal { clsId = t.id, count = t.value, orgId = changes.id }).ToArray();
            var data = module.UpgradeTotals(totals);
            int code = -1;
            if (data)
            {
                code = 0;
            }
            return Json(new { data = data, code = code, msg = "Ok" }, "text/html");
        }

        [System.Web.Mvc.HttpPost]
        [ValidateInput(false)]
        public JsonResult UpgradeTite([FromBody]string title)
        {
            var data = false;
            var code = 0;
            var msg = "Ok";
            if (!string.IsNullOrWhiteSpace(title))
            {
                title = title.ToLower().Replace("\r", "").Replace("\t", "");
                var reg = new System.Text.RegularExpressions.Regex("[<][/]{0,1}[a-zA-Z]{1,}[>]");
                var matches = reg.Matches(title);
                var len = 0;
                for (int i = 0; i < matches.Count; i++)
                {
                    var item = matches[i].Value;
                    if (!string.IsNullOrWhiteSpace(item))
                    {
                        len += item.Length;
                    }
                }
                len += (title.Length - len) * 2;
                if (1024 <= len)
                {
                    code = -1;
                    msg = "错误：内容太长 !!!";
                }
                else
                {
                    var module = new module.ConfigModule();
                    data = module.ChangeHomeTitle(title);
                }
            }
            
            return Json(new { data = data, code = code, msg = msg }, "text/html");
        }
    }
}