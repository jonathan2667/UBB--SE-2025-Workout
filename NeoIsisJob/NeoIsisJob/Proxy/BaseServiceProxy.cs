using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace NeoIsisJob.Proxy
{
    public abstract class BaseServiceProxy
    {
        protected readonly HttpClient _httpClient;
        protected readonly string _baseUrl;
        protected readonly JsonSerializerOptions _jsonOptions;

        protected BaseServiceProxy(IConfiguration configuration = null)
        {
            _httpClient = new HttpClient();
            
            // If configuration is provided, read base URL from there, otherwise use default
            if (configuration != null)
            {
                _baseUrl = configuration["ApiSettings:BaseUrl"] ?? "https://localhost:5261/";
            }
            else
            {
                _baseUrl = "https://localhost:5261/";
            }

            // Ensure the base URL ends with a slash
            if (!_baseUrl.EndsWith("/"))
            {
                _baseUrl += "/";
            }

            _httpClient.BaseAddress = new Uri(_baseUrl + "api/");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        protected async Task<T> GetAsync<T>(string url)
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>(_jsonOptions);
        }

        protected async Task<T> PostAsync<T>(string url, object data)
        {
            var response = await _httpClient.PostAsJsonAsync(url, data, _jsonOptions);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>(_jsonOptions);
        }

        protected async Task PostAsync(string url, object data)
        {
            var response = await _httpClient.PostAsJsonAsync(url, data, _jsonOptions);
            response.EnsureSuccessStatusCode();
        }

        protected async Task<T> PutAsync<T>(string url, object data)
        {
            var response = await _httpClient.PutAsJsonAsync(url, data, _jsonOptions);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>(_jsonOptions);
        }

        protected async Task PutAsync(string url, object data)
        {
            var response = await _httpClient.PutAsJsonAsync(url, data, _jsonOptions);
            response.EnsureSuccessStatusCode();
        }

        protected async Task DeleteAsync(string url)
        {
            var response = await _httpClient.DeleteAsync(url);
            response.EnsureSuccessStatusCode();
        }

        protected async Task<T> DeleteAsync<T>(string url)
        {
            var response = await _httpClient.DeleteAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>(_jsonOptions);
        }
    }
} 