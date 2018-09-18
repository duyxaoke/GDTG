using System.Collections.Generic;
using System.Linq;

namespace Data
{
    public interface IRepository<T> where T : class
    {
        T GetById(object id);    
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        IQueryable<T> Table { get; }
    }
}
