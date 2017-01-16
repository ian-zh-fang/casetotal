namespace zh.fang.repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public interface IRepository : IUnit
    {
        TModel Add<TModel>(TModel model) where TModel : class, new();

        TModel Update<TModel>(TModel model) where TModel : class, new();

        IEnumerable<TModel> RemoveAny<TModel>(Expression<Func<TModel, bool>> predicate) where TModel : class, new();

        IQueryable<TModel> Query<TModel>(Expression<Func<TModel, bool>> predicate) where TModel : class, new();
    }
}
