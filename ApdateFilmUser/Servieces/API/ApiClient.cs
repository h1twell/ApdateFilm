using Flurl.Http;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ApdateFilmUser.Services.API
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

        public static string GetURL()
        {
            return _baseUrl;
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
            Debug.WriteLine($"GET: {_baseUrl}/{endpoint}");
            var response = await _httpClient.GetAsync($"{_baseUrl}/{endpoint}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public static async Task<string?> PostAsync(string endpoint, object data)
        {
            string jsonData = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            Debug.WriteLine($"POST: {_baseUrl}/{endpoint}\nPayload: {jsonData}");

            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_baseUrl}/{endpoint}", content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public static async Task<string> SendFormDataAsync(string endpoint, MultipartFormDataContent formData)
        {
            Debug.WriteLine($"POST (Multipart): {_baseUrl}/{endpoint}");

            // Убедимся, что токен установлен
            if (!string.IsNullOrEmpty(_token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);
            }

            var response = await _httpClient.PostAsync($"{_baseUrl}/{endpoint}", formData);
            response.EnsureSuccessStatusCode();
            // Дополнительная отладка
            Console.WriteLine($"[Отладка] Ответ сервера: {response.StatusCode}");
            string responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"[Отладка] Тело ответа: {responseContent}");
            return await response.Content.ReadAsStringAsync();
        }
        public static async Task<bool> DeleteAsync(string endpoint)
        {
            Debug.WriteLine($"DELETE: {_baseUrl}/{endpoint}");
            var response = await _httpClient.DeleteAsync($"{_baseUrl}/{endpoint}");
            response.EnsureSuccessStatusCode();
            return true;
        }
        public static void HandleException(Exception ex)
        {
            switch (ex)
            {
                case JsonException jsonEx:
                    Debug.WriteLine($"Ошибка десериализации JSON: {jsonEx.Message}");
                    break;
                case FlurlHttpException httpEx:
                    Debug.WriteLine($"Ошибка HTTP-запроса: {httpEx.Message}");
                    if (httpEx.StatusCode.HasValue)
                    {
                        Debug.WriteLine($"HTTP статус код: {httpEx.StatusCode} Описание: {httpEx.Message}");
                    }
                    break;
                default:
                    Debug.WriteLine($"Неожиданная ошибка: {ex.Message}");
                    break;
            }
        }
    }
}