using ApdateFilmUser.ViewModels;

namespace ApdateFilmUser.Views;

public partial class CatalogFiltrPage : ContentPage
{
	public CatalogFiltrPage()
	{
		InitializeComponent();
	}

    private void Button_Clicked(object sender, EventArgs e)
    {
		Shell.Current.GoToAsync("../");
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        var vm = (CatalogFiltrViewModel)BindingContext;
        vm.LoadMedia();
    }
}