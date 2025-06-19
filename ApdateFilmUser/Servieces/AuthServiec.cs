using ApdateFilmUser.Models;
using ApdateFilmUser.Models.Response;
using ApdateFilmUser.Services.API;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ApdateFilmUser.Servieces
{
    public static class AuthServiec
    {
        public static async Task<User> RegisterAsync(User userRequest)
        {
            try
            {
                var us = new
                {
                    surname = userRequest.Surname,
                    name = userRequest.Name,
                    email = userRequest.Email,
                    password = userRequest.Password,
                    birthday = userRequest.Birthday
                };

                var userResponse = await ApiClient.PostAsync("api/register", us);

                if (string.IsNullOrEmpty(userResponse))
                {
                    Debug.WriteLine("Ошибка: Пустой ответ от сервера.");
                    return null;
                }

                var authResponse = JsonSerializer.Deserialize<AuthResponse>(userResponse, ApiClient.options);

                if (!string.IsNullOrEmpty(authResponse.Token))
                {
                    // Устанавливаем токен в ApdateFilmUserClient
                    ApiClient.SetToken(authResponse.Token);
                    Debug.WriteLine("Токен успешно установлен.");
                    return authResponse.User;
                }
                else
                {
                    Debug.WriteLine("Токен не обнаружен в ответе.");
                }
            }
            catch (Exception ex)
            {
                ApiClient.HandleException(ex);
            }
            return null;
        }

        public static async Task UpdateAsync(User user)
        {
            string filePath = user.Avatar; // Путь к изображению (если оно есть)

            using var formData = new MultipartFormDataContent();
            FileStream fileStream = null;

            try
            {
                // Добавляем текстовые данные пользователя в форму
                Console.WriteLine("[Отладка] Добавляем данные пользователя...");
                formData.Add(new StringContent(user.Surname), "surname");
                formData.Add(new StringContent(user.Name), "name");
                //formData.Add(new StringContent(user.Email), "email");
                formData.Add(new StringContent(user.Birthday.ToString("yyyy-MM-dd")), "birthday");

                // Если есть пароль, добавляем его
                if (!string.IsNullOrEmpty(user.Password))
                {
                    formData.Add(new StringContent(user.Password), "password");
                }

                Console.WriteLine("[Отладка] Данные пользователя успешно добавлены.");

                // Если есть аватар, добавляем его
                if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
                {
                    Console.WriteLine($"[Отладка] Загружаем файл: {filePath}");
                    fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                    var streamContent = new StreamContent(fileStream);
                    formData.Add(streamContent, "avatar", Path.GetFileName(filePath));
                    Console.WriteLine("[Отладка] Аватар добавлен в форму данных.");
                }
                else
                {
                    Console.WriteLine("[Предупреждение] Аватар не найден или путь пуст. Профиль будет обновлен без изменения аватара.");
                }

                await ApiClient.SendFormDataAsync($"api/profile", formData);
            }
            catch (Exception ex)
            {
                ApiClient.HandleException(ex);
            }
            finally
            {
                fileStream?.Dispose();
            }
        }

        public static async Task<User> LoginAsync(User userRequest)
        {
            try
            {
                var userResponse = await ApiClient.PostAsync("api/login", userRequest);

                if (string.IsNullOrEmpty(userResponse))
                {
                    Debug.WriteLine("Ошибка: Пустой ответ от сервера.");
                    return null;
                }

                var authResponse = JsonSerializer.Deserialize<AuthResponse>(userResponse, ApiClient.options);

                if (!string.IsNullOrEmpty(authResponse.Token))
                {
                    // Устанавливаем токен в ApdateFilmUserClient
                    ApiClient.SetToken(authResponse.Token);
                    Debug.WriteLine("Токен успешно установлен.");

                    authResponse.User.Avatar = authResponse.User.Avatar.Contains("assets")
                        ? $"{ApiClient.GetURL()}/{authResponse.User.Avatar}"
                        : $"{ApiClient.GetURL()}/storage/{authResponse.User.Avatar}";

                    return authResponse.User;
                }
                else
                {
                    Debug.WriteLine("Токен не обнаружен в ответе.");
                }
            }
            catch (Exception ex)
            {
                ApiClient.HandleException(ex);
            }
            return null;
        }
        
        public static async Task<User> GetProfileAsync()
        {
            try
            {
                var userResponse = await ApiClient.GetAsync("api/profile");

                if (string.IsNullOrEmpty(userResponse))
                {
                    Debug.WriteLine("Ошибка: Пустой ответ от сервера.");
                    return null;
                }

                var authResponse = JsonSerializer.Deserialize<UserResponse>(userResponse, ApiClient.options);

                authResponse.User.Avatar = authResponse.User.Avatar.Contains("assets")
                        ? $"{ApiClient.GetURL()}/{authResponse.User.Avatar}"
                        : $"{ApiClient.GetURL()}/storage/{authResponse.User.Avatar}";

                return authResponse.User;
            }
            catch (Exception ex)
            {
                ApiClient.HandleException(ex);
            }
            return null;
        }
        public static async Task LogoutAsync()
        {
            try
            {
                var userResponse = await ApiClient.GetAsync("api/logout");

                if (string.IsNullOrEmpty(userResponse))
                {
                    Debug.WriteLine("Ошибка: Пустой ответ от сервера.");
                }
            }
            catch (Exception ex)
            {
                ApiClient.HandleException(ex);
            }
        }
    }
}
