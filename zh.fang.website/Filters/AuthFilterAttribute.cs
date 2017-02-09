namespace zh.fang.website.Filters
{
    using System.Web;
    using System.Web.Mvc;

    public class AuthFilterAttribute: AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var usr = httpContext.Session[GloableConfig.UserKey] as data.entity.User;
            return usr != null;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.HttpContext.Session.Clear();
            filterContext.HttpContext.Session.Abandon();
            //filterContext.HttpContext.Response.Redirect("/admin/login");
            filterContext.Result = new RedirectResult("/admin/toplogin");
        }
    }
}