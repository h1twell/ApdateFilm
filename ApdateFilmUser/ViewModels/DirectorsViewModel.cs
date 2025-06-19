using ApdateFilmUser.Models;
using ApdateFilmUser.Servieces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApdateFilmUser.ViewModels
{
    public partial class DirectorsViewModel : ObservableObject
    {
        [ObservableProperty]
        ObservableCollection<Director> directors;

        public DirectorsViewModel(List<Director> list)
        {
            Directors = new ObservableCollection<Director>(list);
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
