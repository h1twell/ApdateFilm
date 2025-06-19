using ApdateFilmUser.Models;
using ApdateFilmUser.Models.Response;
using ApdateFilmUser.Services.API;
using System.Diagnostics;
using System.Text.Json;


namespace ApdateFilmUser.Servieces
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
                    Debug.WriteLine("Ответ от сервера пустой");
                    return null;
                }

                var mediaListResponse = JsonSerializer.Deserialize<List<Media>>(mediaResponse, ApiClient.options);

                foreach (var media in mediaListResponse)
                {
                    media.Preview = media.Preview.Contains("assets")
                        ? $"{ApiClient.GetURL()}/{media.Preview}"
                        : $"{ApiClient.GetURL()}/storage/{media.Preview}";

                    foreach (var item in media.Footages)
                    {
                        item.Photo = item.Photo.Contains("assets")
                            ? $"{ApiClient.GetURL()}/{item.Photo}"
                            : $"{ApiClient.GetURL()}/storage/{item.Photo}";
                    }

                    foreach (var item in media.Actors)
                    {
                        item.Photo = item.Photo.Contains("assets")
                            ? $"{ApiClient.GetURL()}/{item.Photo}"
                            : $"{ApiClient.GetURL()}/storage/{item.Photo}";
                    }
                }

                return mediaListResponse;
            }
            catch (Exception ex)
            {
                ApiClient.HandleException(ex);
            }

            return null;
        }
        public static async Task<Media> GetMediaAsync(int id)
        {
            try
            {
                var mediaReqwest = await ApiClient.GetAsync($"api/media/{id}");

                if (String.IsNullOrEmpty(mediaReqwest))
                {
                    Debug.WriteLine("Ответ от сервера пустой");
                    return null;
                }

                var mediaResponse = JsonSerializer.Deserialize<MediaResponse>(mediaReqwest, ApiClient.options);

                return mediaResponse.Media;
            }
            catch (Exception ex)
            {
                ApiClient.HandleException(ex);
            }

            return null;
        }
        public static async Task<List<Series>> GetSeriesAsync(int mediaId)
        {
            try
            {
                var seriesResponse = await ApiClient.GetAsync($"api/series?media_id={mediaId}");

                if (String.IsNullOrEmpty(seriesResponse))
                {
                    Debug.WriteLine("Ответ от сервера пустой");
                    return null;
                }

                var seriesList = JsonSerializer.Deserialize<List<Series>>(seriesResponse, ApiClient.options);

                return seriesList;
            }
            catch (Exception ex)
            {
                ApiClient.HandleException(ex);
                return null;
            }
        }
        public static async Task<Series> GetSeriesAsync(int mediaId, int seriesNumber)
        {
            try
            {
                var seriesResponse = await ApiClient.GetAsync($"api/series/show?media_id={mediaId}&series_number={seriesNumber}");

                if (String.IsNullOrEmpty(seriesResponse))
                {
                    Debug.WriteLine("Ответ от сервера пустой");
                    return null;
                }

                var series = JsonSerializer.Deserialize<Series>(seriesResponse, ApiClient.options);

                return series;
            }
            catch (Exception ex)
            {
                ApiClient.HandleException(ex);
                return null;
            }
        }
        public static async Task<List<Genre>> GetGenreAsync()
        {
            try
            {
                var genreResponse = await ApiClient.GetAsync("api/genres");

                if (String.IsNullOrEmpty(genreResponse))
                {
                    Debug.WriteLine("Ответ от сервера пустой");
                    return null;
                }

                var mediaListResponse = JsonSerializer.Deserialize<List<Genre>>(genreResponse, ApiClient.options);

                return mediaListResponse;
            }
            catch (Exception ex)
            {
                ApiClient.HandleException(ex);
            }

            return null;
        }
        public static async Task<List<Studio>> GetStudioAsync()
        {
            try
            {
                var genreResponse = await ApiClient.GetAsync("api/studios");

                if (String.IsNullOrEmpty(genreResponse))
                {
                    Debug.WriteLine("Ответ от сервера пустой");
                    return null;
                }

                var mediaListResponse = JsonSerializer.Deserialize<List<Studio>>(genreResponse, ApiClient.options);

                return mediaListResponse;
            }
            catch (Exception ex)
            {
                ApiClient.HandleException(ex);
            }

            return null;
        }
        public static async Task<User> AddToFavoriteAsync(int id)
        {
            try
            {
                var reqwest = new { media_id = id };

                var userResponse = await ApiClient.PostAsync("api/favorites", reqwest);

                if (string.IsNullOrEmpty(userResponse))
                {
                    Debug.WriteLine("Ошибка: Пустой ответ от сервера.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                ApiClient.HandleException(ex);
            }
            return null;
        }
        public static async Task<bool> CheckFavoriteAsync(int id)
        {
            try
            {
                var checkResponse = await ApiClient.GetAsync($"api/favorites/{id}/exist");

                if (string.IsNullOrEmpty(checkResponse))
                {
                    Debug.WriteLine("[Отладка] Ошибка: Пустой ответ от сервера.");
                    return false;
                }

                return checkResponse == "1" ? true : false;
            }
            catch (Exception ex)
            {
                ApiClient.HandleException(ex);
            }
            return false;
        }
        public static async Task DeleteFromFavoriteAsync(int id)
        {
            try
            {
                var checkResponse = await ApiClient.GetAsync($"api/favorites/media/{id}");

                if (string.IsNullOrEmpty(checkResponse))
                {
                    Debug.WriteLine("[Отладка] Ошибка: Пустой ответ от сервера.");
                    return;
                }
            }
            catch (Exception ex)
            {
                ApiClient.HandleException(ex);
            }
        }
        public static async Task<bool> AddReviewAsync(int id, string text, int rating)
        {
            try
            {
                var checkResponse = await ApiClient.PostAsync($"api/reviews?media_id={id}&text={text}&rating={rating}", "");

                if (string.IsNullOrEmpty(checkResponse))
                {
                    Debug.WriteLine("[Отладка] Ошибка: Пустой ответ от сервера.");

                    await Shell.Current.DisplayAlert("Ошибка", "Отзыв не добавлен", "ОК");
                }
                await Shell.Current.DisplayAlert("Успех", "Отзыв добавлен", "ОК");
                return true;
            }
            catch (Exception ex)
            {
                ApiClient.HandleException(ex);
                await Shell.Current.DisplayAlert("Ошибка", "Отзыв не добавлен", "ОК");
            }
            return false;
        }

        public static async Task<bool> DeleteReviewAsync(int id)
        {
            var checkResponse = await ApiClient.DeleteAsync($"api/reviews/{id}");
            return checkResponse;
        }

        public static async Task<List<FavoriteMedia>> GetMediaFavoriteAsync()
        {
            try
            {
                var mediaResponse = await ApiClient.GetAsync("api/favorites");

                if (String.IsNullOrEmpty(mediaResponse))
                {
                    Debug.WriteLine("Ответ от сервера пустой");
                    return null;
                }

                var mediaListResponse = JsonSerializer.Deserialize<List<FavoriteMedia>>(mediaResponse, ApiClient.options);


                foreach (var media in mediaListResponse)
                {
                    media.Media.Preview = media.Media.Preview.Contains("assets")
                        ? $"{ApiClient.GetURL()}/{media.Media.Preview}"
                        : $"{ApiClient.GetURL()}/storage/{media.Media.Preview}";
                }

                    return mediaListResponse;
            }
            catch (Exception ex)
            {
                ApiClient.HandleException(ex);
            }

            return null;
        }
        public static async Task<Actor> GetActorAsync(int id)
        {
            try
            {
                var mediaReqwest = await ApiClient.GetAsync($"api/actors/{id}");

                if (String.IsNullOrEmpty(mediaReqwest))
                {
                    Debug.WriteLine("Ответ от сервера пустой");
                    return null;
                }

                var mediaResponse = JsonSerializer.Deserialize<ActorResponse>(mediaReqwest, ApiClient.options);

                mediaResponse.Actor.Photo = mediaResponse.Actor.Photo.Contains("assets")
                        ? $"{ApiClient.GetURL()}/{mediaResponse.Actor.Photo}"
                        : $"{ApiClient.GetURL()}/storage/{mediaResponse.Actor.Photo}";

                foreach (var media in mediaResponse.Actor.Media)
                {
                    media.Preview = media.Preview.Contains("assets")
                        ? $"{ApiClient.GetURL()}/{media.Preview}"
                        : $"{ApiClient.GetURL()}/storage/{media.Preview}";
                }

                return mediaResponse.Actor;
            }
            catch (Exception ex)
            {
                ApiClient.HandleException(ex);
            }

            return null;
        }
        public static async Task<Director> GetDirectorAsync(int id)
        {
            try
            {
                var directorReqwest = await ApiClient.GetAsync($"api/directors/{id}");

                if (String.IsNullOrEmpty(directorReqwest))
                {
                    Debug.WriteLine("Ответ от сервера пустой");
                    return null;
                }

                var directorResponse = JsonSerializer.Deserialize<DirectorResponse>(directorReqwest, ApiClient.options);

                directorResponse.Director.Photo = directorResponse.Director.Photo.Contains("assets")
                        ? $"{ApiClient.GetURL()}/{directorResponse.Director.Photo}"
                        : $"{ApiClient.GetURL()}/storage/{directorResponse.Director.Photo}";



                return directorResponse.Director;
            }
            catch (Exception ex)
            {
                ApiClient.HandleException(ex);
            }

            return null;
        }
    }
}
