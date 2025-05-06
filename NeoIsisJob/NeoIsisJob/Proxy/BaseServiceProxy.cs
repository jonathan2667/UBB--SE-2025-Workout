using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Text.Json;
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

        //protected async Task<T> GetAsync<T>(string url)
        //{
        //    try
        //    {
        //        Debug.WriteLine($"[BaseServiceProxy] GET: {_httpClient.BaseAddress}{url}");
        //        var response = await _httpClient.GetAsync(url);

        //        Debug.WriteLine($"[BaseServiceProxy] Response status: {response.StatusCode}");
        //        response.EnsureSuccessStatusCode();

        //        var result = await response.Content.ReadFromJsonAsync<T>(_jsonOptions);
        //        Debug.WriteLine($"[BaseServiceProxy] Received data: {result != null}");
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine($"[BaseServiceProxy] ERROR in GetAsync: {ex.Message}");
        //        throw;
        //    }
        //}
        protected async Task<T> GetAsync<T>(string url)
        {
            Debug.WriteLine($"[BaseServiceProxy] GET: {_httpClient.BaseAddress}{url}");
            using var response = await _httpClient.GetAsync(url);
            Debug.WriteLine($"[BaseServiceProxy] Response status: {response.StatusCode}");

            // If the server returns 204 No Content, just return default(T)
            if (response.StatusCode == HttpStatusCode.NoContent)
                return default!;

            response.EnsureSuccessStatusCode();

            // Read the raw body string first
            var json = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(json))
                return default!;    // no JSON ? return null / default

            // Deserialize the non-empty JSON
            return JsonSerializer.Deserialize<T>(json, _jsonOptions)!;
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

        //protected async Task PostAsync(string url, object data)
        //{
        //    try
        //    {
        //        Debug.WriteLine($"[BaseServiceProxy] POST: {_httpClient.BaseAddress}{url}");
        //        var response = await _httpClient.PostAsJsonAsync(url, data, _jsonOptions);

        //        Debug.WriteLine($"[BaseServiceProxy] Response status: {response.StatusCode}");
        //        response.EnsureSuccessStatusCode();
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine($"[BaseServiceProxy] ERROR in PostAsync: {ex.Message}");
        //        throw;
        //    }
        //}
        protected async Task PostAsync(string url, object data)
        {
            Debug.WriteLine($"[BaseServiceProxy] POST: {_httpClient.BaseAddress}{url}");
            var response = await _httpClient.PostAsJsonAsync(url, data, _jsonOptions);
            Debug.WriteLine($"[BaseServiceProxy] Response status: {response.StatusCode}");

            // if it succeeded (200–299), return immediately
            if (response.IsSuccessStatusCode)
                return;

            // otherwise read the error payload and throw a more descriptive exception
            var errorPayload = await response.Content.ReadAsStringAsync();
            Debug.WriteLine($"[BaseServiceProxy] POST error body: {errorPayload}");
            throw new HttpRequestException(
                $"POST {url} failed with {(int)response.StatusCode} {response.StatusCode}: {errorPayload}"
            );
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

        //protected async Task DeleteAsync(string url)
        //{
        //    try
        //    {
        //        Debug.WriteLine($"[BaseServiceProxy] DELETE: {_httpClient.BaseAddress}{url}");
        //        var response = await _httpClient.DeleteAsync(url);

        //        Debug.WriteLine($"[BaseServiceProxy] Response status: {response.StatusCode}");
        //        response.EnsureSuccessStatusCode();
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine($"[BaseServiceProxy] ERROR in DeleteAsync: {ex.Message}");
        //        throw;
        //    }
        //}
        protected async Task DeleteAsync(string url)
        {
            // Log the full request URL
            Debug.WriteLine($"[BaseServiceProxy] DELETE: {_httpClient.BaseAddress}{url}");

            // Send the DELETE
            var response = await _httpClient.DeleteAsync(url);
            Debug.WriteLine($"[BaseServiceProxy] Response status: {response.StatusCode}");

            // If it isn’t a success (2xx), read and log the error JSON, then throw
            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"[BaseServiceProxy] DELETE error body: {body}");
                response.EnsureSuccessStatusCode();
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