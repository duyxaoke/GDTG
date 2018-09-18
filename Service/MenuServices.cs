using System.Collections.Generic;
using Core.Data;

using System.Data.SqlClient;
using System.Data;
using System.Transactions;
using Service.CacheService;
using System.Linq;
using Shared.Models;
using Core.DTO.Response;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace Service
{
    public interface IMenuServices
    {
        CRUDResult<MenuViewModel> GetById(int id);
        CRUDResult<IEnumerable<Menu>> GetAll();
        CRUDResult<IEnumerable<Menu>> GetParent();
        CRUDResult<IEnumerable<Menu>> GetChildren(int parentId);
        CRUDResult<bool> Add(MenuViewModel model);
        CRUDResult<bool> Update(MenuViewModel model);
        CRUDResult<bool> Delete(int id);
        void Save();
        void Dispose();

    }
    public class MenuServices : IMenuServices
    {
        private readonly UnitOfWork _unitOfWork;
        public MenuServices(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public CRUDResult<MenuViewModel> GetById(int id)
        {
            var obj = _unitOfWork.MenuRepository.GetById(id);
            var result = Mapper.Map<Menu, MenuViewModel>(obj);
            result.Parents = _unitOfWork.MenuRepository.GetAll()
                .Select(c=> new SelectListViewModel{
                    Text = c.Name,
                    Value = c.Id.ToString()
                });
            return new CRUDResult<MenuViewModel> { StatusCode = CRUDStatusCodeRes.Success, Data = result };

        }
        public CRUDResult<IEnumerable<Menu>> GetAll()
        {
            var result = _unitOfWork.MenuRepository.GetAll();
            return new CRUDResult<IEnumerable<Menu>> { StatusCode = CRUDStatusCodeRes.Success, Data = result };

        }
        public CRUDResult<IEnumerable<Menu>> GetParent()
        {
            var result = _unitOfWork.MenuRepository.GetMany(c => c.ParentId == null && c.IsActive == true);
            return new CRUDResult<IEnumerable<Menu>> { StatusCode = CRUDStatusCodeRes.Success, Data = result };

        }
        public CRUDResult<IEnumerable<Menu>> GetChildren(int parentId)
        {
            var result = _unitOfWork.MenuRepository.GetMany(c => c.ParentId == parentId && c.IsActive == true);
            return new CRUDResult<IEnumerable<Menu>> { StatusCode = CRUDStatusCodeRes.Success, Data = result };

        }

        public CRUDResult<bool> Add(MenuViewModel model)
        {
            var obj = Mapper.Map<MenuViewModel, Menu>(model);
            var result = _unitOfWork.MenuRepository.Insert(obj);
            return new CRUDResult<bool> { StatusCode = CRUDStatusCodeRes.Success, Data = result };
        }
        public CRUDResult<bool> Update(MenuViewModel model)
        {
            var obj = Mapper.Map<MenuViewModel, Menu>(model);
            obj.Id = model.Id;
            var result = _unitOfWork.MenuRepository.Update(obj);
            return new CRUDResult<bool> { StatusCode = CRUDStatusCodeRes.Success, Data = result };
        }
        public CRUDResult<bool> Delete(int id)
        {
            var result = _unitOfWork.MenuRepository.Delete(id);
            return new CRUDResult<bool> { StatusCode = CRUDStatusCodeRes.Success, Data = result };
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
