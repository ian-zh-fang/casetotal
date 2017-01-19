﻿
namespace zh.fang.website.Models
{
    using System;
    using System.Linq;
    using zh.fang.handle.Model;

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
            get { return new ClsTotalTableCellModel { field = "_parentId", title = "PID", items = new ClsTotalTableCellModel[0] }; }
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
                    val = 0;
                    if (null != clsval)
                    {
                        val = clsval.TotalCount;
                    }
                    result[cell.field] = clsval.TotalCount;
                    total += val;
                }
            }
            result[header.total.field] = total;

            return result;
        }
    }
}