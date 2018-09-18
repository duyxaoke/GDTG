using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using Data.Models;
using DataTablesDotNet;
using DataTablesDotNet.Models;
using Service;
using Shared.Common;
using Shared.Models;
using Web.Infrastructure;

namespace Web.Areas.Admin.Controllers
{
    [MvcAuthorize]
    public class RolesController : BaseController
    {
        private readonly RoleManager _roleManager;
        private readonly ClaimedActionsProvider _claimedActionsProvider;
        public RolesController(RoleManager roleManager, ClaimedActionsProvider claimedActionsProvider)
        {
            _roleManager = roleManager;
            _claimedActionsProvider = claimedActionsProvider;
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Data(DataTablesRequest model)
        {
            var data = _roleManager.Roles.AsQueryable();
            var dataTableParser = new DataTablesParser<ApplicationRole>(model, data);
            var formattedList = dataTableParser.Process();
            return Json(formattedList, JsonRequestBehavior.AllowGet);
        }

    }
}