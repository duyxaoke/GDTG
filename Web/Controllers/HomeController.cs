using AutoMapper;

using Data.Models;
using System.Web.Mvc;
using Web.Helpers;
using Web.Infrastructure;

namespace Web.Controllers
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
        public ActionResult Index(int? room = 1)
        {
            ViewBag.Room = room;
            return View();
        }
        public ActionResult Test()
        {
            return View();
        }
        public ActionResult Sender()
        {
            return View();
        }

        public ActionResult Receiver()
        {
            return View();
        }


    }
}