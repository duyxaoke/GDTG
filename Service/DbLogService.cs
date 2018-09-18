using System.Collections.Generic;
using Core.Data;

using System.Data.SqlClient;
using System.Data;
using System.Transactions;
using Service.CacheService;
using System.Linq;
using Shared.Models;

namespace Service
{
    public interface IDbLogServices
    {
        IEnumerable<DB_LOG> GetDatatracking();
        IEnumerable<DB_LOG> GetAll();
        DB_LOG Get(int id);
        void Insert(DB_LOG model);
        void Update(DB_LOG model);
        void Delete(int id);
        void Save();
        void Dispose();

    }
    public class DbLogServices : IDbLogServices
    {
        readonly ICacheProviderService _cacheProviderService = new CacheProviderService();
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();
        public IEnumerable<DB_LOG> GetDatatracking()
        {
            // First, check the cache
            var trackingdata = _cacheProviderService.Get("TrackingLOG") as IDictionary<int, DB_LOG>;

            // If it's not in the cache, we need to read it from the repository
            if (trackingdata == null)
            {
                //kiem tra thong bao trong ngay
                trackingdata = _unitOfWork.dbLogRepository.GetAll().ToDictionary(v => v.Id);

                if (trackingdata.Any())
                {
                    // Put this data into the cache for 300 minutes
                    _cacheProviderService.Set("TrackingLOG", trackingdata); // 300 minute
                }
            }
            return trackingdata.Values;
        }
        public IEnumerable<DB_LOG> GetAll()
        {
            return GetDatatracking();
        }
        public DB_LOG Get(int id)
        {
            return _unitOfWork.dbLogRepository.GetById(id);
        }
        public void Insert(DB_LOG model)
        {
            _unitOfWork.dbLogRepository.Insert(model);
            #region Clear cache when insert or update
            _cacheProviderService.Invalidate("TrackingLOG");
            #endregion
        }
        public void Update(DB_LOG model)
        {
            _unitOfWork.dbLogRepository.Update(model);
            #region Clear cache when insert or update
            _cacheProviderService.Invalidate("TrackingLOG");
            #endregion
        }
        public void Delete(int id)
        {
            _unitOfWork.dbLogRepository.Delete(id);
            #region Clear cache when insert or update
            _cacheProviderService.Invalidate("TrackingLOG");
            #endregion
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
