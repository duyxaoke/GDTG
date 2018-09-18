using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Web.Infrastructure
{
    public class ApiAuthorizeAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            if (!actionContext.RequestContext.Principal.Identity.IsAuthenticated)
                base.HandleUnauthorizedRequest(actionContext);
            else
            {
                // Authenticated, but not AUTHORIZED.  Return 403 instead!
                actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden);
            }
        }

    }
}