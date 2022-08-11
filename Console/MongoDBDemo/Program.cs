using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace MongoDBDemo
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var serializer = new DateTimeSerializer(DateTimeKind.Local, BsonType.DateTime);
            BsonSerializer.RegisterSerializer(typeof(DateTime), serializer);
           await AddAsync();
            await FindListAsync();

            Console.ReadLine();
          var doc=  new BsonArray
            {
                new BsonDocument("$match",
                new BsonDocument
                    {
                        { "vcOperatorNo", "1527224445704286208" },
                        { "vcMerChantNo", "1241985555798124192" }
                    }),
                new BsonDocument("$limit", 10)
            };

          var db=   MongoDbHelper.Instance()._database;

          var coll = db.GetCollection<BsonDocument>(nameof(SQ_UserTest));
           // var result = coll.Find<BsonDocument>(doc).ToList();
        }
        /// <summary>
        /// 获取数量
        /// </summary>
        /// <returns></returns>
        public static async Task GetCountAsync()
        {
            List<FilterDefinition<SQ_UserTest>> arr = new List<FilterDefinition<SQ_UserTest>>();
            FilterDefinitionBuilder<SQ_UserTest> builder = Builders<SQ_UserTest>.Filter;
            DateTime dateTime = DateTime.Now.AddDays(-30);
            arr.Add(builder.And(builder.Gte(t => t.dateTime, dateTime)));
            FilterDefinition<SQ_UserTest> filter = builder.And(arr);
            long nCount = await MongoDbHelper.Instance().CountAsync(filter);

            Console.WriteLine($"获取数量 {nCount}");
        }
    
        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        public static async Task UpdateAsync()
        {
            var userTest = new SQ_UserTest { vcName = "西六1", vcUser = Guid.NewGuid().ToString(), dateTime = DateTime.Now };
          
          var r=  await MongoDbHelper.Instance().UpdateAsync(userTest, "628ee6ac5e198276851140f6", true);

            Console.WriteLine($"修改数据 {r.ModifiedCount}");
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <returns></returns>
        public static async Task AddAsync()
        {
            var userTest = new SQ_UserTest { vcName = "西六", vcUser = Guid.NewGuid().ToString(), dateTime = DateTime.Now };
           var r= await MongoDbHelper.Instance().AddAsync(userTest, nameof(SQ_UserTest));
            Console.WriteLine($"新增数据 {r}");
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public static async Task DeleteAsync()
        {
            var r = await MongoDbHelper.Instance().DeleteAsync<SQ_UserTest>("628ee6ac5e198276851140f6", true);
            Console.WriteLine($"删除数据 {r.DeletedCount}");
        }
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <returns></returns>
        public static async Task FindListAsync()
        {
            FilterDefinitionBuilder<SQ_UserTest> builder = Builders<SQ_UserTest>.Filter;

            DateTime dateTime = DateTime.Now.AddDays(-30);

            FilterDefinition<SQ_UserTest> filter = builder.And(builder.Gte(t=>t.dateTime, dateTime));

           var list= await MongoDbHelper.Instance().FindListAsync(filter);

            Console.WriteLine($"获取列表 {Newtonsoft.Json.JsonConvert.SerializeObject(list)}");
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        public static async Task FindOneAsync()
        {
            var list = await MongoDbHelper.Instance().FindOneAsync<SQ_UserTest>("");

            Console.WriteLine($"获取列表 {Newtonsoft.Json.JsonConvert.SerializeObject(list)}");
        }
    }
}
