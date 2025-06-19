using ApdateFilmUser.Models;
using ApdateFilmUser.Services.API;
using ApdateFilmUser.Servieces;
using ApdateFilmUser.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Maui.Devices;

namespace ApdateFilmUser.Views;

[QueryProperty(nameof(MediaItem), "media")]
public partial class MediaPage : ContentPage
{
    private int _selectedRating = 0;
    private bool _checkFavorite = false;
    public Media MediaItem { get; set; }

    public MediaPage()
    {
        InitializeComponent();
        InitializeRatingControls();

        // Подписываемся на изменение ориентации
        DeviceDisplay.Current.MainDisplayInfoChanged += OnDisplayInfoChanged;
        SetSize();
        UpdateWebViewHeight();
    }

    async Task SetSize()
    {
        await Task.Delay(2000);
        TrailerView.HeightRequest = 340;
    }

    private void OnDisplayInfoChanged(object sender, DisplayInfoChangedEventArgs e)
    {
        UpdateWebViewHeight();
    }

    private void UpdateWebViewHeight()
    {
        var displayInfo = DeviceDisplay.Current.MainDisplayInfo;

        if (displayInfo.Orientation == DisplayOrientation.Portrait)
        {
            TrailerView.HeightRequest = 340;
            Shell.SetTabBarIsVisible(this, true);
        }
        else
        {
            TrailerView.HeightRequest = 480;
            Shell.SetTabBarIsVisible(this, false);

        }


    }

    private void InitializeRatingControls()
    {
        for (int i = 1; i <= 10; i++)
        {
            var button = new Button
            {
                Text = i.ToString(),
                FontSize = 16,
                Padding = 3,
                WidthRequest = 40,
                HeightRequest = 40,
                CornerRadius = 20,
                Margin = new Thickness(3),
                BackgroundColor = Color.FromArgb("#E5E7EB"),
                TextColor = Colors.Black
            };

            button.Clicked += (sender, e) =>
            {
                // Сбрасываем цвет всех кнопок
                foreach (var child in RatingContainer.Children)
                {
                    if (child is Button btn)
                    {
                        btn.BackgroundColor = Color.FromArgb("#E5E7EB");
                        btn.TextColor = Colors.Black;
                    }
                }

                // Устанавливаем цвет для выбранной кнопки
                var selectedButton = (Button)sender;
                selectedButton.BackgroundColor = Color.FromArgb("#2563EB");
                selectedButton.TextColor = Colors.White;

                _selectedRating = int.Parse(selectedButton.Text);
            };

            RatingContainer.Children.Add(button);
        }
    }

    private async Task InitializeFavorite()
    {


        foreach (var item in MediaItem.Review)
        {
            item.User.Avatar = item.User.Avatar.Contains("assets")
                ? $"{ApiClient.GetURL()}/{item.User.Avatar}"
                : $"{ApiClient.GetURL()}/storage/{item.User.Avatar}";
        }

        _checkFavorite = await MediaServiec.CheckFavoriteAsync(MediaItem.Id);

        CheckFavorite.Source = (_checkFavorite) ? "checkfavorites.png" : "close.png";
    }
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        // Отписываемся от события при закрытии страницы
        DeviceDisplay.Current.MainDisplayInfoChanged -= OnDisplayInfoChanged;
    }

    private async void GoBack_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("../");
    }

    private async void Button_Clicked_1(object sender, EventArgs e)
    {
        MediaServiec.AddReviewAsync(MediaItem.Id, ReviewEntry.Text, _selectedRating);
        var newMedia = await MediaServiec.GetMediaAsync(MediaItem.Id);
        MediaItem.Review = newMedia.Review;
        MyReviewsInit();
        BindingContext = MediaItem;
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        if (!_checkFavorite)
        {
            MediaServiec.AddToFavoriteAsync(MediaItem.Id);
            _checkFavorite = true;
        }
        else
        {
            MediaServiec.DeleteFromFavoriteAsync(MediaItem.Id);
            _checkFavorite = false;
        }

        CheckFavorite.Source = (_checkFavorite) ? "checkfavorites.png" : "close.png";
    }

    private async void TapGestureRecognizer_Tapped_1(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("actors",
                 new Dictionary<string, object> { { "actors", MediaItem.Actors } });
    }

    private async void TapGestureRecognizer_Tapped_2(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("director",
                 new Dictionary<string, object> { { "director", MediaItem.Directors } });
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        try
        {
            base.OnNavigatedTo(args);

            if (MediaItem == null)
            {
                await Shell.Current.DisplayAlert("Ошибка", "Данные медиа не загружены", "OK");
                await Shell.Current.GoToAsync("..");
                return;
            }

            BindingContext = MediaItem;

            // Настройка интерфейса в зависимости от типа контента
            if (GoBack is View goBackView)
            {
                Grid.SetColumnSpan(goBackView, MediaItem.Type == 0 ? 2 : 1);
            }
            SelectSeries.IsVisible = MediaItem.Type != 0;

            MyReviewsInit();

            // Инициализация состояния "Избранное"
            await InitializeFavorite();

            // Настройка размера трейлера
            TrailerView.HeightRequest = DeviceInfo.Platform == DevicePlatform.Android ? 380 : 350;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Ошибка в OnNavigatedTo: {ex.Message}");
            await Shell.Current.DisplayAlert("Ошибка", "Не удалось загрузить данные", "OK");
        }
    }

    private async void MyReviewsInit()
    {
        if (ApiClient.GetToken() != string.Empty)
        {
            List<Review> reviews = new List<Review>();
            var user = await AuthServiec.GetProfileAsync();

            foreach (var item in MediaItem.Review)
            {
                if (user.id == item.User.id)
                {
                    reviews.Add(item);
                }
            }

            MyReviewsCollection.ItemsSource = reviews;
        }
    }

    private async void SelectSeries_Clicked(object sender, EventArgs e)
    {
        List<Series> series = await MediaServiec.GetSeriesAsync(MediaItem.Id);

        // Создаем список названий для отображения
        var seriesTitles = series.Select(s => $"Серия {s.series_number}").ToArray();

        // Показываем диалог выбора
        var selectedTitle = await DisplayActionSheet("Выберите серию", "Отмена", null, seriesTitles);

        // Если пользователь не нажал "Отмена" и выбрал серию
        if (selectedTitle != null && selectedTitle != "Отмена")
        {
            // Находим индекс выбранной серии
            var selectedIndex = Array.IndexOf(seriesTitles, selectedTitle);
            var selectedSeries = series[selectedIndex];

            MediaItem.ContentURL = selectedSeries.Url;
            BindingContext = null;
            BindingContext = MediaItem;
        }
    }

    private async void DeleteRewiews_Clicked(object sender, EventArgs e)
    {
        var button = (ImageButton)sender;

        if (button.BindingContext is Review reviewToDelete)
        {
            // Удаляем из коллекции
            if (MyReviewsCollection.ItemsSource is IList<Review> reviews)
            {
                if (!await MediaServiec.DeleteReviewAsync(reviewToDelete.Id)) 
                {
                    return;
                }
                

                reviews.Remove(reviewToDelete);

                MyReviewsCollection.ItemsSource = null;
                MyReviewsCollection.ItemsSource = reviews;

                MediaItem.Review.Remove(reviewToDelete);

                BindingContext = null;
                BindingContext = MediaItem;
            }
        }
    }
}