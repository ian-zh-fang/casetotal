namespace zh.fang.website.Models
{
    using Newtonsoft.Json.Linq;
    using zh.fang.data.entity;

    public class ClsTableHeaderModel
    {
        public TableCellDataModel id => new TableCellDataModel { field="id", title="ID" };

        public TableCellDataModel name => new TableCellDataModel { field="name", title="类型名称" };

        public TableCellDataModel code => new TableCellDataModel { field="code", title="类型编码" };

        public TableCellDataModel parentId => new TableCellDataModel { field = "_parentId", title = "PID" };

        public TableCellDataModel alertVal => new TableCellDataModel { field="alertVal", title="预警值（预设/未启用）" };
    }

    public interface IClsModelToTableCellModel<T, TResult>
    {
        TResult GetData(T data, ClsTableHeaderModel header);
    }

    public class ClsModelToTableCellModel : IClsModelToTableCellModel<CaseClasses, JObject>
    {
        public JObject GetData(CaseClasses data, ClsTableHeaderModel header)
        {
            var json = new JObject();
            json[header.id.field] = data.Id;
            json[header.name.field] = data.Name;
            json[header.parentId.field] = data.ParentId;
            json[header.alertVal.field] = data.ThdVal;
            json[header.code.field] = data.Code;
            return json;
        }
    }
}