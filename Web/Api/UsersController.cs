using Data.Models;
using Microsoft.AspNet.Identity;
using Service;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using Web.Helpers;
using Web.Infrastructure;
using WebApiThrottle;

namespace Web.Api
{
    [ApiAuthorizeAttribute]
    [RoutePrefix("api/Users")]
    public class UsersController : ApiControllerBase
    {
        private readonly RoleManager _roleManager;
        private readonly UserManager _userManager;
        private readonly ClaimedActionsProvider _claimedActionsProvider;


        public UsersController(RoleManager roleManager, UserManager userManager, ClaimedActionsProvider claimedActionsProvider)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _claimedActionsProvider = claimedActionsProvider;
        }
        [HttpGet]
        [Route("List")]
        public IHttpActionResult List()
        {
            var result = _userManager.Users.ToList();
            return CCOk(result);
        }


        [HttpPost]
        [Route("GetBalance")]
        [EnableThrottling(PerSecond = 1)]
        public IHttpActionResult GetBalance()
        {
            var result = _userManager.FindByIdAsync(User.Identity.GetUserId()).Result.Balance;
            return CCOk(result);
        }

        [HttpGet]
        [Route("ViewClaims/{id}")]
        public async System.Threading.Tasks.Task<IHttpActionResult> GetAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            var userRoles = await _userManager.GetRolesAsync(id);
            var userclaims = new List<Claim>();
            foreach (var role in userRoles)
            {
                var roleClaims = await _roleManager.GetClaimsAsync(role);

                userclaims.AddRange(roleClaims);
            }

            var claimGroups = _claimedActionsProvider.GetClaimGroups();

            var result = new UserClaimsViewModel()
            {
                UserName = user.UserName,
            };

