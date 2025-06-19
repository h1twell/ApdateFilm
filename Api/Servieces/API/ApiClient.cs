using Flurl.Http;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Services.API
{
    public static class ApiClient
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private static string _baseUrl = string.Empty;
        private static string _token = string.Empty;

        public static JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public static void Initialize(string baseUrl)
        {
            _baseUrl = baseUrl.TrimEnd('/');
        }

        public static void SetToken(string token)
        {
            _token = token;
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);
        }

        public static string GetToken()
        {
            return _token;
        }

        public static async Task<string?> GetAsync(string endpoint)
        {
            try
            {
                Console.WriteLine($"GET: {_baseUrl}/{endpoint}");
                var response = await _httpClient.GetAsync($"{_baseUrl}/{endpoint}");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"GET request failed: {ex.Message}\nStackTrace: {ex.StackTrace}");
                return null;
            }
        }

        public static async Task<string?> PostAsync(string endpoint, object data)
        {
            try
            {
                string jsonData = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                Console.WriteLine($"POST: {_baseUrl}/{endpoint}\nPayload: {jsonData}");

                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{_baseUrl}/{endpoint}", content);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"POST request failed: {ex.Message}\nStackTrace: {ex.StackTrace}");
                return null;
            }
        }

        public static async Task<bool> DeleteAsync(string endpoint)
        {
            try
            {
                Console.WriteLine($"DELETE: {_baseUrl}/{endpoint}");
                var response = await _httpClient.DeleteAsync($"{_baseUrl}/{endpoint}");
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"DELETE request failed: {ex.Message}\nStackTrace: {ex.StackTrace}");
                return false;
            }
        }

        public static void HandleException(Exception ex)
        {
            switch (ex)
            {
                case JsonException jsonEx:
                    Console.WriteLine($"Ошибка десериализации JSON: {jsonEx.Message}");
                    break;
                case FlurlHttpException httpEx:
                    Console.WriteLine($"Ошибка HTTP-запроса: {httpEx.Message}");
                    if (httpEx.StatusCode.HasValue)
                    {
                        Console.WriteLine($"HTTP статус код: {httpEx.StatusCode} Описание: {httpEx.Message}");
                    }
                    break;
                default:
                    Console.WriteLine($"Неожиданная ошибка: {ex.Message}");
                    break;
            }
        }
    }
}