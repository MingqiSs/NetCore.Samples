using System;
using KF.ElasticSearch.Config;
using Microsoft.Extensions.DependencyInjection;
using KF.ElasticSearch;
using KF.ElasticSearch.Interfaces;
using KF.ElasticSearch.Provider;
using System.Threading.Tasks;
using System.Collections.Generic;
using Nest;

namespace ElasticsearchDemo
{
    internal class Program
    {
        public static string es_index_user = "demo_user_test";
        public static IEsClientService _esClientService;
        public static IEsIndexService _esIndexService;
        static async Task Main(string[] args)
        {
            var serviceProvider = GetServiceProvider();
              _esClientService = serviceProvider.GetService<IEsClientService>();
              _esIndexService = serviceProvider.GetService<IEsIndexService>();

            //AddRange();
            GetCountQuery();
            GetTotalCount();
            //DeleteALL();
            //Get();
            Console.ReadKey();
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        public static void Get()
        {
           
            var r =  _esClientService.Client.SearchAsync<UserListSearchResultDto>(x => x.Index(es_index_user)).Result;
            var data = r.Documents;
            Console.WriteLine($"获取数据 {Newtonsoft.Json.JsonConvert.SerializeObject(data)}");
         
        }
        /// <summary>
        /// 新增数据
        /// </summary>
        public static bool Add()
        {
            var model = new UserListSearchResultDto() { vcUser = Guid.NewGuid().ToString(), vcName = "张三22" };
            var isSave = _esIndexService.Insert(model, es_index_user);
            if (isSave) Console.WriteLine("写入成功");
            return isSave;
        }
        /// <summary>
        /// 新增数据
        /// </summary>
        public static void AddRange()
        {
            var list = new List<UserListSearchResultDto>() ;
            list.Add(new UserListSearchResultDto { vcUser = Guid.NewGuid().ToString(), vcName = "张三" });
            list.Add(new UserListSearchResultDto { vcUser = Guid.NewGuid().ToString(), vcName = "李四" });
            list.Add(new UserListSearchResultDto { vcUser = Guid.NewGuid().ToString(), vcName = "王五" });
             _esIndexService.InsertRangeAsync(list, es_index_user).ConfigureAwait(true);
           
        }
        /// <summary>
        /// 修改数据
        /// </summary>
        public static void UpdateByQuery()
        {
            var mustFilters = new List<Func<QueryContainerDescriptor<UserListSearchResultDto>, QueryContainer>>();
            mustFilters.Add(t => t.Match(m => m.Field(f => f.vcName).Query("张三")));

            var response =  _esClientService.Client.UpdateByQuery<UserListSearchResultDto>(q => q.Index(es_index_user)
                                    .Query(q => q.Bool(t => t.Must(mustFilters)))
                                    .WaitForCompletion(true).Script(script => script.Source("ctx._source.vcName='wg';")
                                    ));
            if (response.Updated>0)
            {
                Console.WriteLine($"修改成功");
            }
            else
            {
                Console.WriteLine($"修改失败");
            }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        public static void DeleteByID(string Id)
        {
            BulkResponse response =  _esClientService.Client.Bulk(bulkDescriptor =>
            {
                bulkDescriptor = bulkDescriptor.Delete<UserListSearchResultDto>(bulkIndexDescriptor =>
                {
                    return bulkIndexDescriptor.Index("demo_user_test").Id(Id);
                });

                return bulkDescriptor;
            });

            if (response.IsValid)
            {
                Console.WriteLine($"删除成功");
            }
            else
            {
                Console.WriteLine($"删除失败");
            }
        }
        /// <summary>
        /// 删除索引
        /// </summary>
        public static void DeleteIndex()
        {
           _esIndexService.RemoveIndex(es_index_user);
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        public static void DeleteByQuery()
        {
            var mustFilters = new List<Func<QueryContainerDescriptor<UserListSearchResultDto>, QueryContainer>>();
            mustFilters.Add(t => t.Match(m => m.Field(f => f.vcName).Query("张三")));
            var response = _esClientService.Client.DeleteByQuery<UserListSearchResultDto>(x => x.Index(es_index_user).Query(q => q.Bool(b => b.Must(mustFilters))));
            if (response.Deleted>0)
            {
                 Console.WriteLine($"删除成功");
            }
            else
            {
                Console.WriteLine($"删除失败");
            }
        }

        /// <summary>
        /// 获取数量
        /// </summary>
        public static long GetCountQuery()
        {
            var mustFilters = new List<Func<QueryContainerDescriptor<UserListSearchResultDto>, QueryContainer>>();
            mustFilters.Add(t => t.Match(m => m.Field(f => f.vcName).Query("张三")));
            var nCount = _esClientService.Client.Count<UserListSearchResultDto>(x => x.Index(es_index_user)
                                         .Query(q => q.Bool(b => b.Must(mustFilters))));
             Console.WriteLine($"获取数量 {nCount.Count}");
            return nCount.Count;
        }

        /// <summary>
        /// 获取总数
        /// </summary>
        /// <returns></returns>
        public static long GetTotalCount()
        {
            var client = _esClientService.Client.Count<UserListSearchResultDto>(q=>q.Index(es_index_user));
            Console.WriteLine($"获取数量 {client.Count}");
            return client.Count;
        }
        /// <summary>
        /// 注入Es
        /// </summary>
        /// <returns></returns>
        public static ServiceProvider GetServiceProvider()
        {
            // 依赖注入配置集合
            var services = new ServiceCollection();
            services.AddLogging();
            services.Configure<EsConfig>(options =>
            {
                options.Urls = "http://127.0.0.1:9200/";
                options.UserName = String.Empty;
                options.Password = String.Empty;
            });
            // 置依赖注入
            services.AddTransient<IEsClientService, EsClientService>();
            services.AddTransient<IEsIndexService, EsService>();
            services.AddTransient<IEsSearchService, EsSearchService>();
            // 构建依赖注入容器
            return services.BuildServiceProvider();

        }
    }

}
