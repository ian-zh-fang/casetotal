namespace zh.fang.website.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Newtonsoft.Json.Linq;

    public class PrewarnController : Controller
    {
        // GET: Prewarm
        public virtual ActionResult Index()
        {
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
        
        [HttpGet]
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
    }
}