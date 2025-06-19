using ApdateFilmUser.Models;
using ApdateFilmUser.Services.API;
using ApdateFilmUser.Servieces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ApdateFilmUser.ViewModels
{
    public partial class CatalogViewModel : ObservableObject
    {
        [ObservableProperty]
        ObservableCollection<Media> medias;

        public CatalogViewModel()
        {
            Medias = new ObservableCollection<Media>();
            LoadMedia();
        }

        async Task LoadMedia()
        {
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    var list = await MediaServiec.GetMediaAsync();

                    foreach (var media in list)
                    {
                        Medias.Add(media);
                    }
                    return;
                }
                catch (Exception)
                {
                    await Shell.Current.DisplayAlert("Ошибка загрузки", "Проверте подключение к интернету", "ОК");
                    await Task.Delay(10000);
                }
            }
            await Shell.Current.DisplayAlert("Ошибка загрузки", $"Долгий ответ от сервера, попробуйте позже\n{ApiClient.GetURL()}", "ОК");
            
        }

        [RelayCommand]
        async Task TapMedia(Media media)
        {
            var serializedItem = JsonSerializer.Serialize(media);
            await Shell.Current.GoToAsync("media",
                new Dictionary<string, object> { { "media", media } });
        }
    }
}
