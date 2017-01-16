
namespace zh.fang.handle
{
    using System;

    public abstract class Handle:System.IDisposable
    {
        private repository.IRepository _rep;

        protected Handle()
            :this(new zh.fang.data.oracle.Repository("U_CT"))
        { }

        protected Handle(repository.IRepository repository)
        {
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }
            _rep = repository;
        }

        public virtual bool Add<TModel>(TModel entity)
            where TModel :data.entity.BaseEntity, new()
        {
            Repository.Add(entity);
            return 0 < Repository.Commit();
        }

        public virtual bool Update<TModel>(TModel entity)
            where TModel:data.entity.BaseEntity, new()
        {
            Repository.Update(entity);
            return 0 < Repository.Commit();
        }

        public virtual bool Remove<TModel>(System.Linq.Expressions.Expression<Func<TModel, bool>> predicate)
            where TModel:data.entity.BaseEntity,new()
        {
            Repository.RemoveAny(predicate);
            return 0 < Repository.Commit();
        }

        /// <summary>
        /// 数据仓库
        /// </summary>
        public repository.IRepository Repository
        {
            get { return _rep; }
        }

        private bool isDisposed = false;
        protected virtual void Disposed(bool disposing)
        {
            if (isDisposed)
            {
                return;
            }

            //if (disposing)
            //{

            //}

            _rep.Dispose();
            _rep = null;

            isDisposed = true;
        }

        void IDisposable.Dispose()
        {
            Disposed(true);
            GC.SuppressFinalize(this);
        }

        ~Handle()
        {
            Disposed(false);
        }
    }
}
