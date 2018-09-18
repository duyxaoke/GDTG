using System.Web.Mvc;

namespace Web.Controllers
{
    public class ErrorsController : Controller
    {
        public ActionResult Unauthorised()
        {
            return View();
        }
    }
}