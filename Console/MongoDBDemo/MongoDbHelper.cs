using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBDemo
{
    /// <summary>
    /// MongoDb帮助类
    /// </summary>
    public class MongoDbHelper
    {
        //private static INacosAppsettings appsettings = ServiceProviderInstance.Instance.GetService(typeof(INacosAppsettings)) as INacosAppsettings;

        //private static readonly ILogger<NacosService> _logger = ServiceProviderInstance.Instance.GetService(typeof(ILogger<NacosService>)) as ILogger<NacosService>;

        public static MongoDbHelper _mongoDb;

        public static MongoDbHelper Instance()
        {
            _mongoDb = new MongoDbHelper();
            if (_mongoDb == null)
            {
                _mongoDb = new MongoDbHelper();
            }
            return _mongoDb;
        }

        #region 连接配置
        /// <summary>
        /// 链接
        /// </summary>
        private static readonly string conneStr = "mongodb://127.0.0.1:10004";

        /// <summary>
        /// 数据库
        /// </summary>
        private static readonly string dbName = "dbname";
        #endregion

        #region 单例创建链接
        private static IMongoClient _mongoclient { get; set; }
        private IMongoClient Client
        {
            get
            {
                if (_mongoclient == null)
                {
                    _mongoclient = new MongoClient(conneStr);
                }
                return _mongoclient;
            }
        }
        #endregion

        #region 获取链接和数据库

        public IMongoDatabase _database { get { return Client.GetDatabase(dbName); } }

        public IMongoDatabase GetDatabase()
        {
            return _database;
        }

        public IMongoCollection<T> GetClient<T>() where T : class, new()
        {
            return _database.GetCollection<T>(typeof(T).Name);
        }
        #endregion


        #region +Add 添加一条数据
        /// <summary>
        /// 添加一条数据
        /// </summary>
        /// <param name="t">添加的实体</param>
        /// <param name="host">mongodb连接信息</param>
        /// <returns></returns>
        public int Add<T>(T t, string serviceName) where T : class, new()
        {
            try
            {
                var client = _database.GetCollection<T>(serviceName);
                client.InsertOne(t);
                return 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + t.ToJson());
                return 0;
            }
        }
        #endregion

        #region +AddAsync 异步添加一条数据
        /// <summary>
        /// 异步添加一条数据
        /// </summary>
        /// <param name="t">添加的实体</param>
        /// <param name="host">mongodb连接信息</param>
        /// <returns></returns>
        public async  Task<int> AddAsync<T>(T t, string serviceName) where T : class, new()
        {
            try
            {
                var client = _database.GetCollection<T>(serviceName);
                await client.InsertOneAsync(t);

                //if (serviceName.Equals(MongoDbVar.taskCallin) || serviceName.Equals(MongoDbVar.taskCallback))
                //{
                //    _logger.LogInformation(t.ToJson());
                //}
                return 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + t.ToJson());
                return 0;
            }
        }
        #endregion

        #region +InsertMany 批量插入
        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="host">mongodb连接信息</param>
        /// <param name="t">实体集合</param>
        /// <returns></returns>
        public int InsertMany<T>(List<T> t) where T : class, new()
        {
            try
            {
                var client = _database.GetCollection<T>(typeof(T).Name);
                client.InsertMany(t);
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        #endregion

        #region +InsertManyAsync 异步批量插入
        /// <summary>
        /// 异步批量插入
        /// </summary>
        /// <param name="host">mongodb连接信息</param>
        /// <param name="t">实体集合</param>
        /// <returns></returns>
        public async Task<int> InsertManyAsync<T>(List<T> t) where T : class, new()
        {
            try
            {
                var client = _database.GetCollection<T>(typeof(T).Name);
                await client.InsertManyAsync(t);
                return 1;
            }
            catch
            {
                return 0;
            }
        }
        #endregion


        #region +Update 修改一条数据
        /// <summary>
        /// 修改一条数据
        /// </summary>
        /// <param name="t">添加的实体</param>
        /// <param name="host">mongodb连接信息</param>
        /// <returns></returns>
        public UpdateResult Update<T>(T t, string id, bool isObjectId = true) where T : class, new()
        {
            try
            {
                var client = _database.GetCollection<T>(typeof(T).Name);
                //修改条件
                FilterDefinition<T> filter;
                if (isObjectId)
                {
                    filter = Builders<T>.Filter.Eq("_id", new ObjectId(id));
                }
                else
                {
                    filter = Builders<T>.Filter.Eq("_id", id);
                }
                //要修改的字段
                var list = new List<UpdateDefinition<T>>();
                foreach (var item in t.GetType().GetProperties())
                {
                    if (item.Name.ToLower() == "id") continue;
                    list.Add(Builders<T>.Update.Set(item.Name, item.GetValue(t)));
                }
                var updatefilter = Builders<T>.Update.Combine(list);
                return client.UpdateOne(filter, updatefilter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region +UpdateAsync 异步修改一条数据
        /// <summary>
        /// 异步修改一条数据
        /// </summary>
        /// <param name="t">添加的实体</param>
        /// <param name="host">mongodb连接信息</param>
        /// <returns></returns>
        public async Task<UpdateResult> UpdateAsync<T>(T t, string id, bool isObjectId) where T : class, new()
        {
            try
            {
                var client = _database.GetCollection<T>(typeof(T).Name);
                //修改条件
                FilterDefinition<T> filter;
                if (isObjectId)
                {
                    filter = Builders<T>.Filter.Eq("_id", new ObjectId(id));
                }
                else
                {
                    filter = Builders<T>.Filter.Eq("_id", id);
                }
                //要修改的字段
                var list = new List<UpdateDefinition<T>>();
                foreach (var item in t.GetType().GetProperties())
                {
                    if (item.Name.ToLower() == "id") continue;
                    list.Add(Builders<T>.Update.Set(item.Name, item.GetValue(t)));
                }
                var updatefilter = Builders<T>.Update.Combine(list);
                return await client.UpdateOneAsync(filter, updatefilter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region +UpdateManay 批量修改数据
        /// <summary>
        /// 批量修改数据
        /// </summary>
        /// <param name="dic">要修改的字段</param>
        /// <param name="host">mongodb连接信息</param>
        /// <param name="filter">修改条件</param>
        /// <returns></returns>
        public UpdateResult UpdateManay<T>(Dictionary<string, string> dic, FilterDefinition<T> filter) where T : class, new()
        {
            try
            {
                var client = _database.GetCollection<T>(typeof(T).Name);
                T t = new T();
                //要修改的字段
                var list = new List<UpdateDefinition<T>>();
                foreach (var item in t.GetType().GetProperties())
                {
                    if (!dic.ContainsKey(item.Name)) continue;
                    var value = dic[item.Name];
                    list.Add(Builders<T>.Update.Set(item.Name, value));
                }
                var updatefilter = Builders<T>.Update.Combine(list);
                return client.UpdateMany(filter, updatefilter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region +UpdateManayAsync 异步批量修改数据
        /// <summary>
        /// 异步批量修改数据
        /// </summary>
        /// <param name="dic">要修改的字段</param>
        /// <param name="host">mongodb连接信息</param>
        /// <param name="filter">修改条件</param>
        /// <returns></returns>
        public async Task<UpdateResult> UpdateManayAsync<T>(Dictionary<string, string> dic, FilterDefinition<T> filter) where T : class, new()
        {
            try
            {
                var client = _database.GetCollection<T>(typeof(T).Name);
                T t = new T();
                //要修改的字段
                var list = new List<UpdateDefinition<T>>();
                foreach (var item in t.GetType().GetProperties())
                {
                    if (!dic.ContainsKey(item.Name)) continue;
                    var value = dic[item.Name];
                    list.Add(Builders<T>.Update.Set(item.Name, value));
                }
                var updatefilter = Builders<T>.Update.Combine(list);
                return await client.UpdateManyAsync(filter, updatefilter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Delete 删除一条数据
        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <param name="host">mongodb连接信息</param>
        /// <param name="id">objectId</param>
        /// <returns></returns>
        public DeleteResult Delete<T>(string id, bool isObjectId = true) where T : class, new()
        {
            try
            {
                var client = _database.GetCollection<T>(typeof(T).Name);
                FilterDefinition<T> filter;
                if (isObjectId)
                {
                    filter = Builders<T>.Filter.Eq("_id", new ObjectId(id));
                }
                else
                {
                    filter = Builders<T>.Filter.Eq("_id", id);
                }
                return client.DeleteOne(filter);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

        #region DeleteAsync 异步删除一条数据
        /// <summary>
        /// 异步删除一条数据
        /// </summary>
        /// <param name="host">mongodb连接信息</param>
        /// <param name="id">objectId</param>
        /// <returns></returns>
        public async Task<DeleteResult> DeleteAsync<T>(string id, bool isObjectId = true) where T : class, new()
        {
            try
            {
                var client = _database.GetCollection<T>(typeof(T).Name);
                //修改条件
                FilterDefinition<T> filter;
                if (isObjectId)
                {
                    filter = Builders<T>.Filter.Eq("_id", new ObjectId(id));
                }
                else
                {
                    filter = Builders<T>.Filter.Eq("_id", id);
                }
                return await client.DeleteOneAsync(filter);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

        #region DeleteMany 删除多条数据
        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <param name="host">mongodb连接信息</param>
        /// <param name="filter">删除的条件</param>
        /// <returns></returns>
        public DeleteResult DeleteMany<T>(FilterDefinition<T> filter) where T : class, new()
        {
            try
            {
                var client = _database.GetCollection<T>(typeof(T).Name);
                return client.DeleteMany(filter);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

        #region DeleteManyAsync 异步删除多条数据
        /// <summary>
        /// 异步删除多条数据
        /// </summary>
        /// <param name="host">mongodb连接信息</param>
        /// <param name="filter">删除的条件</param>
        /// <returns></returns>
        public async Task<DeleteResult> DeleteManyAsync<T>(FilterDefinition<T> filter) where T : class, new()
        {
            try
            {
                var client = _database.GetCollection<T>(typeof(T).Name);
                return await client.DeleteManyAsync(filter);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

        #region FindOne 根据id查询一条数据
        /// <summary>
        /// 根据id查询一条数据
        /// </summary>
        /// <param name="host">mongodb连接信息</param>
        /// <param name="id">objectid</param>
        /// <param name="field">要查询的字段，不写时查询全部</param>
        /// <returns></returns>
        public T FindOne<T>(string id, bool isObjectId = true, string[] field = null) where T : class, new()
        {
            try
            {
                var client = _database.GetCollection<T>(typeof(T).Name);
                FilterDefinition<T> filter;
                if (isObjectId)
                {
                    filter = Builders<T>.Filter.Eq("_id", new ObjectId(id));
                }
                else
                {
                    filter = Builders<T>.Filter.Eq("_id", id);
                }
                //不指定查询字段
                if (field == null || field.Length == 0)
                {
                    return client.Find(filter).FirstOrDefault<T>();
                }

                //制定查询字段
                var fieldList = new List<ProjectionDefinition<T>>();
                for (int i = 0; i < field.Length; i++)
                {
                    fieldList.Add(Builders<T>.Projection.Include(field[i].ToString()));
                }
                var projection = Builders<T>.Projection.Combine(fieldList);
                fieldList?.Clear();
                return client.Find(filter).Project<T>(projection).FirstOrDefault<T>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region FindOneAsync 异步根据id查询一条数据
        /// <summary>
        /// 异步根据id查询一条数据
        /// </summary>
        /// <param name="host">mongodb连接信息</param>
        /// <param name="id">objectid</param>
        /// <returns></returns>
        public async Task<T> FindOneAsync<T>(string id, bool isObjectId = true, string[] field = null) where T : class, new()
        {
            try
            {
                var client = _database.GetCollection<T>(typeof(T).Name);
                FilterDefinition<T> filter;
                if (isObjectId)
                {
                    filter = Builders<T>.Filter.Eq("_id", new ObjectId(id));
                }
                else
                {
                    filter = Builders<T>.Filter.Eq("_id", id);
                }

                //不指定查询字段
                if (field == null || field.Length == 0)
                {
                    return await client.Find(filter).FirstOrDefaultAsync();
                }

                //制定查询字段
                var fieldList = new List<ProjectionDefinition<T>>();
                for (int i = 0; i < field.Length; i++)
                {
                    fieldList.Add(Builders<T>.Projection.Include(field[i].ToString()));
                }
                var projection = Builders<T>.Projection.Combine(fieldList);
                fieldList?.Clear();
                return await client.Find(filter).Project<T>(projection).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region FindList 查询集合
        /// <summary>
        /// 查询集合
        /// </summary>
        /// <param name="host">mongodb连接信息</param>
        /// <param name="filter">查询条件</param>
        /// <param name="field">要查询的字段,不写时查询全部</param>
        /// <param name="sort">要排序的字段</param>
        /// <returns></returns>
        public List<T> FindList<T>(FilterDefinition<T> filter, string[] field = null, SortDefinition<T> sort = null) where T : class, new()
        {
            try
            {
                var client = _database.GetCollection<T>(typeof(T).Name);
                //不指定查询字段
                if (field == null || field.Length == 0)
                {
                    if (sort == null) return client.Find(filter).ToList();
                    //进行排序
                    return client.Find(filter).Sort(sort).ToList();
                }

                //制定查询字段
                var fieldList = new List<ProjectionDefinition<T>>();
                for (int i = 0; i < field.Length; i++)
                {
                    fieldList.Add(Builders<T>.Projection.Include(field[i].ToString()));
                }
                var projection = Builders<T>.Projection.Combine(fieldList);
                fieldList?.Clear();
                if (sort == null) return client.Find(filter).Project<T>(projection).ToList();
                //排序查询
                return client.Find(filter).Sort(sort).Project<T>(projection).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region FindListAsync 异步查询集合
        /// <summary>
        /// 异步查询集合
        /// </summary>
        /// <param name="host">mongodb连接信息</param>
        /// <param name="filter">查询条件</param>
        /// <param name="field">要查询的字段,不写时查询全部</param>
        /// <param name="sort">要排序的字段</param>
        /// <returns></returns>
        public async Task<List<T>> FindListAsync<T>(FilterDefinition<T> filter, string[] field = null, SortDefinition<T> sort = null) where T : class, new()
        {
            try
            {
                var client = _database.GetCollection<T>(typeof(T).Name);
                //不指定查询字段
                if (field == null || field.Length == 0)
                {
                    if (sort == null) return await client.Find(filter).ToListAsync();
                    return await client.Find(filter).Sort(sort).ToListAsync();
                }

                //制定查询字段
                var fieldList = new List<ProjectionDefinition<T>>();
                for (int i = 0; i < field.Length; i++)
                {
                    fieldList.Add(Builders<T>.Projection.Include(field[i].ToString()));
                }
                var projection = Builders<T>.Projection.Combine(fieldList);
                fieldList?.Clear();
                if (sort == null) return await client.Find(filter).Project<T>(projection).ToListAsync();
                //排序查询
                return await client.Find(filter).Sort(sort).Project<T>(projection).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region FindListByPage 分页查询集合
        /// <summary>
        /// 分页查询集合
        /// </summary>
        /// <param name="host">mongodb连接信息</param>
        /// <param name="filter">查询条件</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="count">总条数</param>
        /// <param name="field">要查询的字段,不写时查询全部</param>
        /// <param name="sort">要排序的字段</param>
        /// <returns></returns>
        public List<T> FindListByPage<T>(FilterDefinition<T> filter, int pageIndex, int pageSize, out long count, string[] field = null, SortDefinition<T> sort = null) where T : class, new()
        {
            try
            {
                var client = _database.GetCollection<T>(typeof(T).Name);
                count = client.CountDocuments(filter);
                //不指定查询字段
                if (field == null || field.Length == 0)
                {
                    if (sort == null) return client.Find(filter).Skip((pageIndex - 1) * pageSize).Limit(pageSize).ToList();
                    //进行排序
                    return client.Find(filter).Sort(sort).Skip((pageIndex - 1) * pageSize).Limit(pageSize).ToList();
                }

                //制定查询字段
                var fieldList = new List<ProjectionDefinition<T>>();
                for (int i = 0; i < field.Length; i++)
                {
                    fieldList.Add(Builders<T>.Projection.Include(field[i].ToString()));
                }
                var projection = Builders<T>.Projection.Combine(fieldList);
                fieldList?.Clear();

                //不排序
                if (sort == null) return client.Find(filter).Project<T>(projection).Skip((pageIndex - 1) * pageSize).Limit(pageSize).ToList();

                //排序查询
                return client.Find(filter).Sort(sort).Project<T>(projection).Skip((pageIndex - 1) * pageSize).Limit(pageSize).ToList();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region FindListByPageAsync 异步分页查询集合
        /// <summary>
        /// 异步分页查询集合
        /// </summary>
        /// <param name="host">mongodb连接信息</param>
        /// <param name="filter">查询条件</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="field">要查询的字段,不写时查询全部</param>
        /// <param name="sort">要排序的字段</param>
        /// <returns></returns>
        public async Task<List<T>> FindListByPageAsync<T>(FilterDefinition<T> filter, int pageIndex, int pageSize, string[] field = null, SortDefinition<T> sort = null) where T : class, new()
        {
            try
            {
                var client = _database.GetCollection<T>(typeof(T).Name);
                //不指定查询字段
                if (field == null || field.Length == 0)
                {
                    if (sort == null) return await client.Find(filter).Skip((pageIndex - 1) * pageSize).Limit(pageSize).ToListAsync();
                    //进行排序
                    return await client.Find(filter).Sort(sort).Skip((pageIndex - 1) * pageSize).Limit(pageSize).ToListAsync();
                }

                //制定查询字段
                var fieldList = new List<ProjectionDefinition<T>>();
                for (int i = 0; i < field.Length; i++)
                {
                    fieldList.Add(Builders<T>.Projection.Include(field[i].ToString()));
                }
                var projection = Builders<T>.Projection.Combine(fieldList);
                fieldList?.Clear();

                //不排序
                if (sort == null) return await client.Find(filter).Project<T>(projection).Skip((pageIndex - 1) * pageSize).Limit(pageSize).ToListAsync();

                //排序查询
                return await client.Find(filter).Sort(sort).Project<T>(projection).Skip((pageIndex - 1) * pageSize).Limit(pageSize).ToListAsync();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


        #region 同步分页查询集合
        /// <summary>
        /// 同步步分页查询集合
        /// </summary>
        /// <param name="host">mongodb连接信息</param>
        /// <param name="filter">查询条件</param>
        /// <param name="currentIndex">当前条数</param>
        /// <param name="pageSize">取多少条</param>
        /// <param name="field">要查询的字段,不写时查询全部</param>
        /// <param name="sort">要排序的字段</param>
        /// <returns></returns>
        public async Task<List<T>> FindListByPageIndexAsync<T>(FilterDefinition<T> filter, int currentIndex, int pageSize, string[] field = null, SortDefinition<T> sort = null) where T : class, new()
        {
            try
            {
                var client = _database.GetCollection<T>(typeof(T).Name);
                //不指定查询字段
                if (field == null || field.Length == 0)
                {
                    if (sort == null) return await client.Find(filter).Skip(currentIndex).Limit(pageSize).ToListAsync();
                    //进行排序
                    return await client.Find(filter).Sort(sort).Skip(currentIndex).Limit(pageSize).ToListAsync();
                }

                //制定查询字段
                var fieldList = new List<ProjectionDefinition<T>>();
                for (int i = 0; i < field.Length; i++)
                {
                    fieldList.Add(Builders<T>.Projection.Include(field[i].ToString()));
                }
                var projection = Builders<T>.Projection.Combine(fieldList);
                fieldList?.Clear();

                //不排序
                if (sort == null) return await client.Find(filter).Project<T>(projection).Skip(currentIndex).Limit(pageSize).ToListAsync();

                //排序查询
                return await client.Find(filter).Sort(sort).Project<T>(projection).Skip(currentIndex).Limit(pageSize).ToListAsync();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 同步步分页查询集合
        /// </summary>
        /// <param name="host">mongodb连接信息</param>
        /// <param name="filter">查询条件</param>
        /// <param name="currentIndex">当前条数</param>
        /// <param name="pageSize">取多少条</param>
        /// <param name="field">要查询的字段,不写时查询全部</param>
        /// <param name="sort">要排序的字段</param>
        /// <returns></returns>
        public List<T> FindListByPageIndex<T>(FilterDefinition<T> filter, int currentIndex, int pageSize, string[] field = null, SortDefinition<T> sort = null) where T : class, new()
        {
            try
            {
                var client = _database.GetCollection<T>(typeof(T).Name);
                //不指定查询字段
                if (field == null || field.Length == 0)
                {
                    if (sort == null) return client.Find(filter).Skip(currentIndex).Limit(pageSize).ToList();
                    //进行排序
                    return client.Find(filter).Sort(sort).Skip(currentIndex).Limit(pageSize).ToList();
                }

                //制定查询字段
                var fieldList = new List<ProjectionDefinition<T>>();
                for (int i = 0; i < field.Length; i++)
                {
                    fieldList.Add(Builders<T>.Projection.Include(field[i].ToString()));
                }
                var projection = Builders<T>.Projection.Combine(fieldList);
                fieldList?.Clear();

                //不排序
                if (sort == null) return client.Find(filter).Project<T>(projection).Skip(currentIndex).Limit(pageSize).ToList();

                //排序查询
                return client.Find(filter).Sort(sort).Project<T>(projection).Skip(currentIndex).Limit(pageSize).ToList();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        /// <summary>
        /// 异步分页查询集合
        /// </summary>
        /// <param name="host">mongodb连接信息</param>
        /// <param name="filter">查询条件 必须包含时间才可以使用</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="field">要查询的字段,不写时查询全部</param>
        /// <param name="sort">要排序的字段</param>
        /// <returns></returns>
        public async Task<List<T>> FindListByTimePageAsync<T>(FilterDefinition<T> filter, int pageSize, string[] field = null, SortDefinition<T> sort = null) where T : class, new()
        {
            try
            {
                var client = _database.GetCollection<T>(typeof(T).Name);
                //不指定查询字段
                if (field == null || field.Length == 0)
                {
                    if (sort == null) return await client.Find(filter).Limit(pageSize).ToListAsync();
                    //进行排序
                    return await client.Find(filter).Sort(sort).Limit(pageSize).ToListAsync();
                }

                //制定查询字段
                var fieldList = new List<ProjectionDefinition<T>>();
                for (int i = 0; i < field.Length; i++)
                {
                    fieldList.Add(Builders<T>.Projection.Include(field[i].ToString()));
                }
                var projection = Builders<T>.Projection.Combine(fieldList);
                fieldList?.Clear();

                //不排序
                if (sort == null) return await client.Find(filter).Project<T>(projection).Limit(pageSize).ToListAsync();

                //排序查询
                return await client.Find(filter).Sort(sort).Project<T>(projection).Limit(pageSize).ToListAsync();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region FindGroupList 查询分组集合
        /// <summary>
        /// 查询集合
        /// </summary>
        /// <param name="host">mongodb连接信息</param>
        /// <param name="filter">查询条件</param>
        /// <param name="field">要查询的字段,不写时查询全部</param>
        /// <param name="sort">要排序的字段</param>
        /// <returns></returns>
        public List<T> FindGroupList<T>(FilterDefinition<T> filter, string[] field = null,
            string[] fieldIn = null) where T : class, new()
        {
            try
            {
                var client = _database.GetCollection<T>(typeof(T).Name);

                //制定查询字段
                var fieldList = new List<ProjectionDefinition<T>>();
                for (int i = 0; i < field.Length; i++)
                {
                    fieldList.Add(Builders<T>.Projection.Include(field[i].ToString()));
                }
                var projection = Builders<T>.Projection.Combine(fieldList);
                fieldList?.Clear();
                BsonDocument db = new BsonDocument { { "vcOperatorNo", "$vcOperatorNo" }, { "SumCount", new BsonDocument("count", 1) } };
                var aggregate = client.Aggregate().Match(Builders<T>.Filter.In("vcOperatorNo", fieldIn)).Group(db);
                List<BsonDocument> list = aggregate.ToList<BsonDocument>();
                return (list.Cast<T>().ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Count 根据条件获取总数
        /// <summary>
        /// 根据条件获取总数
        /// </summary>
        /// <param name="host">mongodb连接信息</param>
        /// <param name="filter">条件</param>
        /// <returns></returns>
        public long Count<T>(FilterDefinition<T> filter) where T : class, new()
        {
            try
            {
                var client = _database.GetCollection<T>(typeof(T).Name);
                return client.CountDocuments(filter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region CountAsync 异步根据条件获取总数
        /// <summary>
        /// 异步根据条件获取总数
        /// </summary>
        /// <param name="host">mongodb连接信息</param>
        /// <param name="filter">条件</param>
        /// <returns></returns>
        public async Task<long> CountAsync<T>(FilterDefinition<T> filter) where T : class, new()
        {
            try
            {
                var client = _database.GetCollection<T>(typeof(T).Name);
                return await client.CountDocumentsAsync(filter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


        /// <summary>
        /// 根据查询条件，判断是否存在记录
        /// </summary>
        /// <param name="match">条件表达式</param>
        /// <returns></returns>
        public async Task<bool> IsExistRecord<T>(Expression<Func<T, bool>> match)
        {
            var client = _database.GetCollection<T>(typeof(T).Name);
            long nCount = await client.Find(match).CountDocumentsAsync();

            return nCount > 0 ? true : false;
        }
    }
}
