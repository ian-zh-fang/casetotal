namespace zh.fang.website.Models
{
    using Newtonsoft.Json.Linq;
    using data.entity;
    using System;

    public class OrgTableHeaderModel
    {
        public TableCellDataModel id => new TableCellDataModel { field="id", title="ID" };

        public TableCellDataModel parentId => new TableCellDataModel { field= "_parentId", title="PID" };

        public TableCellDataModel name => new TableCellDataModel { field="name", title="组织机构名称" };

        public TableCellDataModel code => new TableCellDataModel { field = "code", title = "组织机构编码" };

        public TableCellDataModel safety => new TableCellDataModel { field = "safety", title = "安全阈值（黄色）" };

        public TableCellDataModel warning => new TableCellDataModel { field = "warning", title = "警告阈值（橙色）" };

        public TableCellDataModel danger => new TableCellDataModel { field = "danger", title = "危险阈值（红色）" };
    }

    public interface IOrgModelToTableCellModel<T, TResult>
    {
        TResult GetData(T data, OrgTableHeaderModel header);
    }

    public class OrgModelToTableCellModel : IOrgModelToTableCellModel<Orgnization, JObject>
    {
        public JObject GetData(Orgnization data, OrgTableHeaderModel header)
        {
            var json = new JObject();
            json[header.id.field] = data.Id;
            json[header.name.field] = data.Name;
            json[header.parentId.field] = data.ParentId;
            json[header.code.field] = data.Code;
            json[header.safety.field] = data.Glv;
            json[header.warning.field] = data.Ylv;
            json[header.danger.field] = data.Olv;
            return json;
        }
    }
}