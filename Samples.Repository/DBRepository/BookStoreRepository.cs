
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Samples.Repository.Context;
using Samples.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Samples.Repository
{
    public class BookStoreRepository<TEntity> : UnitWork<BookStoreContext>, IBookStoreRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="marketInfoContext"></param>
        public BookStoreRepository(BookStoreContext _context) : base(_context)
        {

        }
       /// <summary>
       /// 添加数据
       /// </summary>
       /// <param name="model"></param>
        public  void Add(TEntity model)
        {
            _context.Set<TEntity>().Add(model);
        }
        /// <summary>
        /// 获取实体-适用于修改数据(实体跟踪)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TEntity GetById(object id)
        {
            return _context.Set<TEntity>().Find(id);
        }
        /// <summary>
        /// 获取实体-适用于修改数据(实体跟踪)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TEntity> GetByIdAsync(object id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }
        /// <summary>
        /// 根据条件获取实体-适用于修改数据(实体跟踪)
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public TEntity Get(Expression<Func<TEntity, bool>> exp = null)
        {
            return  _context.Set<TEntity>().Where(exp).FirstOrDefault();
        }
        /// <summary>
        /// 根据条件获取实体-适用于修改数据(实体跟踪)
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> exp = null)
        {
            return  await _context.Set<TEntity>().Where(exp).FirstOrDefaultAsync();
        }

       /// <summary>
       /// 修改数据
       /// </summary>
       /// <param name="model"></param>
        public  void Update(TEntity model)
        {
            _context.Set<TEntity>().Update(model);
        } 
        /// <summary>
        /// 查询是否存在
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public bool IsExist(Expression<Func<TEntity, bool>> exp)
        {
            return _context.Set<TEntity>().AsNoTracking().Any(exp);
        }

        public IQueryable<TEntity> GetAll()
        {
            return _context.Set<TEntity>();
        }

        /// <summary>
        /// 查询-适用于查询不修改数据(不实体跟踪)
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public TEntity FindSingle(Expression<Func<TEntity, bool>> exp)
        {
            return _context.Set<TEntity>().AsNoTracking().FirstOrDefault(exp);
        }
        /// <summary>
        ///  查询-适用于查询不修改数据(不实体跟踪)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<TEntity> FindSingleAsync(Expression<Func<TEntity, bool>> exp)
        {
            return await _context.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(exp);
        }
        /// <summary>
        /// 查询-适用于查询不修改数据(不实体跟踪)
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> exp = null)
        {
            return Filter(exp);
        }
        /// <summary>
        /// 分页查询-适用于查询不修改数据(不实体跟踪)
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="keySelector"></param>
        /// <param name="exp"></param>
        /// <returns></returns>
        public IQueryable<TEntity> Find(int pageindex = 1, int pagesize = 10, Expression<Func<TEntity, string>> keySelector = null, Expression<Func<TEntity, bool>> exp = null)
        {
            if (pageindex < 1) pageindex = 1;
            var filter = Filter(exp);
            if (keySelector != null)
                filter.OrderBy(keySelector);

            return filter.Skip(pagesize * (pageindex - 1)).Take(pagesize);
        }
        public int GetCount(Expression<Func<TEntity, bool>> exp = null)
        {
            return Filter(exp).Count();
        }

        private IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>> exp)
        {
            var dbSet = _context.Set<TEntity>().AsNoTracking().AsQueryable();
            if (exp != null)
                dbSet = dbSet.Where(exp);
            return dbSet;
        }
    }
}
