using ApdateFilmUser.Models;
using ApdateFilmUser.Services.API;
using ApdateFilmUser.Servieces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApdateFilmUser.ViewModels.AuthViewModels
{
    public partial class RegistrationViewModel : ObservableObject
    {
        [ObservableProperty]
        private string surname;

        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private DateTime birthday;

        

        [RelayCommand]
        async void ToAuth()
        {
            await Shell.Current.GoToAsync("../");
        }

        [RelayCommand]
        async Task SignIn()
        {
            var user = new User(Surname, Name, Email, Password, Birthday);

            var userResponse = await AuthServiec.RegisterAsync(user);


            if (String.IsNullOrEmpty(ApiClient.GetToken()))
            {
                Debug.WriteLine("Вход не выполнен");

                await Shell.Current.DisplayAlert("Ошибка авторизации", "Вход не выполнен", "ОК");
                return;
            }

            await Shell.Current.GoToAsync("profile");
        }
    }
}
