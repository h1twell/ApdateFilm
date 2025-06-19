
using ApdateFilmUser.Services.API;
using ApdateFilmUser.ViewModels.AuthViewModels;

namespace ApdateFilmUser.Views.AuthViews;

public partial class ProfilePage : ContentPage
{
	public ProfilePage()
	{
		InitializeComponent();
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (String.IsNullOrEmpty(ApiClient.GetToken()))
        {
            // Если пользователь не авторизован, перенаправляем на страницу авторизации
            await Shell.Current.GoToAsync("auth");
        }
        else
        {
            var vm = (ProfileViewModel)BindingContext;
            await vm.LoadProfile();
        }
    }
}