using Microsoft.Extensions.Logging;
using Samples.Service.APP.Interface;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Samples.Service.APP.Common
{
    /// <summary>
    /// http服务
    /// </summary>
    public class HttpClientService : IHttpClientService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<HttpClientService> _logger;
        /// <summary>
        /// http服务
        /// </summary>
        public HttpClientService(IHttpClientFactory clientFactory, ILogger<HttpClientService> logger)
        {
            _clientFactory = clientFactory;
            _logger = logger;
        }
        /// <summary>
        /// 执行Get请求
        /// </summary>
        /// <param name="hostUrl"></param>
        /// <param name="requestMethodUrl"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public async Task<string> GetAsync(string hostUrl, string requestMethodUrl, Dictionary<string, object> headers = null)
        {
            try
            {
                var client = _clientFactory.CreateClient();
                client.BaseAddress = new Uri(hostUrl);
                if (headers != null)
                {
                    foreach (var item in headers)
                    {
                        client.DefaultRequestHeaders.Add(item.Key, $"{item.Value}");
                    }
                }
                var result = await client.GetAsync(requestMethodUrl);
                if (result.IsSuccessStatusCode)
                {
                    return await result.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Url:{hostUrl}{requestMethodUrl},请求失败{ex}");
            }

            return null;
        }
        /// <summary>
        /// 执行Post请求
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hostUrl"></param>
        /// <param name="requestMethodUrl"></param>
        /// <param name="data"></param> 
        /// <returns></returns>
        public async Task<string> PostAsync(string key, string hostUrl, string requestMethodUrl, string data)
        {
            try
            {
                HttpContent contentPost = new StringContent(data, Encoding.UTF8, "application/json");
                HttpClient client = null;
                if (string.IsNullOrEmpty(key))
                {
                    client = _clientFactory.CreateClient();
                }
                else
                {
                    client = _clientFactory.CreateClient(key);
                }
                if (!string.IsNullOrEmpty(hostUrl)) client.BaseAddress = new Uri(hostUrl);
                var result = await client.PostAsync(requestMethodUrl, contentPost);
                if (result.IsSuccessStatusCode)
                {
                    return await result.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Url:{hostUrl}{requestMethodUrl},请求参数:{data},请求失败{ex}");
            }

            return null;
        }
        /// <summary>
        /// 执行Post请求
        /// </summary>
        /// <param name="hostUrl"></param>
        /// <param name="requestMethodUrl"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<string> PostAsync(string hostUrl, string requestMethodUrl, string data)
        {
            try
            {
                HttpContent contentPost = new StringContent(data, Encoding.UTF8, "application/json");
                HttpClient client = _clientFactory.CreateClient();
                client.BaseAddress = new Uri(hostUrl);
                var result = await client.PostAsync(requestMethodUrl, contentPost);
                if (result.IsSuccessStatusCode)
                {
                    return await result.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Url:{hostUrl}{requestMethodUrl},请求参数:{data},请求失败{ex}");
            }

            return null;
        }

      

        public Task<string> PostAsync(string key, string hostUrl, string requestMethodUrl, List<KeyValuePair<string, object>> datas, byte[] filebuffer, List<KeyValuePair<string, string>> headers = null)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 执行Post请求
        /// </summary>
        /// <param name="requestMethodUrl"></param>
        /// <param name="textMap"></param>
        /// <param name="fileMap"></param>
        /// <returns></returns>
        public async Task<string> PostByFormDataAsync(string requestMethodUrl, List<KeyValuePair<string, string>> textMap, List<KeyValuePair<string, byte[]>> fileMap = null)
        {
            try
            {
                var client = _clientFactory.CreateClient();
                using (var multipartFormDataContent = new MultipartFormDataContent())
                {
                    if (textMap != null)
                    {
                        foreach (var item in textMap)
                        {
                            multipartFormDataContent.Add(new StringContent(item.Value.ToString()), item.Key);
                        }
                    }
                    if (fileMap != null)
                    {
                        foreach (var item in fileMap)
                        {
                            multipartFormDataContent.Add(new ByteArrayContent(item.Value), item.Key, $"{Guid.NewGuid()}");
                        }
                    }
                    var result = await client.PostAsync(requestMethodUrl, multipartFormDataContent);
                    if (result.IsSuccessStatusCode)
                    {
                        return await result.Content.ReadAsStringAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Url:{requestMethodUrl},请求失败{ex}");
            }

            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> PostDataAsync(string url, string data, List<KeyValuePair<string, string>> headers)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Content = new StringContent(data);
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                if (headers != null)
                {
                    foreach (var item in headers)
                    {
                        request.Headers.Add($"{item.Key}", $"{item.Value}");
                    }
                }
                var client = _clientFactory.CreateClient();
                var result = await client.SendAsync(request);

                _logger.LogInformation($"PostDataAsync:url:{url},token:{client.DefaultRequestHeaders.Authorization},请求参数:{data}, 返回：{result?.Content?.ReadAsStringAsync()?.Result}");
                if (result.IsSuccessStatusCode)
                {
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Url:{url},请求参数:{data},请求失败{ex}");
            }
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hostUrl"></param>
        /// <param name="requestMethodUrl"></param>
        /// <param name="data"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> PostDataAsync(string key, string hostUrl, string requestMethodUrl, string data, List<KeyValuePair<string, string>> headers = null)
        {
            try
            {
                HttpContent contentPost = new StringContent(data, Encoding.UTF8, "application/json");
                HttpClient client = null;
                if (string.IsNullOrEmpty(key))
                {
                    client = _clientFactory.CreateClient();
                }
                else
                {
                    client = _clientFactory.CreateClient(key);
                }
                if (!string.IsNullOrEmpty(hostUrl)) client.BaseAddress = new Uri(hostUrl);
                if (headers != null)
                {
                    foreach (var item in headers)
                    {

                        if (item.Key == "Authorization")
                        {
                            client.DefaultRequestHeaders.TryAddWithoutValidation(item.Key, item.Value);
                        }
                        else
                        {
                            client.DefaultRequestHeaders.Remove(item.Key);
                            client.DefaultRequestHeaders.Add(item.Key, item.Value);
                        }
                    }
                }
                var result = await client.PostAsync(requestMethodUrl, contentPost);
                _logger.LogInformation($"PostDataAsync:key:{key},hostUrl:{hostUrl}{requestMethodUrl},token:{client.DefaultRequestHeaders.Authorization},返回：{result?.Content?.ReadAsStringAsync()?.Result}");
                if (result.IsSuccessStatusCode)
                {
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Url:{hostUrl}{requestMethodUrl},请求参数:{data},请求失败{ex}");
            }

            return null;
        }
    }
}
