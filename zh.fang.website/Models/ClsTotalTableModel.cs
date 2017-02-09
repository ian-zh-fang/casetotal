
namespace zh.fang.website.Models
{
    using System;
    using System.Linq;
    using Newtonsoft.Json.Linq;
    using zh.fang.handle.Model;

    public class ClsTotalTableCellModel: TableCellDataModel
    {
        public ClsTotalTableCellModel[] items { get; set; }
    }

    public class ClsTotalTableHeaderModel
    {
        public ClsTotalTableCellModel id
        {
            get { return new ClsTotalTableCellModel { field = "id", title = "ID", items = new ClsTotalTableCellModel[0] }; }
        }

        public ClsTotalTableCellModel parentId
        {
            get { return new ClsTotalTableCellModel { field = "_parentId", title = "PID", items = new ClsTotalTableCellModel[0] }; }
        }

        public ClsTotalTableCellModel rawId
        {
            get { return new ClsTotalTableCellModel { field = "rawId", title = "rawId", items = new ClsTotalTableCellModel[0] }; }
        }

        public ClsTotalTableCellModel company
        {
            get { return new ClsTotalTableCellModel { field = "company", title = "组织机构", items = new ClsTotalTableCellModel[0] }; }
        }

        public ClsTotalTableCellModel total
        {
            get { return new ClsTotalTableCellModel { field = "total", title = "合计", items = new ClsTotalTableCellModel[0] }; }
        }

        public ClsTotalTableCellModel gVal
        {
            get { return new ClsTotalTableCellModel { field = "gVal", title = "警告阈值", items = new ClsTotalTableCellModel[0] }; }
        }

        public ClsTotalTableCellModel yVal
        {
            get { return new ClsTotalTableCellModel { field = "yVal", title = "危险阈值", items = new ClsTotalTableCellModel[0] }; }
        }

        public ClsTotalTableCellModel oVal
        {
            get { return new ClsTotalTableCellModel { field = "oVal", title = "危害阈值", items = new ClsTotalTableCellModel[0] }; }
        }

        public ClsTotalTableCellModel verif => new ClsTotalTableCellModel { field = "verif", title = "验证数据", items = new ClsTotalTableCellModel[0] };

        public ClsTotalTableCellModel[] items { get; set; }
    }

    public interface IClsTotalTableDataModel<T, TResult>
    {
        TResult GetData(T data, ClsTotalTableHeaderModel header);
    }

    public class OrgClsTotalModel : IClsTotalTableDataModel<OrgClassesTotal, JObject>
    {
        public JObject GetData(OrgClassesTotal data, ClsTotalTableHeaderModel header)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }
            var result = new JObject();
            var hashId = data.OrgId.GetHashCode();
            result[header.id.field] = hashId;
            result[header.rawId.field] = data.OrgId;
            result[header.gVal.field] = data.GVal;
            result[header.yVal.field] = data.YVal;
            result[header.oVal.field] = data.OVal;
            if (!string.IsNullOrWhiteSpace(data.ParentId))
            {
                result[header.parentId.field] = data.ParentId.GetHashCode();
            }
            result[header.company.field] = data.OrgName;
            var total = 0;
            foreach (var item in header.items)
            {
                var clsval = data.ClassesTotals.FirstOrDefault(t => t.ClassId == item.field);
                var val = 0;
                if (null != clsval)
                {
                    val = clsval.TotalCount;
                }
                result[item.field] = val;
                total += val;

                foreach (var cell in item.items)
                {
                    clsval = data.ClassesTotals.FirstOrDefault(t => t.ClassId == cell.field);
                    result[cell.field] = clsval.TotalCount;
                }
            }
            result[header.total.field] = total;
            return result;
        }
    }
}