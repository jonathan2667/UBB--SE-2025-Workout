using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;

namespace NeoIsisJob.Proxy
{
    public abstract class BaseServiceProxy
    {
        protected readonly HttpClient _httpClient;
        protected readonly string _baseUrl;
        protected readonly JsonSerializerOptions _jsonOptions;

        //protected BaseServiceProxy(IConfiguration configuration = null)
        //{
        //    _httpClient = new HttpClient();

        //    // If configuration is provided, read base URL from there, otherwise use default
        //    if (configuration != null)
        //    {
        //        _baseUrl = configuration["ApiSettings:BaseUrl"] ?? "https://localhost:5261/api/";
        //    }
        //    else
        //    {
        //        _baseUrl = "https://localhost:5261/";
        //    }

        //    // Ensure the base URL ends with a slash
        //    if (!_baseUrl.EndsWith("/"))
        //    {
        //        _baseUrl += "/";
        //    }

        //    _httpClient.BaseAddress = new Uri(_baseUrl.TrimEnd('/') + "/api/");
        //    _httpClient.DefaultRequestHeaders.Accept.Clear();
        //    _httpClient.DefaultRequestHeaders.Accept.Add(
        //        new MediaTypeWithQualityHeaderValue("application/json"));

        //    _jsonOptions = new JsonSerializerOptions
        //    {
        //        PropertyNameCaseInsensitive = true,
        //        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        //    };
        //}
        
        protected BaseServiceProxy(IConfiguration configuration = null)
        {
            _httpClient = new HttpClient();

            // Read base URL (no trailing "/api")
            _baseUrl = configuration?["ApiSettings:BaseUrl"]?.TrimEnd('/') 
                       ?? "http://localhost:5261";
            
            Debug.WriteLine($"[BaseServiceProxy] Using base URL: {_baseUrl}");

            // Point HttpClient at /api/
            _httpClient.BaseAddress = new Uri(_baseUrl + "/api/");
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
            try
            {
                Debug.WriteLine($"[BaseServiceProxy] GET: {_httpClient.BaseAddress}{url}");
                var response = await _httpClient.GetAsync(url);
                
                Debug.WriteLine($"[BaseServiceProxy] Response status: {response.StatusCode}");
                response.EnsureSuccessStatusCode();
                
                var result = await response.Content.ReadFromJsonAsync<T>(_jsonOptions);
                Debug.WriteLine($"[BaseServiceProxy] Received data: {result != null}");
                return result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[BaseServiceProxy] ERROR in GetAsync: {ex.Message}");
                throw;
            }
        }

        protected async Task<T> PostAsync<T>(string url, object data)
        {
            try
            {
                Debug.WriteLine($"[BaseServiceProxy] POST: {_httpClient.BaseAddress}{url}");
                var response = await _httpClient.PostAsJsonAsync(url, data, _jsonOptions);
                
                Debug.WriteLine($"[BaseServiceProxy] Response status: {response.StatusCode}");
                response.EnsureSuccessStatusCode();
                
                var result = await response.Content.ReadFromJsonAsync<T>(_jsonOptions);
                Debug.WriteLine($"[BaseServiceProxy] Received data: {result != null}");
                return result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[BaseServiceProxy] ERROR in PostAsync<T>: {ex.Message}");
                throw;
            }
        }

        protected async Task PostAsync(string url, object data)
        {
            try
            {
                Debug.WriteLine($"[BaseServiceProxy] POST: {_httpClient.BaseAddress}{url}");
                var response = await _httpClient.PostAsJsonAsync(url, data, _jsonOptions);
                
                Debug.WriteLine($"[BaseServiceProxy] Response status: {response.StatusCode}");
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[BaseServiceProxy] ERROR in PostAsync: {ex.Message}");
                throw;
            }
        }

        protected async Task<T> PutAsync<T>(string url, object data)
        {
            try
            {
                Debug.WriteLine($"[BaseServiceProxy] PUT: {_httpClient.BaseAddress}{url}");
                var response = await _httpClient.PutAsJsonAsync(url, data, _jsonOptions);
                
                Debug.WriteLine($"[BaseServiceProxy] Response status: {response.StatusCode}");
                response.EnsureSuccessStatusCode();
                
                var result = await response.Content.ReadFromJsonAsync<T>(_jsonOptions);
                Debug.WriteLine($"[BaseServiceProxy] Received data: {result != null}");
                return result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[BaseServiceProxy] ERROR in PutAsync<T>: {ex.Message}");
                throw;
            }
        }

        protected async Task PutAsync(string url, object data)
        {
            try
            {
                Debug.WriteLine($"[BaseServiceProxy] PUT: {_httpClient.BaseAddress}{url}");
                var response = await _httpClient.PutAsJsonAsync(url, data, _jsonOptions);
                
                Debug.WriteLine($"[BaseServiceProxy] Response status: {response.StatusCode}");
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[BaseServiceProxy] ERROR in PutAsync: {ex.Message}");
                throw;
            }
        }

        protected async Task DeleteAsync(string url)
        {
            try
            {
                Debug.WriteLine($"[BaseServiceProxy] DELETE: {_httpClient.BaseAddress}{url}");
                var response = await _httpClient.DeleteAsync(url);
                
                Debug.WriteLine($"[BaseServiceProxy] Response status: {response.StatusCode}");
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[BaseServiceProxy] ERROR in DeleteAsync: {ex.Message}");
                throw;
            }
        }

        protected async Task<T> DeleteAsync<T>(string url)
        {
            try
            {
                Debug.WriteLine($"[BaseServiceProxy] DELETE: {_httpClient.BaseAddress}{url}");
                var response = await _httpClient.DeleteAsync(url);
                
                Debug.WriteLine($"[BaseServiceProxy] Response status: {response.StatusCode}");
                response.EnsureSuccessStatusCode();
                
                var result = await response.Content.ReadFromJsonAsync<T>(_jsonOptions);
                Debug.WriteLine($"[BaseServiceProxy] Received data: {result != null}");
                return result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[BaseServiceProxy] ERROR in DeleteAsync<T>: {ex.Message}");
                throw;
            }
        }
    }
} 