            foreach (var claimGroup in claimGroups)
            {
                var claimGroupModel = new UserClaimsViewModel.ClaimGroup()
                {
                    GroupId = claimGroup.GroupId,
                    GroupName = claimGroup.GroupName,
                    GroupClaimsCheckboxes = claimGroup.Claims
                        .Select(c => new SelectListViewModel()
                        {
                            Value = String.Format("{0}#{1}", claimGroup.GroupId, c),
                            Text = c,
                            Selected = userclaims.Any(ac => ac.Type == claimGroup.GroupId.ToString() && ac.Value == c)
                        }).ToList()
                };
                result.ClaimGroups.Add(claimGroupModel);
            }
            return CCOk(result);
        }
        [HttpPost]
        [Route("Create")]
        [EnableThrottling(PerSecond = 1)]
        public async System.Threading.Tasks.Task<IHttpActionResult> PostAsync([FromBody]UserViewModel model)
        {
            ApplicationUser user = new ApplicationUser
            {
                Name = model.Name,
                UserName = model.UserName,
                Email = model.Email
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return CCOk(result.Succeeded);
            }
            return CCNotAcceptable(result.Errors);

        }
        [HttpGet]
        [Route("{id}")]
        [EnableThrottling(PerSecond = 1)]
        public async System.Threading.Tasks.Task<IHttpActionResult> GetByIdAsync(string id)
        {
            ApplicationUser result = await _userManager.FindByIdAsync(id);
            return CCOk(result);

        }
        [HttpPut]
        [Route("Update")]
        [EnableThrottling(PerSecond = 1)]
        public async System.Threading.Tasks.Task<IHttpActionResult> PutAsync([FromBody]UserViewModel model)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(model.Id);
            user.Name = model.Name;
            user.Email = model.Email;
            user.UserName = model.UserName;
            if (!String.IsNullOrEmpty(model.Password))
            {
                user.PasswordHash = _userManager.PasswordHasher.HashPassword(model.Password);
            }
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return CCOk(result.Succeeded);
            }
            return CCNotAcceptable(result.Errors);
        }
        [HttpGet]
        [Route("EditRoles/{id}")]
        public async System.Threading.Tasks.Task<IHttpActionResult> EditRoles(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var assignedRoles = await _userManager.GetRolesAsync(user.Id);

            var allRoles = await _roleManager.Roles.ToListAsync();

            var userRoles = allRoles.Select(r => new SelectListViewModel()
            {
                Value = r.Name,
                Text = r.Name,
                Selected = assignedRoles.Contains(r.Name),
            }).ToList();

            var result = new UserRolesViewModel
            {
                Username = user.UserName,
                UserId = user.Id,
                UserRoles = userRoles,
            };
            return CCOk(result);
        }
        [HttpPost]
        [Route("EditRoles")]
        [EnableThrottling(PerSecond = 1)]
        public async System.Threading.Tasks.Task<IHttpActionResult> EditRoles([FromBody]UserRolesViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            var possibleRoles = await _roleManager.Roles.ToListAsync();
            var userRoles = await _userManager.GetRolesAsync(user.Id);

            var submittedRoles = model.SelectedRoles;

            var shouldUpdateSecurityStamp = false;

            foreach (var submittedRole in submittedRoles)
            {
                var hasRole = await _userManager.IsInRoleAsync(user.Id, submittedRole);
                if (!hasRole)
                {
                    shouldUpdateSecurityStamp = true;
                    await _userManager.AddToRoleAsync(user.Id, submittedRole);
                }
            }

            foreach (var removedRole in possibleRoles.Select(r => r.Name).Except(submittedRoles))
            {
                shouldUpdateSecurityStamp = true;
                await _userManager.RemoveFromRoleAsync(user.Id, removedRole);
            }

            if (shouldUpdateSecurityStamp)
            {
                await _userManager.UpdateSecurityStampAsync(user.Id);
            }
            return CCOk(true);
        }
        [HttpDelete]
        [EnableThrottling(PerSecond = 1)]
        public async System.Threading.Tasks.Task<IHttpActionResult> DeleteAsync(string id)
        {
            ApplicationUser applicationUser = await _userManager.FindByIdAsync(id);
            if (applicationUser != null)
            {
                var result = await _userManager.DeleteAsync(applicationUser);
                return CCOk(result.Succeeded);
            }
            return CCOk(false);
        }
        [HttpPost]
        [Route("LockUser/{id}")]
        public async System.Threading.Tasks.Task<IHttpActionResult> LockUser(string id)
        {
            var result = await _userManager.SetLockoutEnabledAsync(id, true);
            if (result.Succeeded)
            {
                result = await _userManager.SetLockoutEndDateAsync(id, DateTimeOffset.MaxValue);
                return CCOk(result.Succeeded);
            }
            return CCNotAcceptable(result.Errors);
        }
        [HttpPost]
        [Route("UnlockUser/{id}")]
        public async System.Threading.Tasks.Task<IHttpActionResult> UnlockUser(string id)
        {
            var result = await _userManager.SetLockoutEnabledAsync(id, false);
            if (result.Succeeded)
            {
                await _userManager.ResetAccessFailedCountAsync(id);
                return CCOk(result.Succeeded);
            }
            return CCNotAcceptable(result.Errors);
        }
    }
    public class UsersIndexViewIndex
    {
        public List<ApplicationUser> Users { get; set; }
    }


    public class UserClaimsViewModel
    {
        public UserClaimsViewModel()
        {
            ClaimGroups = new List<ClaimGroup>();

            SelectedClaims = new List<String>();
        }

        public String UserName { get; set; }

        public List<ClaimGroup> ClaimGroups { get; set; }

        public IEnumerable<String> SelectedClaims { get; set; }


        public class ClaimGroup
        {
            public ClaimGroup()
            {
                GroupClaimsCheckboxes = new List<SelectListViewModel>();
            }
            public String GroupName { get; set; }

            public int GroupId { get; set; }

            public List<SelectListViewModel> GroupClaimsCheckboxes { get; set; }
        }
    }



    public class UserRolesViewModel
    {
        public UserRolesViewModel()
        {
            UserRoles = new List<SelectListViewModel>();
            SelectedRoles = new List<String>();
        }

        public String UserId { get; set; }
        public String Username { get; set; }
        public List<SelectListViewModel> UserRoles { get; set; }
        public List<String> SelectedRoles { get; set; }
    }

}
