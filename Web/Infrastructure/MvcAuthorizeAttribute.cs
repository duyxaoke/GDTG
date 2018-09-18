using System.Web.Mvc;

namespace Web.Infrastructure
{
    public class MvcAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            // Redirect to the login page if necessary
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.HttpContext.Response.StatusCode = 401;
                    return;
                }
                base.HandleUnauthorizedRequest(filterContext);
                return;
            }

            // Redirect to your "access denied" view here
            if (filterContext.Result is HttpUnauthorizedResult)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.HttpContext.Response.StatusCode = 403;
                    return;
                }
                filterContext.Result = new RedirectResult("~/Errors/Unauthorised");
            }
        }
    }
}