using ApdateFilmUser.ViewModels;

namespace ApdateFilmUser.Views;

public partial class FavoritesPage : ContentPage
{

	public FavoritesPage()
	{
		InitializeComponent();
	}

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        var vm =(FavoriteViewModel)BindingContext;

        vm.LoadMedia();
    }
}