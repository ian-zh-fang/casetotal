namespace zh.fang.website.Models
{
    public class OrgClsTotalSubmitModel
    {
        public string id { get; set; }

        public ClsTotalSubmitModel[] items { get; set; }
    }

    public class ClsTotalSubmitModel
    {
        public string id { get; set; }

        public int value { get; set; }
    }
}