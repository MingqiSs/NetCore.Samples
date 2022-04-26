using Infrastructure.CacheManager;
using Infrastructure.Models;
using Samples.Repository;
using Samples.Repository.Context;
using Samples.Repository.Models;
using Samples.Service.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Samples.Service.Domain
{
    public class DictionaryDomainService : BookStoreRepository<SysDictionary>, IDictionaryDomainService
    {
        public DictionaryDomainService(BookStoreContext dbContext)
      : base(dbContext)
        {
        }
        public List<SysDictionary> GetDictionarys()
        {
            return CacheContext.Cache.Exec($"SysDictionary", () =>
            {
                return Find(q =>true).ToList();
            });

        }
        public List<SysDictionaryList> GetDictionaryList(int dic_ID)
        {
            var list = CacheContext.Cache.Exec($"SysDictionaryList", () =>
            {
                return Find<SysDictionaryList>(q => true).ToList();
            });
            return list.Where(q => q.DicId == dic_ID).OrderBy(q => q.OrderNo).ToList();
        }
        public List<DictionaryDto> GetDictionary(string[] dicNos)
        {
            var list = new List<DictionaryDto> { };
            if (dicNos == null || dicNos.Count() == 0) return list;

            var data = GetDictionarys();

            var dicConfig = new List<SysDictionary>();
            foreach (var dicNo in dicNos)
            {
                var dic = data.Where(q => q.DicNo == dicNo.ToLower()).FirstOrDefault();
                if (dic != null) dicConfig.Add(dic);
            }
            List<DicValue> GetSourceData(int dic_ID, string dbSql)
            {
                if (string.IsNullOrEmpty(dbSql))
                {
                    return GetDictionaryList(dic_ID).Select(q => new DicValue
                    {
                        Id = q.DicListId,
                        Value = q.DicValue.ToInt32(),
                        Label = q.DicName,
                        //MapValue = q.MapName,
                        // OrderNo=q.OrderNo,
                    }).ToList();
                }
                else
                {
                    return new List<DicValue>();
                   // return SqlQuery<DicValue>(dbSql, System.Data.CommandType.Text).ToList();
                }
            };

            list = dicConfig.Select(item => new DictionaryDto
            {
                DicName = item.DicName,
                DicNo = item.DicNo,
                Config = item.Config,
                Data = GetSourceData(item.DicId, item.DbSql)
            }).ToList();

            return list;
        }


        public string GetDictionaryLabel(string key, int value)
        {
            var dic = GetDictionary(new string[] { key }).FirstOrDefault();
            if (dic != null)
            {
                return dic.Data.Where(q => q.Value == value).Select(q => q.Label).FirstOrDefault() ?? string.Empty;
            }
            return string.Empty;
        }

        public string GetDictionaryMapValue(string key, int value)
        {
            var dic = GetDictionary(new string[] { key }).FirstOrDefault();
            if (dic != null)
            {
                return dic.Data.Where(q => q.Value == value).Select(q => q.MapValue).FirstOrDefault() ?? string.Empty;
            }
            return string.Empty;
        }
        public int GetDictionaryValue(string key, int id)
        {
            var dic = GetDictionary(new string[] { key }).FirstOrDefault();
            if (dic != null)
            {
                return dic.Data.Where(q => q.Id == id).Select(q => q.Value).FirstOrDefault();
            }
            return 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetDictionaryMapValueById(string key, int id)
        {
            var dic = GetDictionary(new string[] { key }).FirstOrDefault();
            if (dic != null)
            {
                return dic.Data.Where(q => q.Id == id).Select(q => q.MapValue).FirstOrDefault() ?? string.Empty;
            }
            return string.Empty;
        }
    }
}
