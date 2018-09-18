using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;


namespace Web.Infrastructure
{
    public class WeborisationFilter : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var user = HttpContext.Current.User as ClaimsPrincipal;
            var controllerGroup = filterContext.Controller.GetType().GetCustomAttribute<ClaimsGroupAttribute>();

            if (controllerGroup == null)
            {
                return;
            }

            var actionClaim = (ClaimsActionAttribute)filterContext.ActionDescriptor.GetCustomAttributes(typeof(ClaimsActionAttribute), true).FirstOrDefault();

            actionClaim = actionClaim ?? new ClaimsActionAttribute(ClaimsActions.Index);

            var groupId = controllerGroup.GetGroupId();
            var claimValue = actionClaim.Claim.ToString();

            var hasClaim = user.HasClaim(groupId, claimValue);
            if (!hasClaim)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.HttpContext.Response.StatusCode = 403;
                    filterContext.Result = new JsonResult()
                    {
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        Data = new
                        {
                            errorMessage = "Unauthorised"
                        }
                    };
                }
                else
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary()
                    {
                        { "controller", "Errors" },
                        { "action", "Unauthorised" },
                        { "area", null}
                    });
                }

            }
        }
    }
}