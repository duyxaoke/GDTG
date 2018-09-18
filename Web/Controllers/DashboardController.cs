using AutoMapper;

using Data.Models;
using System.Web.Mvc;
using Web.Helpers;
using Web.Infrastructure;

namespace Web.Controllers
{
    public class DashboardController : Controller
    {
        #region Fields

        private readonly UserManager _userManager;
        #endregion

        #region Constructor

        public DashboardController(UserManager userManager)
        {
            _userManager = userManager;
        }

        #endregion

        public ActionResult Index()
        {
            return View();
        }
    }
}