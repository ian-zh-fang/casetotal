namespace zh.fang.repository
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;

    public abstract class EntityFrameworkRepository : IRepository, IUnit, IDisposable
    {
        private DbContext _db;
        private bool isDisposed;

        protected EntityFrameworkRepository(DbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }
            _db = dbContext;
            isDisposed = false;
        }

        public virtual IQueryable<TModel> Query<TModel>(Expression<Func<TModel, bool>> predicate)
            where TModel:class,new()
        {
            var query = _db.Set<TModel>().AsQueryable();
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            return query;
        }

        public virtual TModel Add<TModel>(TModel model)
            where TModel : class, new()
        {
            var set = _db.Set<TModel>();
            return set.Add(model);
        }

        public virtual IEnumerable<TModel> RemoveAny<TModel>(Expression<Func<TModel, bool>> predicate)
            where TModel : class, new()
        {
            var set = _db.Set<TModel>();
            var items = Query(predicate);
            return set.RemoveRange(items);
        }

        public virtual TModel Update<TModel>(TModel model)
            where TModel : class, new()
        {
            var set = _db.Set<TModel>();
            var m = set.Attach(model);
            var entry = _db.Entry<TModel>(model);
            entry.State = EntityState.Modified;
            return m;
        }

        public virtual int Commit()
        {
            return _db.SaveChanges();
        }

        protected virtual void Disposed(bool disposing)
        {
            if (isDisposed)
            {
                return;
            }

            if (disposing)
            {

            }

            _db.Dispose();
            _db = null;
            isDisposed = true;
        }

        void IDisposable.Dispose()
        {
            Disposed(true);
            GC.SuppressFinalize(this);
        }

        ~EntityFrameworkRepository()
        {
            Disposed(false);
        }
    }
}
