
namespace zh.fang.handle
{
    using System;
    using System.Linq.Expressions;

    public abstract class Handle: IDisposable
    {
        // 未删除状态类型
        public static readonly short[] NonDeletedStatus = { 0 };

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

        public virtual bool Add<TModel>(TModel entity, bool autoCommit = true)
            where TModel :data.entity.BaseEntity, new()
        {
            Repository.Add(entity);
            if (autoCommit)
            {
                return 0 < Repository.Commit();
            }
            return true;
        }

        public virtual bool Update<TModel>(TModel entity, bool autoCommit = true)
            where TModel:data.entity.BaseEntity, new()
        {
            Repository.Update(entity);
            if (autoCommit)
            {
                return 0 < Repository.Commit();
            }
            return true;
        }

        public virtual bool Remove<TModel>(Expression<Func<TModel, bool>> predicate = null, bool autoCommit = true)
            where TModel:data.entity.BaseEntity,new()
        {
            Repository.RemoveAny(predicate);
            if (autoCommit)
            {
                return 0 < Repository.Commit();
            }
            return true;
        }

        public bool Commit()
        {
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
