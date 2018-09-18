using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using Data.Models;
using DataTablesDotNet;
using DataTablesDotNet.Models;
using Shared.Common;
using Shared.Models;
using Web.Infrastructure;

namespace Web.Areas.Admin.Controllers
{
    //[ClaimsGroup(ClaimResources.Users)]
    [MvcAuthorize]
    public class UsersController : BaseController
    {
        private readonly UserManager _userManager;
        private readonly ClaimedActionsProvider _claimedActionsProvider;
        private readonly RoleManager _roleManager;


        public UsersController(UserManager userManager, ClaimedActionsProvider claimedActionsProvider, RoleManager roleManager)
        {
            _userManager = userManager;
            _claimedActionsProvider = claimedActionsProvider;
            _roleManager = roleManager;
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Data(DataTablesRequest model)
        {
            var data = _userManager.Users.AsQueryable();
            var dataTableParser = new DataTablesParser<ApplicationUser>(model, data);
            var formattedList = dataTableParser.Process();
            return Json(formattedList, JsonRequestBehavior.AllowGet);
        }
    }
}