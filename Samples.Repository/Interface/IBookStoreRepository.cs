using Infrastructur.AutofacManager;
using Samples.Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Samples.Repository.Interface
{
    public interface IBookStoreRepository<TEntity> :IUnitWork<BookStoreContext> where TEntity : class
    {
        void Add(TEntity entity);
        TEntity GetById(object id);
        Task<TEntity> GetByIdAsync(object id);
        /// <summary>
        /// 根据条件获取实体(实体跟踪)
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        TEntity Get(Expression<Func<TEntity, bool>> exp = null);
        /// <summary>
        /// 根据条件获取实体(实体跟踪)
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> exp = null);

        IQueryable<TEntity> GetAll();
        void Update(TEntity entity);
        bool IsExist(Expression<Func<TEntity, bool>> exp);
        TEntity FindSingle(Expression<Func<TEntity, bool>> exp);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        Task<TEntity> FindSingleAsync(Expression<Func<TEntity, bool>> exp);
        IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> exp = null);
        IQueryable<TEntity> Find(int pageindex = 1, int pagesize = 10, Expression<Func<TEntity, string>> keySelector = null, Expression<Func<TEntity, bool>> exp = null);
        int GetCount(Expression<Func<TEntity, bool>> exp = null);
    }
}
