using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
namespace NeoIsisJob.Proxy
{
    public abstract class BaseServiceProxy
    {
        protected readonly HttpClient httpClient;
        protected readonly string baseUrl;
        protected readonly JsonSerializerOptions jsonOptions;

        protected BaseServiceProxy(IConfiguration configuration = null)
        {
            httpClient = new HttpClient();

            // Read base URL (no trailing "/api")
            baseUrl = configuration?["ApiSettings:BaseUrl"]?.TrimEnd('/')
                       ?? "http://localhost:5261";
            Debug.WriteLine($"[BaseServiceProxy] Using base URL: {baseUrl}");

            // Point HttpClient at /api/
            httpClient.BaseAddress = new Uri(baseUrl + "/api/");
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }
        protected async Task<T> GetAsync<T>(string url)
        {
            Debug.WriteLine($"[BaseServiceProxy] GET: {httpClient.BaseAddress}{url}");
            using var response = await httpClient.GetAsync(url);
            Debug.WriteLine($"[BaseServiceProxy] Response status: {response.StatusCode}");

            // If the server returns 204 No Content, just return default(T)
            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return default!;
            }

            response.EnsureSuccessStatusCode();

            // Read the raw body string first
            var json = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(json))
            {
                return default!;    // no JSON ? return null / default
            }

            // Deserialize the non-empty JSON
            return JsonSerializer.Deserialize<T>(json, jsonOptions)!;
        }
        protected async Task<T> PostAsync<T>(string url, object data)
        {
            try
            {
                Debug.WriteLine($"[BaseServiceProxy] POST: {httpClient.BaseAddress}{url}");
                var response = await httpClient.PostAsJsonAsync(url, data, jsonOptions);

                Debug.WriteLine($"[BaseServiceProxy] Response status: {response.StatusCode}");
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<T>(jsonOptions);
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
            Debug.WriteLine($"[BaseServiceProxy] POST: {httpClient.BaseAddress}{url}");
            var response = await httpClient.PostAsJsonAsync(url, data, jsonOptions);
            Debug.WriteLine($"[BaseServiceProxy] Response status: {response.StatusCode}");

            // if it succeeded (200–299), return immediately
            if (response.IsSuccessStatusCode)
            {
                return;
            }

            // otherwise read the error payload and throw a more descriptive exception
            var errorPayload = await response.Content.ReadAsStringAsync();
            Debug.WriteLine($"[BaseServiceProxy] POST error body: {errorPayload}");
            throw new HttpRequestException(
                $"POST {url} failed with {(int)response.StatusCode} {response.StatusCode}: {errorPayload}");
        }
        protected async Task<T> PutAsync<T>(string url, object data)
        {
            try
            {
                Debug.WriteLine($"[BaseServiceProxy] PUT: {httpClient.BaseAddress}{url}");
                var response = await httpClient.PutAsJsonAsync(url, data, jsonOptions);

                Debug.WriteLine($"[BaseServiceProxy] Response status: {response.StatusCode}");
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<T>(jsonOptions);
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
                Debug.WriteLine($"[BaseServiceProxy] PUT: {httpClient.BaseAddress}{url}");
                var response = await httpClient.PutAsJsonAsync(url, data, jsonOptions);

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
            // Log the full request URL
            Debug.WriteLine($"[BaseServiceProxy] DELETE: {httpClient.BaseAddress}{url}");

            // Send the DELETE
            var response = await httpClient.DeleteAsync(url);
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
                Debug.WriteLine($"[BaseServiceProxy] DELETE: {httpClient.BaseAddress}{url}");
                var response = await httpClient.DeleteAsync(url);

                Debug.WriteLine($"[BaseServiceProxy] Response status: {response.StatusCode}");
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<T>(jsonOptions);
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