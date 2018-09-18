using Data.Models;
using Service;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using Web.Helpers;
using Web.Infrastructure;
using WebApiThrottle;

namespace Web.Api
{
    [ApiAuthorizeAttribute]
    [RoutePrefix("api/Roles")]
    public class RolesController : ApiControllerBase
    {
        private readonly IRoleServices _roleServices;
        private readonly RoleManager _roleManager;
        private readonly ClaimedActionsProvider _claimedActionsProvider;
        private IMenuServices _menuService;
        private IMenuInRolesServices _menuInRolesService;


        public RolesController(IRoleServices roleServices, RoleManager roleManager, ClaimedActionsProvider claimedActionsProvider,
            IMenuServices menuService, IMenuInRolesServices menuInRolesService)
        {
            _roleServices = roleServices;
            _roleManager = roleManager;
            _claimedActionsProvider = claimedActionsProvider;
            _menuService = menuService;
            _menuInRolesService = menuInRolesService;
        }
        [HttpGet]
        [Route("List")]
        public IHttpActionResult List()
        {
            var result = _roleServices.GetAll();
            return ApiHelper.ReturnHttpAction(result, this);
        }

        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult Get(string id)
        {
            var result = _roleServices.GetById(id);
            return ApiHelper.ReturnHttpAction(result, this);
        }
        [HttpGet]
        [Route("MenuInRoles/{roleId:guid}")]
        public IHttpActionResult MenuInRoles(Guid? roleId)
        {
            var result = new List<MenuViewModel>();
            var menus = _menuService.GetParent().Data.OrderBy(l => l.Order);
            foreach (var item in menus)
            {
                MenuViewModel model = new MenuViewModel();
                model.Id = item.Id;
                model.Name = item.Name;
                model.Url = item.Url;
                model.Icon = item.Icon;
                model.Order = item.Order;
                model.IsActive = item.IsActive;
                model.ParentId = item.ParentId;
                model.RoleId = roleId;
                model.Childrens = GetChildrens(item.Id, roleId);
                if (roleId.HasValue)
                {
                    var menu = _menuInRolesService.GetMenuByRoleId(roleId.Value).Data.Any(c => c.MenuId == item.Id);
                    if (menu)
                        model.Checked = true;
                    else
                        model.Checked = false;
                }
                result.Add(model);
            }
            return CCOk(result);
        }

        [HttpPost]
        [Route("MenuInRoles")]
        [EnableThrottling(PerSecond = 1)]
        public IHttpActionResult MenuInRoles(UpdateMenuInRoleViewModel model)
        {
            var result = _menuInRolesService.AddOrUpdateMenuInRoles(model.RoleId, model.MenuIds);
            _menuInRolesService.Save();
            return ApiHelper.ReturnHttpAction(result, this);
        }


