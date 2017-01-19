using System;
using zh.fang.handle.Model;

namespace zh.fang.website.Models
{

    public class ClsTotalTableCellModel
    {
        public string field { get; set; }

        public string title { get; set; }

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
            get { return new ClsTotalTableCellModel { field = "parentId", title = "PID", items = new ClsTotalTableCellModel[0] }; }
        }

        public ClsTotalTableCellModel company
        {
            get { return new ClsTotalTableCellModel { field = "company", title = "组织机构", items = new ClsTotalTableCellModel[0] }; }
        }

        public ClsTotalTableCellModel total
        {
            get { return new ClsTotalTableCellModel { field = "total", title = "合计", items = new ClsTotalTableCellModel[0] }; }
        }

        public ClsTotalTableCellModel[] items { get; set; }
    }

    public interface IClsTotalTableDataModel<T>
    {
        object GetData(T data, ClsTotalTableHeaderModel header);
    }

    public class OrgClsTotalModel : IClsTotalTableDataModel<OrgClassesTotal>
    {
        public object GetData(OrgClassesTotal data, ClsTotalTableHeaderModel header)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            dynamic result = new { };
            result[header.id.field] = data.OrgId;
            result[header.parentId.field] = data.ParentId;
            result[header.company.field] = data.OrgName;
            

            return result;
        }
    }
}