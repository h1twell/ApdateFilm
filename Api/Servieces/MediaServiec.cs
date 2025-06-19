using Api.Models;
using Api.Services.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Servieces
{
    public static class MediaServiec
    {
        public static async Task<List<Media>> GetMediaAsync()
        {
            try
            {
                var mediaResponse = await ApiClient.GetAsync("api/media");

                if (String.IsNullOrEmpty(mediaResponse))
                {
                    Console.WriteLine("Ответ от сервера пустой");
                    return null;
                }

                var mediaListResponse = JsonSerializer.Deserialize<List<Media>>(mediaResponse, ApiClient.options);

                return mediaListResponse;
            }
            catch (Exception ex)
            {
                ApiClient.HandleException(ex);
            }

            return null;
        }
    }
}
