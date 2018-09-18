using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Service;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Core.Data;
using Shared.Common;
using Web.Infrastructure;
using DataTablesDotNet.Models;
using DataTablesDotNet;

namespace Web.Areas.Admin.Controllers
{
    [MvcAuthorize]
    [ClaimsGroup(ClaimResources.Menus)]
    public class MenusController : BaseController
    {
        private readonly IMenuServices _menuServices;

        public MenusController(IMenuServices menuServices)
        {
            _menuServices = menuServices;
        }

        [ClaimsAction(ClaimsActions.Index)]
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult Data(DataTablesRequest model)
        {
            var data = _menuServices.GetAll().Data.AsQueryable();
            var dataTableParser = new DataTablesParser<Menu>(model, data);
            var formattedList = dataTableParser.Process();
            return Json(formattedList, JsonRequestBehavior.AllowGet);
        }

        //#region Create
        //[HttpGet]
        ////[ClaimsAction(ClaimsActions.Create)]
        //public PartialViewResult Add()
        //{
        //    MenuViewModel model = new MenuViewModel();
        //    model.Parent = _menuServices.GetAll().Select(r => new SelectListItem
        //    {
        //        Text = r.Name,
        //        Value = r.Id.ToString()
        //    }).ToList();
        //    return PartialView("_AddMenu", model);
        //}
        //[HttpPost]
        //[ClaimsAction(ClaimsActions.Create)]
        //public JsonResult Add(MenuViewModel model)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            var menu = new Menu();
        //            menu.Name = model.Name;
        //            menu.ParentId = model.ParentId;
        //            menu.Icon = model.Icon;
        //            menu.Url = model.Url;
        //            menu.IsActive = model.IsActive;
        //            menu.Order = model.Order;
        //            _menuServices.Add(menu);
        //            _menuServices.Save();
        //            return Json(new { status = true, message = Command.MessageSuccess });
        //        }
        //        return Json(new { status = false, message = Command.MessageError });
        //    }
        //    catch
        //    {
        //        return Json(new { status = false, message = Command.MessageError });
        //    }
        //}
        //#endregion

        //#region Edit
        //[HttpGet]
        //[ClaimsAction(ClaimsActions.Edit)]
        //public PartialViewResult Edit(int? id)
        //{
        //    MenuViewModel model = new MenuViewModel();
        //    model.Parent = _menuServices.GetAll().Select(r => new SelectListItem
        //    {
        //        Text = r.Name,
        //        Value = r.Id.ToString()
        //    }).ToList();

        //    if (id.HasValue)
        //    {
        //        var menu = _menuServices.Get(id.Value);
        //        if (menu != null)
        //        {
        //            model.Id = menu.Id;
        //            model.Name = menu.Name;
        //            model.Url = menu.Url;
        //            model.Icon = menu.Icon;
        //            model.Order = menu.Order;
        //            model.IsActive = menu.IsActive;
        //            model.ParentId = menu.ParentId;
        //        }
        //    }
        //    return PartialView("_EditMenu", model);
        //}
        //[HttpPost]
        //[ClaimsAction(ClaimsActions.Edit)]
        //public JsonResult Edit(int? id, MenuViewModel model)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            var menu = _menuServices.Get(id.Value);
        //            menu.Name = model.Name;
        //            menu.ParentId = model.ParentId;
        //            menu.Icon = model.Icon;
        //            menu.Url = model.Url;
        //            menu.IsActive = model.IsActive;
        //            menu.Order = model.Order;
        //            _menuServices.Update(menu);
        //            _menuServices.Save();
        //            return Json(new { status = true, message = Command.MessageSuccess });
        //        }
        //        return Json(new { status = false, message = Command.MessageError });
        //    }
        //    catch
        //    {
        //        return Json(new { status = false, message = Command.MessageError });
        //    }
        //}
        //#endregion

        //#region Delete
        //[HttpGet]
        //[ClaimsAction(ClaimsActions.Delete)]
        //public PartialViewResult Delete(int id)
        //{
        //    var model = new Menu();
        //    if (id > 0)
        //    {
        //        model = _menuServices.Get(id);
        //    }
        //    return PartialView("_DeleteMenu", model);
        //}

        //[HttpPost]
        //[ClaimsAction(ClaimsActions.Delete)]
        //public JsonResult Delete(int id, FormCollection form)
        //{
        //    try
        //    {
        //        if (id > 0)
        //        {
        //            _menuServices.Delete(id);
        //            _menuServices.Save();
        //            return Json(new { status = true, message = Command.MessageSuccess });
        //        }
        //        return Json(new { status = false, message = Command.MessageError });
        //    }
        //    catch
        //    {
        //        return Json(new { status = false, message = Command.MessageError });
        //    }
        //}

        //#endregion

        //#region Function
        ////public JsonResult GetDataTable()
        ////{
        ////    var request = new DataTablesRequest<MenuViewModel>(Request.Params);
        ////    request.Columns[p => p.Name]
        ////        .GlobalSearchPredicate = (p) => p.Name.ToUpper().Contains(request.GlobalSearchValue.ToUpper());
        ////    request.Columns[p => p.ParentName]
        ////        .GlobalSearchPredicate = (p) => p.ParentName.ToUpper().Contains(request.GlobalSearchValue.ToUpper());
        ////    request.Columns[p => p.Url]
        ////        .GlobalSearchPredicate = (p) => p.Url.ToUpper().Contains(request.GlobalSearchValue.ToUpper());
        ////    var model = _menuServices.GetAll().Select(c => new MenuViewModel
        ////    {
        ////        Id = c.Id,
        ////        Name = c.Name,
        ////        Url = c.Url,
        ////        Order = c.Order,
        ////        IsActive = c.IsActive,
        ////        ParentId = c.ParentId ?? 0,
        ////        ParentName = _menuServices.Get(c.ParentId ?? 0)?.Name ?? string.Empty
        ////    });
        ////    var data = model.AsQueryable().ToPagedList(request);
        ////    return JsonDataTable(data);
        ////}
        //#endregion
    }
}