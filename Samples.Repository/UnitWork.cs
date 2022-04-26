using Infrastructur.AutofacManager;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json.Linq;
using Samples.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Samples.Repository
{
    public class UnitWork<TContext> : IDependency,IUnitWork<TContext> where TContext : DbContext
    {
        protected readonly TContext _context;
        public UnitWork(TContext dbContext)
        {
            _context = dbContext ?? throw new ArgumentNullException(nameof(dbContext)); 
        }

        public TContext DbContext => _context;
        public IQueryable<T> GetAll<T>() where T : class
        {
            return _context.Set<T>();
        }
        public void Remove<T>(T entity) where T : class
        {
             _context.Remove<T>(entity);
        }
        public void Add<T>(T entity) where T : class
        {
            _context.Set<T>().Add(entity);
        }
        /// <summary>
        /// 根据条件获取实体-适用于修改数据(实体跟踪)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<T> GetByIdAsync<T>(object id) where T : class
        {
            return await _context.Set<T>().FindAsync(id);
        }
        /// <summary>
        /// 获取实体-适用于修改数据(实体跟踪)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetById<T>(object id) where T : class
        {
            return _context.Set<T>().Find(id);
        }
        /// <summary>
        /// 根据条件获取实体-适用于修改数据(实体跟踪)
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public T Get<T>(Expression<Func<T, bool>> exp = null) where T : class
        {
            return _context.Set<T>().Where(exp).FirstOrDefault();
        }
        /// <summary>
        /// 根据条件获取实体-适用于修改数据(实体跟踪)
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(Expression<Func<T, bool>> exp = null) where T : class
        {
            return await _context.Set<T>().Where(exp).FirstOrDefaultAsync();
        }
        /// <summary>
        /// 获取数量
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<int> GetCountAsync<T>(Expression<Func<T, bool>> exp = null) where T : class
        {
            return await Filter(exp).CountAsync();
        }
        /// <summary>
        /// 获取数量
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp"></param>
        /// <returns></returns>
        public int GetCount<T>(Expression<Func<T, bool>> exp = null) where T : class
        {
            return Filter(exp).Count();
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public void Update<T>(T entity) where T : class
        {
            _context.Set<T>().Update(entity);
        }
     
        public void Update<T>(T entity, string[] properties) where T : class
        {
            if (properties != null && properties.Length > 0)
            {
                //PropertyInfo[] entityProperty = typeof(T).GetProperties();
                //string keyName = entityProperty.GetKeyName();
                //if (properties.Contains(keyName))
                //{
                //    properties = properties.Where(x => x != keyName).ToArray();
                //}
                //properties = properties.Where(x => entityProperty.Select(s => s.Name).Contains(x)).ToArray();
            }
            if (properties == null || properties.Length == 0)
            {
                _context.Entry(entity).State = EntityState.Modified;
            }
            var entry = _context.Entry(entity);
            properties.ToList().ForEach(x =>
            {
                entry.Property(x).IsModified = true;
            });
        }
        public bool IsExist<T>(Expression<Func<T, bool>> exp) where T : class
        {
            return _context.Set<T>().AsNoTracking().Any(exp);
        }

        public T FindSingle<T>(Expression<Func<T, bool>> exp) where T : class
        {
            return _context.Set<T>().AsNoTracking().FirstOrDefault(exp);
        }
        public async Task<T> FindSingleAsync<T>(Expression<Func<T, bool>> exp) where T : class
        {
            return await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(exp);
        }
        public IQueryable<T> Find<T>(Expression<Func<T, bool>> exp = null) where T : class
        {
            return Filter(exp);
        }
        public async Task<List<T>> FindAsync<T>(Expression<Func<T, bool>> exp = null) where T : class
        {
            return await _context.Set<T>().AsNoTracking().Where(exp).ToListAsync();
        }
        public async Task<bool> IsExistAsync<T>(Expression<Func<T, bool>> exp) where T : class
        {
            return await _context.Set<T>().AnyAsync(exp);
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <returns></returns>
        public int SaveChanges()
        {
            return _context.SaveChanges();
        }
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
        /// <summary>
        /// 执行事务
        /// </summary>
        /// <param name="action">如果返回false则回滚事务(可自行定义规则)</param>
        /// <returns></returns>
        public  bool DbContextBeginTransaction(Func<bool> action)
        {
           
            using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var isSave = action();
                    if (isSave)
                    {
                        transaction.Commit();
                    }
                    else
                    {
                        transaction.Rollback();
                    }
                    return isSave;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                   // return new WebResponseContent().Error(ex.Message);
                }
                return false;
            }
        }
        /// <summary>
        /// 执行sql返回影响行数
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int ExecuteSql(string sql)
        {
            return _context.Database.ExecuteSqlCommand(sql);
        }
        /// <summary>
        /// 执行sql
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IEnumerable<T> SqlQuery<T>(string sql, CommandType commandType, params object[] parameters) where T : new()
        {
            var connection = _context.Database.GetDbConnection();
            using (var cmd = connection.CreateCommand())
            {
                _context.Database.OpenConnection();
                cmd.CommandText = sql;
                cmd.CommandTimeout = 300000;
                cmd.CommandType = commandType;
                cmd.Parameters.AddRange(parameters);
                var dr = cmd.ExecuteReader();
                var data = new List<T>();
                // 获取字段名称集合
                var names = dr.GetSchemaTable().Select().Select(t => t.ItemArray[0]).ToList();
                while (dr.Read())
                {
                    //传入的对象
                    T item = new T();
                    //得到item的类型
                    Type type = item.GetType();
                    //返回这个类型的所有公共属性
                    PropertyInfo[] infos = type.GetProperties();

                    //判断是否基本类型

                    foreach (PropertyInfo info in infos)
                    {
                        // 判断是否包含该字段
                        if (!names.Contains(info.Name))
                        {
                            continue;
                        }

                        var ordinal = 0;
                        try
                        {
                            ordinal = dr.GetOrdinal(info.Name);
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                        //将DataRow指定列的值赋给value
                        //注意需要转换数据库中的DBNull类型
                        var value = dr.GetValue(ordinal);
                        ///如果value为null则返回
                        if (value == DBNull.Value) continue;
                        //转为属性类型值
                        value = ChangeType(value, info.PropertyType);
                        ///利用反射自动将value赋值给obj的相应公共属性
                        info.SetValue(item, value, null);
                    }
                    data.Add(item);
                }
                dr.Dispose();
                return data;
            }
        }

        /// <summary>
        ///执行分页查询,返回list     
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public List<T> SqlQueryWithPage<T>(Infrastructure.Models.PageModel page, ref int totalCount) where T : class, new()
        {
            string sql = $"Select COUNT(1) as Count From {page.Table} Where {page.Where}";
            totalCount = SqlQuery<Infrastructure.Models.PageCount>(sql,CommandType.Text).FirstOrDefault()?.Count??0;
            if (totalCount > 0)
            {
                sql = $"Select {page.Query} From {page.Table} Where {page.Where} Order BY {page.Order} LIMIT {(page.Pageindex > 0 ? page.Pageindex - 1 : 0) * page.PageSize},{page.PageSize}";
                return SqlQuery<T>(sql,CommandType.Text).ToList();
            }
            return new List<T> { };
        }
        /// <summary>
        /// 按需更新
        /// </summary>
        /// <param name="where"></param>
        /// <param name="entity"></param>
        public void Update<T>(Expression<Func<T, bool>> where, Expression<Func<T, T>> entity) where T : class
        {
            var data = _context.Set<T>().Where(where);
            _context.Set<T>().UpdateRange(data);

        }
        private IQueryable<T> Filter<T>(Expression<Func<T, bool>> exp) where T : class
        {
            var dbSet = _context.Set<T>().AsNoTracking().AsQueryable();
            if (exp != null)
                dbSet = dbSet.Where(exp);
            return dbSet;
        }
        /// <summary>
        /// 数据类型转换
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static object ChangeType(object value, Type type)
        {
            if (value == null && type.IsGenericType) return Activator.CreateInstance(type);
            if (value == null) return null;
            if (type == value.GetType()) return value;
            if (type.IsEnum)
            {
                if (value is string)
                    return Enum.Parse(type, value as string);
                else
                    return Enum.ToObject(type, value);
            }
            if (!type.IsInterface && type.IsGenericType)
            {
                Type innerType = type.GetGenericArguments()[0];
                object innerValue = ChangeType(value, innerType);
                return Activator.CreateInstance(type, new object[] { innerValue });
            }
            if (value is string && type == typeof(Guid)) return new Guid(value as string);
            if (value is string && type == typeof(Version)) return new Version(value as string);
            if (!(value is IConvertible)) return value;
            return Convert.ChangeType(value, type);
        }
        /// <summary>
        /// 执行sql
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IEnumerable<string> SqlQuery(string sql, CommandType commandType, params object[] parameters)
        {
            var connection = _context.Database.GetDbConnection();
            using (var cmd = connection.CreateCommand())
            {
                _context.Database.OpenConnection();
                cmd.CommandText = sql;
                cmd.CommandTimeout = 300000;
                cmd.CommandType = commandType;
                cmd.Parameters.AddRange(parameters);
                var dr = cmd.ExecuteReader();
                var data = new List<string>();
                while (dr.Read())
                {
                    data.Add(dr.GetValue(0).ToString());
                }
                dr.Dispose();
                return data;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public JArray SqlQueryByJson(string sql, CommandType commandType, params object[] parameters)
        {
            var connection = _context.Database.GetDbConnection();
            using (var cmd = connection.CreateCommand())
            {
                _context.Database.OpenConnection();
                cmd.CommandText = sql;
                cmd.CommandTimeout = 300000;
                cmd.CommandType = commandType;
                cmd.Parameters.AddRange(parameters);
                var dr = cmd.ExecuteReader();
                var data = new JArray();
                while (dr.Read())
                {
                    data.Add(dr.GetValue(0).ToString());
                }
                dr.Dispose();
                return data;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string SqlQueryByStr(string sql, CommandType commandType, params object[] parameters)
        {
            var connection = _context.Database.GetDbConnection();
            using (var cmd = connection.CreateCommand())
            {
                _context.Database.OpenConnection();
                cmd.CommandText = sql;
                cmd.CommandTimeout = 300000;
                cmd.CommandType = commandType;
                cmd.Parameters.AddRange(parameters);
                var data = cmd.ExecuteScalar();
                var result = string.Empty;
                if (data != null)
                {
                    result = data.ToString();
                }
                else
                {
                    result = "0";
                }

                return result;
            }
        }
        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}