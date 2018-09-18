using Data.Models;
using Microsoft.AspNet.Identity;
using Service;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class TemplateController : Controller
    {
        private IMenuServices _menuAppService;
        private IMenuInRolesServices _menuInRolesAppService;
        private IAccountServices _accountServices;
        private readonly UserManager userManager;
        private readonly RoleManager roleManager;
        public TemplateController(UserManager userManager, RoleManager roleManager, IMenuServices menuAppService, IMenuInRolesServices menuInRolesAppService,
            IAccountServices accountServices)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _menuInRolesAppService = menuInRolesAppService;
            _accountServices = accountServices;
            _menuAppService = menuAppService;
        }

        public PartialViewResult Header()
        {

            var lsCurrentRole = new List<string>();
            //Da login => lay list role
            if (User.Identity.IsAuthenticated)
            {
                var currentRole = CurrentRole();
                foreach (var name in currentRole)
                {
                    lsCurrentRole.Add(roleManager.FindByNameAsync(name).Result.Id);
                }
                string userId = User.Identity.GetUserId();
                var Name = userManager.Users.FirstOrDefault(c => c.Id == userId).Name;
                ViewBag.Name = Name;
            }
            //chua login/ thi lay role la anomymous
            else
            {
                lsCurrentRole.Add(roleManager.FindByNameAsync("Anonymous").Result.Id);
            }
            //get current menu tu role
            var lsCurrentMenu = _menuInRolesAppService.GetAll().Data.Where(c => lsCurrentRole.Contains(c.RoleId.ToString())).Select(c => c.MenuId);

            var model = _menuAppService.GetParent().Data
                .Where(c => lsCurrentMenu.Contains(c.Id))
                .OrderBy(c => c.Order)
                .Select(l => new MenuViewModel
                {
                    Id = l.Id,
                    Name = l.Name,
                    Url = l.Url,
                    Icon = l.Icon,
                    Order = l.Order,
                    IsActive = l.IsActive,
                    ParentId = l.ParentId,
                    Childrens = GetChildren(l.Id)
                });
            return PartialView(model);
        }
        public PartialViewResult Footer()
        {
            return PartialView();
        }

        private List<MenuViewModel> GetChildren(int parentId)
        {
            var lsCurrentRole = new List<string>();
            //Da login => lay list role
            if (User.Identity.IsAuthenticated)
            {
                var currentRole = CurrentRole();
                foreach (var name in currentRole)
                {
                    lsCurrentRole.Add(roleManager.FindByNameAsync(name).Result.Id);
                }
            }
            //chua login/ thi lay role la anomymous
            else
            {
                lsCurrentRole.Add(roleManager.FindByNameAsync("Anonymous").Result.Id);
            }
            //get current menu tu role
            var lsCurrentMenu = _menuInRolesAppService.GetAll().Data.Where(c => lsCurrentRole.Contains(c.RoleId.ToString())).Select(c => c.MenuId);

            return _menuAppService.GetChildren(parentId).Data
                 .Where(c => lsCurrentMenu.Contains(c.Id))
                 .OrderBy(l => l.Order)
                .Select(l => new MenuViewModel
                {
                    Id = l.Id,
                    Name = l.Name,
                    Url = l.Url,
                    Icon = l.Icon,
                    Order = l.Order,
                    IsActive = l.IsActive,
                    ParentId = l.ParentId,
                    Childrens = GetChildren(l.Id)
                }).ToList();
        }
        private List<string> CurrentRole()
        {
            var UserId = HttpContext.User.Identity.GetUserId();
            var currentRole = userManager.GetRoles(UserId);
            return currentRole.ToList();
        }


    }
}