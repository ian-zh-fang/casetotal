namespace zh.fang.website.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Newtonsoft.Json.Linq;

    public class ClsController : Controller
    {
        // GET: Cls
        [Filters.AuthFilter()]
        public ActionResult Index()
        {
            var header = GetTableHeader();
            return View(header);
        }

        private Models.ClsTableHeaderModel GetTableHeader()
        {
            return new Models.ClsTableHeaderModel();
        }

        [HttpGet]
        public JObject GetData()
        {
            var header = GetTableHeader();
            var model = new Models.ClsModelToTableCellModel();
            var module = new module.ClassesModule();
            var items = module.FetchAll().Select(t => model.GetData(t, header)).ToArray();
            var data = new JArray(items);
            var json = new JObject();
            json["total"] = items.Count();
            json["rows"] = data;
            json["footer"] = new JArray();
            return json;
        }

        [HttpPost]
        public JsonResult AddCls(string name, string parentId, int alertVal)
        {
            var module = new module.ClassesModule();
            var data = module.AddCls(name, parentId, alertVal);
            return Json(new { data = data, code = 0, msg = "Ok" });
        }

        [HttpPost]
        public JsonResult DelCls(string id)
        {
            var module = new module.ClassesModule();
            var data = module.DelCls(id);
            return Json(new { data = data, code = 0, msg = "Ok" });
        }

        [HttpPost]
        public JsonResult UpgCls(string id, string name, int alertVal)
        {
            var module = new module.ClassesModule();
            var data = module.UpgCls(id, name, alertVal);
            return Json(new { data = data, code = 0, msg = "Ok" });
        }
    }
}