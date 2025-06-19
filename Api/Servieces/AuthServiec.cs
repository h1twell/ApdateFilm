using ApdateFilmUser.Models;
using Api.Models.Response;
using Api.Services.API;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Servieces
{
    public static class AuthServiec
    {
        public static async Task<User> AuthAsync(User userRequest)
        {
            try
            {
                var userResponse = await ApiClient.PostAsync("api/register", userRequest);

                if (string.IsNullOrEmpty(userResponse))
                {
                    Console.WriteLine("Ошибка: Пустой ответ от сервера.");
                    return null;
                }

                var authResponse = JsonSerializer.Deserialize<AuthResponse>(userResponse, ApiClient.options);

                if (!string.IsNullOrEmpty(authResponse.Token))
                {
                    // Устанавливаем токен в ApiClient
                    ApiClient.SetToken(authResponse.Token);
                    Console.WriteLine("Токен успешно установлен.");
                    return authResponse.User;
                }
                else
                {
                    Console.WriteLine("Токен не обнаружен в ответе.");
                }
            }
            catch (Exception ex)
            {
                ApiClient.HandleException(ex);
            }
            return null;
        }

        public static async Task<User> LoginAsync(User userRequest)
        {
            try
            {
                var userResponse = await ApiClient.PostAsync("api/login", userRequest);

                if (string.IsNullOrEmpty(userResponse))
                {
                    Console.WriteLine("Ошибка: Пустой ответ от сервера.");
                    return null;
                }

                var authResponse = JsonSerializer.Deserialize<AuthResponse>(userResponse, ApiClient.options);

                if (!string.IsNullOrEmpty(authResponse.Token))
                {
                    // Устанавливаем токен в ApiClient
                    ApiClient.SetToken(authResponse.Token);
                    Console.WriteLine("Токен успешно установлен.");
                    return authResponse.User;
                }
                else
                {
                    Console.WriteLine("Токен не обнаружен в ответе.");
                }
            }
            catch (Exception ex)
            {
                ApiClient.HandleException(ex);
            }
            return null;
        }
        

    }
}
