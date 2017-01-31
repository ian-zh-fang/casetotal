namespace zh.fang.module
{
    public class UserModule
    {
        public data.entity.User Signin(string userid, string passwd)
        {
            using (var handler = new handle.UserHandler())
            {
                return handler.FetchOne(userid, passwd);
            }
        }

        public bool UpgPasswd(string userId, string oldPasswd, string newPasswd)
        {
            using (var handler = new handle.UserHandler())
            {
                var usr = handler.FetchOne(userId, oldPasswd);
                if (usr == null)
                {
                    return false;
                }

                usr.Passwd = newPasswd;
                return handler.Update(usr);
            }
        }
    }
}
