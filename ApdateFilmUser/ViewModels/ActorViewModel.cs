using ApdateFilmUser.Models;
using ApdateFilmUser.Servieces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ApdateFilmUser.ViewModels
{
    partial class ActorViewModel : ObservableObject
    {
        [ObservableProperty]
        public Actor actor;

        public ActorViewModel(Actor actor)
        {
            Actor = actor;
        }
        
        [RelayCommand]
        private async void MediaSelected(object selectedItem)
        {
            if (selectedItem is Media media)
            {
                var item = await MediaServiec.GetMediaAsync(media.Id);
                await Shell.Current.GoToAsync("media",
                    new Dictionary<string, object> { { "media", item } });
            }
        }
    }
}
