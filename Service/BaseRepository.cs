using Data.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;

namespace Service
{
    public partial interface IBaseRepository<T> where T : class
    {
        IEnumerable<T> GetAll();

        T Get(Expression<Func<T, bool>> expression);

        IEnumerable<T> GetMany(Expression<Func<T, bool>> expression);

        T GetById(object id);

        bool Insert(T entity);

        bool Insert(IEnumerable<T> entities);

        bool Update(T entity);

        bool Update(IEnumerable<T> entities);

        bool UpdateNotSave(T entity);

        bool Delete(object id);

        bool Delete(T entity);

        bool Delete(IEnumerable<T> entities);

        long Count(Expression<Func<T, bool>> expression);

        int CountInt(Expression<Func<T, bool>> expression);

        void Save();
        IEnumerable<T> ExecWithStoreProcedure(string query, params object[] parameters);
        void ExecStore(string query);
    }

    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        //internal DatabaseContext Context;
        internal DatabaseContext _context;
        internal DbSet<T> Table;

        public BaseRepository(DatabaseContext context)
        {
            this._context = context;
            this.Table = context.Set<T>();
        }

        public virtual IEnumerable<T> GetAll()
        {
            return Table;
        }

        public virtual T Get(Expression<Func<T, bool>> expression)
        {
            return Table.FirstOrDefault(expression);
        }

        public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> expression)
        {
            return Table.Where(expression);
        }

        public virtual T GetById(object id)
        {
            return Table.Find(id);
        }

        public virtual bool Insert(T entity)
        {
            bool result;
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");
                Table.Add(entity);
                //_context.SaveChanges();
                result = true;
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = string.Empty;

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        msg += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;

                var fail = new Exception(msg, dbEx);
                //throw fail;
                result = false;
            }
            return result;
        }

        public virtual bool Insert(IEnumerable<T> entities)
        {
            bool result;
            try
            {
                if (entities == null)
                    throw new ArgumentNullException("entities");

                foreach (var entity in entities)
                    Table.Add(entity);
                //_context.BulkInsert(entities);
                result = true;
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = string.Empty;

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        msg += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;

                var fail = new Exception(msg, dbEx);
                //throw fail;
                result = false;
            }
            return result;
        }

        public virtual bool Delete(object id)
        {
            bool result;
            try
            {
                if (id == null)
                    throw new ArgumentNullException("id");

                T entityToDelete = Table.Find(id);
                Delete(entityToDelete);
                result = true;
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = string.Empty;

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        msg += Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);

                var fail = new Exception(msg, dbEx);
                //throw fail;
                result = false;
            }
            return result;
        }

        public virtual bool Delete(T entityToDelete)
        {
            bool result;
            try
            {
                if (entityToDelete == null)
                    throw new ArgumentNullException("entityToDelete");

                if (_context.Entry(entityToDelete).State == EntityState.Detached)
                {
                    Table.Attach(entityToDelete);
                }
                Table.Remove(entityToDelete);
                //_context.SaveChanges();
                result = true;
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = string.Empty;

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        msg += Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);

                var fail = new Exception(msg, dbEx);
                //throw fail;
                result = false;
            }
            return result;
        }

        public virtual bool Delete(IEnumerable<T> entities)
        {
            bool result;
            try
            {
                if (entities == null)
                    throw new ArgumentNullException("entities");

                _context.BulkDelete(entities);
                result = true;
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = string.Empty;

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        msg += Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);

                var fail = new Exception(msg, dbEx);
                //throw fail;
                result = false;
            }
            return result;
        }

        public virtual bool Update(T entityToUpdate)
        {
            bool result;
            try
            {
                if (entityToUpdate == null)
                    throw new ArgumentNullException("entityToUpdate");

                Table.Attach(entityToUpdate);
                _context.Entry(entityToUpdate).State = EntityState.Modified;
                _context.SaveChanges();
                result = true;
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = string.Empty;

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        msg += Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);

                var fail = new Exception(msg, dbEx);
                //throw fail;
                result = false;
            }
            return result;

        }

        public virtual bool Update(IEnumerable<T> entities)
        {
            bool result;
            try
            {
                if (entities == null)
                    throw new ArgumentNullException("entities");
                foreach (var entity in entities)
                    Table.Attach(entity);
                //_context.BulkUpdate(entities);
                result = true;
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = string.Empty;

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        msg += Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);

                var fail = new Exception(msg, dbEx);
                //throw fail;
                result = false;
            }
            return result;
        }

        public virtual bool UpdateNotSave(T entityToUpdate)
        {
            bool result;
            try
            {
                if (entityToUpdate == null)
                    throw new ArgumentNullException("entityToUpdate");

                Table.Attach(entityToUpdate);
                _context.BulkUpdate(Table);
                result = true;
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = string.Empty;

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        msg += Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);

                var fail = new Exception(msg, dbEx);
                //throw fail;
                result = false;
            }
            return result;

        }

        public virtual long Count(Expression<Func<T, bool>> expression)
        {
            return Table.Count(expression);
        }

        public virtual int CountInt(Expression<Func<T, bool>> expression)
        {
            return Table.Count(expression);
        }

        public virtual void Save()
        {
            _context.SaveChanges();
        }

        public IEnumerable<T> ExecWithStoreProcedure(string query, params object[] parameters)
        {
            return _context.Database.SqlQuery<T>(query, parameters);
        }
        public virtual void ExecStore(string query)
        {
            _context.Database.ExecuteSqlCommand(query);
        }
    }
}
