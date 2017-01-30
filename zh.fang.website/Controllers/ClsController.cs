namespace zh.fang.website.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Newtonsoft.Json.Linq;

    public class ClsController : Controller
    {
        // GET: Cls
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
    }
}