        [HttpPost]
        [Route("Create")]
        [EnableThrottling(PerSecond = 1)]
        public async System.Threading.Tasks.Task<IHttpActionResult> PostAsync([FromBody]CreateRoleViewModel model)
        {
            var newRole = new ApplicationRole()
            {
                Name = model.Name,
            };
            var result = await _roleManager.CreateAsync(newRole);
            if (result.Succeeded)
            {
                return CCOk(result.Succeeded);
            }
            return CCNotAcceptable(result.Errors);
        }
        [HttpGet]
        [Route("EditClaims/{id}")]
        public async System.Threading.Tasks.Task<IHttpActionResult> EditClaimsAsync(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            var claimGroups = _claimedActionsProvider.GetClaimGroups();

            var assignedClaims = await _roleManager.GetClaimsAsync(role.Name);

            var result = new RoleClaimsViewModel()
            {
                RoleId = role.Id,
                RoleName = role.Name,
            };

            foreach (var claimGroup in claimGroups)
            {
                var claimGroupModel = new RoleClaimsViewModel.ClaimGroup()
                {
                    GroupId = claimGroup.GroupId,
                    GroupName = claimGroup.GroupName,
                    GroupClaimsCheckboxes = claimGroup.Claims
                        .Select(c => new SelectListViewModel()
                        {
                            Value = String.Format("{0}#{1}", claimGroup.GroupId, c),
                            Text = c,
                            Selected = assignedClaims.Any(ac => ac.Type == claimGroup.GroupId.ToString() && ac.Value == c)
                        }).ToList()
                };
                result.ClaimGroups.Add(claimGroupModel);
            }
            return CCOk(result);
        }
        [HttpPost]
        [Route("EditClaims")]
        public async System.Threading.Tasks.Task<IHttpActionResult> EditClaimsAsync([FromBody]RoleClaimsViewModel viewModel)
        {
            var role = await _roleManager.FindByIdAsync(viewModel.RoleId);
            role.Name = viewModel.RoleName;
            var roleResult = await _roleManager.UpdateAsync(role);
            var roleClaims = await _roleManager.GetClaimsAsync(role.Name);
            // this is ugly. Deletes all the claims and adds them back in.
            // can be done in a better fashion
            foreach (var removedClaim in roleClaims)
            {
                await _roleManager.RemoveClaimAsync(role.Id, removedClaim);
            }

            var submittedClaims = viewModel
                .SelectedClaims
                .Select(s =>
                {
                    var tokens = s.Split('#');
                    if (tokens.Count() != 2)
                    {
                        throw new Exception(String.Format("Claim {0} can't be processed because it is in incorrect format", s));
                    }
                    return new Claim(tokens[0], tokens[1]);
                }).ToList();


            roleClaims = await _roleManager.GetClaimsAsync(role.Name);

            foreach (var submittedClaim in submittedClaims)
            {
                var hasClaim = roleClaims.Any(c => c.Value == submittedClaim.Value && c.Type == submittedClaim.Type);
                if (!hasClaim)
                {
                    await _roleManager.AddClaimAsync(role.Id, submittedClaim);
                }
            }

            roleClaims = await _roleManager.GetClaimsAsync(role.Name);

            var cacheKey = ApplicationRole.GetCacheKey(role.Name);
            System.Web.HttpContext.Current.Cache.Remove(cacheKey);
            return CCOk(roleClaims);
        }

        [HttpDelete]
        [EnableThrottling(PerSecond = 1)]
        public async System.Threading.Tasks.Task<IHttpActionResult> DeleteAsync(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            var roleClaims = await _roleManager.GetClaimsAsync(role.Name);
            // this is ugly. Deletes all the claims and adds them back in.
            // can be done in a better fashion
            foreach (var removedClaim in roleClaims)
            {
                await _roleManager.RemoveClaimAsync(role.Id, removedClaim);
            }
            var result = _roleManager.DeleteAsync(role).Result;
            if (result.Succeeded)
            {
                return CCOk(result.Succeeded);
            }
            return CCNotAcceptable(result.Errors);
        }

        #region Helper
        private List<MenuViewModel> GetChildrens(int parentId, Guid? roleId)
        {
            var lsmodel = new List<MenuViewModel>();
            var menus = _menuService.GetChildren(parentId).Data.OrderBy(l => l.Order);
            foreach (var item in menus)
            {
                MenuViewModel model = new MenuViewModel();
                model.Id = item.Id;
                model.Name = item.Name;
                model.Url = item.Url;
                model.Icon = item.Icon;
                model.Order = item.Order;
                model.IsActive = item.IsActive;
                model.ParentId = item.ParentId;
                if (roleId.HasValue)
                {
                    var menu = _menuInRolesService.GetMenuByRoleId(roleId.Value).Data.Any(c => c.MenuId == item.Id);
                    if (menu)
                        model.Checked = true;
                    else
                        model.Checked = false;
                }
                lsmodel.Add(model);
            }
            return lsmodel;
        }

        #endregion
    }
}
