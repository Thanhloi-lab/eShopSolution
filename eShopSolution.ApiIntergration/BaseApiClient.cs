using eShopSolution.Utilities.Constant;
using eShopSolution.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ApiIntergration
{
    public class BaseApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public BaseApiClient(IHttpClientFactory httpClientFactory
            , IConfiguration configuration
            , IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }
        protected async Task<ApiResult<TResponse>> GetAsync<TResponse>(string url)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString(SystemConstant.AppSettings.Token);
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var respond = await client.GetAsync(url);
            var body = await respond.Content.ReadAsStringAsync();
            if (respond.IsSuccessStatusCode)
            {
                var result = JsonConvert.DeserializeObject<ApiSuccessResult<TResponse>>(body);
                return result;
            }

            return JsonConvert.DeserializeObject<ApiErrorResult<TResponse>>(body);
        }
        protected async Task<List<TResponse>> GetListAsync<TResponse>(string url)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString(SystemConstant.AppSettings.Token);
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var respond = await client.GetAsync(url);
            var body = await respond.Content.ReadAsStringAsync();
            if (respond.IsSuccessStatusCode)
            {
                List<TResponse> myDeserializedObjList = (List<TResponse>)JsonConvert.DeserializeObject(body, typeof(List<TResponse>));
                return myDeserializedObjList;
            }
            return JsonConvert.DeserializeObject<List<TResponse>>(body);
        }
        protected async Task<TResponse> GetListPageAsync<TResponse>(string url)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString(SystemConstant.AppSettings.Token);
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var respond = await client.GetAsync(url);
            var body = await respond.Content.ReadAsStringAsync();
            if (respond.IsSuccessStatusCode)
            {
                TResponse myDeserializedObjList = (TResponse)JsonConvert.DeserializeObject(body, typeof(TResponse));
                return myDeserializedObjList;
            }
            return JsonConvert.DeserializeObject<TResponse>(body);
        }
        protected async Task<bool> DeleteAsync(string url)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString(SystemConstant.AppSettings.Token);
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            
            var respond = await client.DeleteAsync(url);
            if (respond.IsSuccessStatusCode)
                return true;
            return false;
        }
        protected async Task<ApiResult<TResponse>> PostAsync<TResponse>(string url, object obj)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var json = JsonConvert.SerializeObject(obj);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var respond = await client.PostAsync(url, httpContent);
            var result = await respond.Content.ReadAsStringAsync();
            if (respond.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<TResponse>>(result);

            return JsonConvert.DeserializeObject<ApiErrorResult<TResponse>>(result);
        }
        protected async Task<ApiResult<TResponse>> PutAsync<TResponse>(string url, object obj)
        {
            var client = _httpClientFactory.CreateClient();
            var session = _httpContextAccessor.HttpContext.Session.GetString(SystemConstant.AppSettings.Token);
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            var json = JsonConvert.SerializeObject(obj);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var respond = await client.PutAsync(url, httpContent);
            var result = await respond.Content.ReadAsStringAsync();
            if (respond.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<TResponse>>(result);

            return JsonConvert.DeserializeObject<ApiErrorResult<TResponse>>(result);
        }
        protected async Task<ApiResult<TResponse>> AuthenticateAsync<TResponse>(string url, object obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var respond = await client.PostAsync(url, httpContent);
            var token = await respond.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<ApiErrorResult<TResponse>>(token);
        }

    }
}
