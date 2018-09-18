using AutoMapper;

using Data.Models;
using System.Web.Mvc;
using Web.Helpers;
using Web.Infrastructure;

namespace Web.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        #region Fields

        private readonly UserManager _userManager;
        #endregion

        #region Constructor

        public HomeController(
            UserManager userManager)
        {
            _userManager = userManager;
        }

        #endregion

        // GET: /Home/
        public ActionResult Index()
        {
            return View();
        }
        [MvcAuthorizeAttribute(Roles = "User")]
        public ActionResult Test()
        {
            return View();
        }


    }
}