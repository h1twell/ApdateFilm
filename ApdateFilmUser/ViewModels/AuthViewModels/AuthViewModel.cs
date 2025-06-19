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
    public partial class AuthViewModel : ObservableObject
    {
        [ObservableProperty]
        string email = "user@mail.ru";
        [ObservableProperty]
        string password = "user123";

        [RelayCommand]
        async void ToRegisters()
        {
            await Shell.Current.GoToAsync("registration");
        }

        [RelayCommand]
        async Task SignIn()
        {
            var user = new User(Email, Password);

            var userResponse = await AuthServiec.LoginAsync(user);
                                                
            
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
