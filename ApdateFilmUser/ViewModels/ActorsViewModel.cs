using ApdateFilmUser.Models;
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
    public partial class ActorsViewModel : ObservableObject
    {
        [ObservableProperty]
        ObservableCollection<Actor> actors;

        public void LoadActors(IEnumerable<Actor> actors)
        {
            Actors = new ObservableCollection<Actor>(actors);
        }

        [RelayCommand]
        void ToBack()
        {
            Shell.Current.GoToAsync("../");
        }

        [RelayCommand]
        async Task TapActor(Actor actor)
        {
            var item = await MediaServiec.GetActorAsync(actor.Id);
            await Shell.Current.GoToAsync("actor",
                new Dictionary<string, object> { { "actor", item } });
        }
    }
}
