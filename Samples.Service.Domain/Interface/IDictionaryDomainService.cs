using Infrastructur.AutofacManager;
using Infrastructure.Models;
using Samples.Repository.Interface;
using Samples.Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Samples.Service.Domain.Interface
{
    public interface IDictionaryDomainService:IBookStoreRepository<SysDictionary>
    {
        List<DictionaryDto> GetDictionary(string[] dicNos);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        string GetDictionaryLabel(string key, int value);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        string GetDictionaryMapValue(string key, int value);
        /// <summary>
        /// 获取Dic Value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        int GetDictionaryValue(string key, int id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        string GetDictionaryMapValueById(string key, int id);
    }
}
