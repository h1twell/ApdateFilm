using ApdateFilmUser.Models;
using ApdateFilmUser.Services.API;
using ApdateFilmUser.Servieces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApdateFilmUser.ViewModels.AuthViewModels
{
    public partial class ProfileViewModel : ObservableObject
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

        [ObservableProperty]
        private string avatar;
        public async Task LoadProfile()
        {
            var user = await AuthServiec.GetProfileAsync();

            Surname = user.Surname;
            Name = user.Name;
            Email = user.Email;
            Birthday = user.Birthday;
            Avatar = user.Avatar;
        }

        [RelayCommand]
        async Task SetImage()
        {
            try
            {
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "Выберите изображение",
                    FileTypes = FilePickerFileType.Images
                });

                if (result != null)
                {
                    using var stream = await result.OpenReadAsync();

                    var filePath = Path.Combine(FileSystem.CacheDirectory, result.FileName);
                    using var newFile = File.Create(filePath);
                    await stream.CopyToAsync(newFile);

                    Avatar = filePath;
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Ошибка", $"Не удалось выбрать изображение: {ex.Message}", "OK");
            }
        }


        [RelayCommand]
        async Task ToAuth()
        {
            await AuthServiec.LogoutAsync();
            await Shell.Current.GoToAsync("auth");
        }

        [RelayCommand]
        async Task Update()
        {
            var user = new User
            {
                Surname = Surname,
                Name = Name,
                Email = Email,
                Birthday = Birthday,
                Avatar = Avatar
            };

            await AuthServiec.UpdateAsync(user);
        }
    }
}
