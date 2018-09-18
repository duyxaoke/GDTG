using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Microsoft.AspNet.Identity;
using Data.DAL;
using Core.DTO.Response;
using Data.Models;

namespace Service
{
    public interface IRoleServices
    {
        CRUDResult<IEnumerable<ApplicationRole>> GetAll();
        CRUDResult<ApplicationRole> GetById(string id);
        CRUDResult<ApplicationRole> GetByName(string name);
        CRUDResult<IEnumerable<IdentityUserRole>> GetByUser(string UserId);
        CRUDResult<bool> AddOrUpdateRole(ApplicationRole role);
    }
    public class RoleServices : IRoleServices
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly DatabaseContext _context;
        public RoleServices(UnitOfWork unitOfWork, DatabaseContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }
        public CRUDResult<IEnumerable<ApplicationRole>> GetAll()
        {
            var result = _unitOfWork.IdentityRoleRepository.GetAll();
            return new CRUDResult<IEnumerable<ApplicationRole>> { StatusCode = CRUDStatusCodeRes.Success, Data = result };

        }
        public CRUDResult<ApplicationRole> GetByName(string name)
        {
            var result = _unitOfWork.IdentityRoleRepository.Get(c => c.Name == name);
            return new CRUDResult<ApplicationRole> { StatusCode = CRUDStatusCodeRes.Success, Data = result };

        }
        public CRUDResult<ApplicationRole> GetById(string id)
        {
            var result = _unitOfWork.IdentityRoleRepository.GetById(id);
            return new CRUDResult<ApplicationRole> { StatusCode = CRUDStatusCodeRes.Success, Data = result };

        }
        public CRUDResult<IEnumerable<IdentityUserRole>> GetByUser(string UserId)
        {
            var result = _unitOfWork.IdentityUserRoleRepository.GetMany(c => c.UserId == UserId);
            return new CRUDResult<IEnumerable<IdentityUserRole>> { StatusCode = CRUDStatusCodeRes.Success, Data = result };

        }
        public CRUDResult<bool> AddOrUpdateRole(ApplicationRole role)
        {
            var result = false;
            var roleManager = new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(_context));
            var existRole = _unitOfWork.IdentityRoleRepository.GetById(role.Id);
            if (existRole != null)
            {
                existRole.Name = role.Name;
                roleManager.Update(existRole);
            }
            else
            {                
                var tmp = roleManager.Create(role);
                result = tmp.Succeeded;
            }
            if(result)
                return new CRUDResult<bool> { StatusCode = CRUDStatusCodeRes.Success, Data = result };
            return new CRUDResult<bool> { StatusCode = CRUDStatusCodeRes.ResetContent, Data = result };
        }        
    }
}
