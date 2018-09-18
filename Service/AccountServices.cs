using Core.DTO.Response;
using Data.DAL;
using Data.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Service
{
    public interface IAccountServices
    {
        CRUDResult<IEnumerable<ApplicationRole>>  GetAllRoles();
        CRUDResult<IEnumerable<ApplicationUser>>  GetAllUsers();
        CRUDResult<IEnumerable<IdentityUserRole>>  GetAllUserRole();
        CRUDResult<ApplicationUser> GetUser(string id);
        CRUDResult<ApplicationUser> GetUserbyUsername(string username);
        CRUDResult<bool> AddOrUpdateUser(ApplicationUser user, IList<string> lstRole);
    }
    public class AccountServices : IAccountServices
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly DatabaseContext _context;
        public AccountServices(UnitOfWork unitOfWork, DatabaseContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public CRUDResult<IEnumerable<ApplicationRole>> GetAllRoles()
        {
            var result = _unitOfWork.IdentityRoleRepository.GetAll();
            return new CRUDResult<IEnumerable<ApplicationRole>> { StatusCode = CRUDStatusCodeRes.Success, Data = result };
        }
        public CRUDResult<IEnumerable<ApplicationUser>>  GetAllUsers()
        {
            var result = _unitOfWork.ApplicationUserRepository.GetAll();
            return new CRUDResult<IEnumerable<ApplicationUser>> { StatusCode = CRUDStatusCodeRes.Success, Data = result };

        }
        public CRUDResult<IEnumerable<IdentityUserRole>>  GetAllUserRole()
        {
            var result = _unitOfWork.IdentityUserRoleRepository.GetAll();
            return new CRUDResult<IEnumerable<IdentityUserRole>> { StatusCode = CRUDStatusCodeRes.Success, Data = result };

        }
        public CRUDResult<ApplicationUser>  GetUser(string id)
        {
            var result = _unitOfWork.ApplicationUserRepository.GetById(id);
            return new CRUDResult<ApplicationUser> { StatusCode = CRUDStatusCodeRes.Success, Data = result };

        }
        public CRUDResult<ApplicationUser>  GetUserbyUsername(string username)
        {
            var result = _unitOfWork.ApplicationUserRepository.Get(c => c.UserName == username);
            return new CRUDResult<ApplicationUser> { StatusCode = CRUDStatusCodeRes.Success, Data = result };

        }
        /// <summary>
        /// Lưu hoặc Sửa thông tin User (demo async)
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public CRUDResult<bool>  AddOrUpdateUser(ApplicationUser user, IList<string> lstRole)
        {
            bool result = false;
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
            var store = new UserStore<ApplicationUser>(_context);
            var manager = new UserManager<ApplicationUser>(store);
            var existUser = userManager.Users.FirstOrDefault(aa => aa.Id == user.Id);
            if (existUser != null)
            {
                try
                {
                    //_unitOfWork.ApplicationUserRepository.Update(user);
                    existUser.Email = user.Email;
                    existUser.PhoneNumber = user.PhoneNumber;

                    manager.Update(existUser);
                    var ctx = store.Context;
                    ctx.SaveChanges();
                    if (lstRole != null && lstRole.Any())
                    {
                        var allRole = userManager.GetRoles(existUser.Id).ToArray();
                        if (allRole != null && allRole.Any())
                        {
                            using (TransactionScope scope = new TransactionScope())
                            {
                                userManager.RemoveFromRoles(existUser.Id, allRole);
                                if (lstRole.Count > 0)
                                {
                                    var roles = _unitOfWork.IdentityRoleRepository.GetMany(aa => lstRole.Contains(aa.Id));
                                    var rolesAdd = roles.Select(aa => aa.Name).ToArray();
                                    userManager.AddToRoles(existUser.Id, rolesAdd);
                                }
                                scope.Complete();
                                result = true;
                            }

                        }                        
                    }                    
                } 
                catch (Exception)
                {
                    return new CRUDResult<bool> { StatusCode = CRUDStatusCodeRes.ResetContent, Data = result };
                }
            }
            if (result)
                return new CRUDResult<bool> { StatusCode = CRUDStatusCodeRes.Success, Data = result };
            return new CRUDResult<bool> { StatusCode = CRUDStatusCodeRes.ResetContent, Data = result };
        }
    }
}
