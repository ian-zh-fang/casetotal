namespace zh.fang.website.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Newtonsoft.Json.Linq;

    public class OrgController : Controller
    {
        // GET: Org
        public ActionResult Index()
        {
            var header = GetTableHeader();
            return View(header);
        }

        private Models.OrgTableHeaderModel GetTableHeader()
        {
            return new Models.OrgTableHeaderModel { };
        }

        public JObject GetData()
        {
            var header = GetTableHeader();
            var model = new Models.OrgModelToTableCellModel();
            var module = new module.OrgModule();
            var items = module.FetchAll().Select(t => model.GetData(t, header)).ToArray();
            var data = new JArray(items);
            var json = new JObject();
            json["total"] = items.Count();
            json["rows"] = data;
            json["footer"] = new JArray();
            return json;
        }
    }
}