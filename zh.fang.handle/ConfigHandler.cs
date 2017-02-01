namespace zh.fang.handle
{
    using System.Linq;

    public class ConfigHandler:Handle
    {
        public data.entity.Config FetchOne(short type)
        {
            return Repository.Query<data.entity.Config>(t => t.IsDel == 0 && t.Type == type).FirstOrDefault();
        }
    }
}
