using AutoMapper;

using Data.Models;
using System.Web.Mvc;
using Web.Helpers;
using Web.Infrastructure;

namespace Web.Controllers
{
    public class TransactionController : Controller
    {
        #region Fields

        private readonly UserManager _userManager;
        #endregion

        #region Constructor

        public TransactionController(UserManager userManager)
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