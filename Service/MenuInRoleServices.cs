using System.Collections.Generic;
using Core.Data;

using System.Data.SqlClient;
using System.Data;
using System.Transactions;
using Service.CacheService;
using System.Linq;
using Shared.Models;
using System;
using Core.DTO.Response;

namespace Service
{
    public interface IMenuInRolesServices
    {
        CRUDResult<MenuInRoles> GetById(int id);
        CRUDResult<IEnumerable<MenuInRoles>> GetAll();
        CRUDResult<bool> Add(MenuInRoles model);
        CRUDResult<bool> Update(MenuInRoles model);
        CRUDResult<bool> Delete(int id);
        void Save();
        void Dispose();
        CRUDResult<bool> AddOrUpdateMenuInRoles(Guid roleId, List<int> menuIds);
        CRUDResult<IEnumerable<MenuInRoles>> GetMenuByRoleId(Guid roleId);
    }
    public class MenuInRolesServices : IMenuInRolesServices
    {
        private readonly UnitOfWork _unitOfWork;
        public MenuInRolesServices(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public CRUDResult<IEnumerable<MenuInRoles>> GetMenuByRoleId(Guid roleId)
        {
            var result = _unitOfWork.MenuInRolesRepository.GetMany(c => c.RoleId == roleId);
            return new CRUDResult<IEnumerable<MenuInRoles>> { StatusCode = CRUDStatusCodeRes.Success, Data = result };

        }

        public CRUDResult<MenuInRoles> GetById(int id)
        {
            var result = _unitOfWork.MenuInRolesRepository.GetById(id);
            return new CRUDResult<MenuInRoles> { StatusCode = CRUDStatusCodeRes.Success, Data = result };

        }
        public CRUDResult<IEnumerable<MenuInRoles>> GetAll()
        {
            var result = _unitOfWork.MenuInRolesRepository.GetAll();
            return new CRUDResult<IEnumerable<MenuInRoles>> { StatusCode = CRUDStatusCodeRes.Success, Data = result };
        }
        public CRUDResult<bool> Add(MenuInRoles model)
        {
            var result = _unitOfWork.MenuInRolesRepository.Insert(model);
            return new CRUDResult<bool> { StatusCode = CRUDStatusCodeRes.Success, Data = result };
        }
        public CRUDResult<bool> Update(MenuInRoles model)
        {
            var result = _unitOfWork.MenuInRolesRepository.Update(model);
            return new CRUDResult<bool> { StatusCode = CRUDStatusCodeRes.Success, Data = result };

        }
        public CRUDResult<bool> Delete(int id)
        {
            var result = _unitOfWork.MenuInRolesRepository.Delete(id);
            return new CRUDResult<bool> { StatusCode = CRUDStatusCodeRes.Success, Data = result };
        }
        public CRUDResult<bool> AddOrUpdateMenuInRoles(Guid roleId, List<int> menuIds)
        {
            bool result = false;
            var existRole = _unitOfWork.MenuInRolesRepository.GetMany(c => c.RoleId == roleId);
            if (existRole != null)
            {
                try
                {
                    //xóa hết dữ liệu trong MenuInRoles theo RoleId
                    foreach (var item in existRole)
                    {
                        _unitOfWork.MenuInRolesRepository.Delete(item.Id);
                    }
                    if (menuIds.Count > 0)
                    {
                        //thêm dữ liệu mới
                        foreach (var item in menuIds)
                        {
                            var model = new MenuInRoles();
                            model.MenuId = item;
                            model.RoleId = roleId;
                            _unitOfWork.MenuInRolesRepository.Insert(model);
                        }
                    }
                    _unitOfWork.MenuInRolesRepository.Save();
                    result = true;
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

        public void Save()
        {
            _unitOfWork.Save();
        }
        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}
