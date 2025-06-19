using ApdateFilmUser.Models;
using ApdateFilmUser.ViewModels;
using System.Collections.ObjectModel;

namespace ApdateFilmUser.Views;

[QueryProperty(nameof(Actors), "actors")]
public partial class ActorsPage : ContentPage
{

    public List<Actor> Actors { get; set; }

    public ActorsPage()
	{
		InitializeComponent();
	}

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        var vm = new ActorsViewModel();
        vm.LoadActors(Actors);
        BindingContext = vm;
    }
}