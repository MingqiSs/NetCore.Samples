using Infrastructur.AutofacManager;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Samples.Repository.Interface
{
    public interface IUnitWork<TContext> : IDependency, IDisposable where TContext : DbContext
    {
        TContext DbContext { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IQueryable<T> GetAll<T>() where T : class;
        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        void Remove<T>(T entity) where T : class;
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        void Add<T>(T entity) where T : class;
        /// <summary>
        /// 获取实体-适用于修改数据(实体跟踪)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T> GetByIdAsync<T>(object id) where T : class;
        /// <summary>
        /// 获取实体-适用于修改数据(实体跟踪)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        T GetById<T>(object id) where T : class;
        /// <summary>
        /// 获取实体-适用于修改数据(实体跟踪)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp"></param>
        /// <returns></returns>
        T Get<T>(Expression<Func<T, bool>> exp = null) where T : class;
        /// <summary>
        /// 获取实体-适用于修改数据(实体跟踪)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp"></param>
        /// <returns></returns>
        Task<T> GetAsync<T>(Expression<Func<T, bool>> exp = null) where T : class;
        /// <summary>
        /// 获取数量
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp"></param>
        /// <returns></returns>
        Task<int> GetCountAsync<T>(Expression<Func<T, bool>> exp = null) where T : class;
        /// <summary>
        /// 获取数量
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp"></param>
        /// <returns></returns>
        int GetCount<T>(Expression<Func<T, bool>> exp = null) where T : class;
        /// <summary>
        /// 修改模型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        void Update<T>(T entity) where T : class;

        /// <summary>
        ///修改数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="properties"></param>
        void Update<T>(T entity, string[] properties) where T : class;
        /// <summary>
        /// 判断数据是否存在
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp"></param>
        /// <returns></returns>
         bool IsExist<T>(Expression<Func<T, bool>> exp) where T : class;
        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp"></param>
        /// <returns></returns>
        T FindSingle<T>(Expression<Func<T, bool>> exp) where T : class;
        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp"></param>
        /// <returns></returns>
        Task<T> FindSingleAsync<T>(Expression<Func<T, bool>> exp) where T : class;
        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp"></param>
        /// <returns></returns>
        IQueryable<T> Find<T>(Expression<Func<T, bool>> exp = null) where T : class;
        Task<List<T>> FindAsync<T>(Expression<Func<T, bool>> exp = null) where T : class;
        /// <summary>
        /// 判断数据是否存在
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp"></param>
        /// <returns></returns>
        Task<bool> IsExistAsync<T>(Expression<Func<T, bool>> exp) where T : class;
        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
         int SaveChanges();
        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
         Task<int> SaveChangesAsync();
        /// <summary>
        /// 事务保存
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        bool DbContextBeginTransaction(Func<bool> action);
        /// <summary>
        /// 执行sql 返回影响行数
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        int ExecuteSql(string sql);
        /// <summary>
        /// 使用SQL脚本查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<T> SqlQuery<T>(string sql, CommandType commandType, params object[] parameters) where T : new();
        /// <summary>
        /// 分页SQL脚本查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="page"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        List<T> SqlQueryWithPage<T>(Infrastructure.Models.PageModel page, ref int totalCount) where T : class, new();

        /// <summary>
        /// 按需修改数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        void Update<T>(Expression<Func<T, bool>> where, Expression<Func<T, T>> entity) where T : class;
        /// <summary>
        /// 执行sql
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<string> SqlQuery(string sql, CommandType commandType, params object[] parameters);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        JArray SqlQueryByJson(string sql, CommandType commandType, params object[] parameters);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        string SqlQueryByStr(string sql, CommandType commandType, params object[] parameters);

    }
}
