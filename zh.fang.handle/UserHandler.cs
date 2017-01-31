namespace zh.fang.handle
{
    using System.Linq;

    public class UserHandler:Handle
    {
        public data.entity.User FetchOne(string userId, string passwd)
        {
            return 
                Repository.Query<data.entity.User>(t => t.Account == userId && t.Passwd == passwd && t.IsDel == 0).FirstOrDefault();
        }
    }
}
