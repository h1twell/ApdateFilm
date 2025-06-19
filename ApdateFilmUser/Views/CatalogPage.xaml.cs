using ApdateFilmUser.Models;

namespace ApdateFilmUser.Views;

public partial class CatalogPage : ContentPage
{
	public CatalogPage()
	{
		InitializeComponent();
	}

    private void OnCurrentItemChanged(object sender, CurrentItemChangedEventArgs e)
    {
        if (sender is CarouselView carouselView)
        {
            foreach (var item in carouselView.VisibleViews)
            {
                if (item is Frame frame)
                {
                    VisualStateManager.GoToState(
                        frame,
                        frame.BindingContext == carouselView.CurrentItem ? "Selected" : "Unselected");
                }
            }
        }
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("catalogfiltr");
    }
}