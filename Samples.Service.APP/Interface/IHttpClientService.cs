using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Samples.Service.APP.Interface
{
    /// <summary>
    /// 
    /// </summary>
    public interface IHttpClientService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostUrl"></param>
        /// <param name="requestMethodUrl"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        Task<string> GetAsync(string hostUrl, string requestMethodUrl, Dictionary<string, object> headers = null);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hostUrl"></param>
        /// <param name="requestMethodUrl"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        Task<string> PostAsync(string key, string hostUrl, string requestMethodUrl, string data);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostUrl"></param>
        /// <param name="requestMethodUrl"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        Task<string> PostAsync(string hostUrl, string requestMethodUrl, string data);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestMethodUrl"></param>
        /// <param name="textMap"></param>
        /// <param name="fileMap"></param>
        /// <returns></returns>
        Task<string> PostByFormDataAsync(string requestMethodUrl, List<KeyValuePair<string, string>> textMap, List<KeyValuePair<string, byte[]>> fileMap = null);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hostUrl"></param>
        /// <param name="requestMethodUrl"></param>
        /// <param name="data"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        Task<HttpResponseMessage> PostDataAsync(string key, string hostUrl, string requestMethodUrl, string data, List<KeyValuePair<string, string>> headers = null);
    }
}
