using ApdateFilmUser.Models;
using ApdateFilmUser.ViewModels;
using ApdateFilmUser.ViewModels.AuthViewModels;

namespace ApdateFilmUser.Views;


[QueryProperty(nameof(ActorItem), "actor")]

public partial class ActorPage : ContentPage
{
	public Actor ActorItem { get; set; }
	public ActorPage()
	{
		InitializeComponent();
	}

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        var vm = new ActorViewModel(ActorItem);
        BindingContext = vm;
    }
}