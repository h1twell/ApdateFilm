using ApdateFilmUser.Models;
using ApdateFilmUser.Servieces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ApdateFilmUser.ViewModels
{
    public partial class FavoriteViewModel : ObservableObject
    {
        [ObservableProperty]
        ObservableCollection<Media> films;
        [ObservableProperty]
        ObservableCollection<Media> serials;

        [ObservableProperty]
        ObservableCollection<Media> filmsFiltr;
        [ObservableProperty]
        ObservableCollection<Media> serialsFiltr;

        [ObservableProperty]
        ObservableCollection<Genre> genres;

        [ObservableProperty]
        ObservableCollection<DateTime> yers;

        [ObservableProperty]
        ObservableCollection<Studio> studios;

        Genre SelectedGenre;
        DateTime? SelectedDate;
        Studio SelectedStudio;

        public FavoriteViewModel()
        {
            Films = new ObservableCollection<Media>();
            Serials = new ObservableCollection<Media>();
            Genres = new ObservableCollection<Genre>();
            Yers = new ObservableCollection<DateTime>();
            Studios = new ObservableCollection<Studio>();
        }

        [RelayCommand]
        async Task TapMedia(Media media)
        {
            var serializedItem = JsonSerializer.Serialize(media);
            await Shell.Current.GoToAsync("media",
                new Dictionary<string, object> { { "media", media } });
        }

        public async Task LoadMedia()
        {
            try
            {
                Films.Clear();
                Serials.Clear();
                Genres.Clear();

                var list = await MediaServiec.GetMediaFavoriteAsync();
                if (list == null) return;

                foreach (var media in list)
                {
                    if (media.Media == null) continue;

                    if (media.Media.Type == 1)
                        Serials.Add(media.Media);
                    else
                        Films.Add(media.Media);

                    Yers.Add(media.Media.Release);
                }

                Genres = new ObservableCollection<Genre>(await MediaServiec.GetGenreAsync());
                Studios = new ObservableCollection<Studio>(await MediaServiec.GetStudioAsync());

                FilmsFiltr = new ObservableCollection<Media>(Films);
                SerialsFiltr = new ObservableCollection<Media>(Serials);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Отладка] Ошибка при загрузке медиа: {ex.Message}");
            }
        }

        private async Task FilterMedia(IEnumerable<Media> source)
        {
            try
            {
                // Применяем фильтры последовательно
                var filteredItems = source.AsEnumerable();

                // Фильтрация по году
                if (SelectedDate != null)
                {
                    filteredItems = filteredItems.Where(m => m.Release.Year == SelectedDate.Value.Year);
                }

                // Фильтрация по жанру
                if (SelectedGenre != null)
                {
                    filteredItems = filteredItems.Where(m =>
                        m.Genres?.Any(g => g.Name == SelectedGenre.Name) == true);
                }

                // Фильтрация по студии
                if (SelectedStudio != null)
                {
                    filteredItems = filteredItems.Where(m =>
                        m.Studio == SelectedStudio.Name);
                }

                // Разделяем на фильмы и сериалы
                var filteredFilms = filteredItems.Where(m => m.Type == 0).ToList();
                var filteredSerials = filteredItems.Where(m => m.Type == 1).ToList();

                // Обновляем отфильтрованные коллекции
                FilmsFiltr = new ObservableCollection<Media>(filteredFilms);
                SerialsFiltr = new ObservableCollection<Media>(filteredSerials);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"[Отладка] Ошибка при фильтрации: {ex.Message}");
                // В случае ошибки показываем все элементы
                FilmsFiltr = new ObservableCollection<Media>(Films);
                SerialsFiltr = new ObservableCollection<Media>(Serials);
            }
        }

        [RelayCommand]
        public async Task SetFiltrDate()
        {
            try
            {
                // Преобразуем года в строки и создаем массив
                var answers = Yers.Select(item => item.Year.ToString()).ToArray();

                // Показываем ActionSheet для выбора года
                string result = await Shell.Current.DisplayActionSheet(
                    "Выберите год",
                    "Отмена",
                    null,
                    answers);

                // Обрабатываем результат выбора
                if (result == null || result == "Отмена")
                {
                    SelectedDate = null;
                }
                else
                {
                    // Парсим выбранный год и устанавливаем в SelectedDate
                    if (int.TryParse(result, out int selectedYear))
                    {
                        SelectedDate = new DateTime(selectedYear, 1, 1);
                    }
                    else
                    {
                        SelectedDate = null;
                    }
                }
                // Применяем фильтрацию после выбора
                FilterMedia(Films.Concat(Films));
                FilterMedia(Films.Concat(Serials));

            }
            catch (Exception ex)
            {
                // Обработка возможных ошибок
                Console.WriteLine($"[Отладка] Ошибка при выборе года: {ex.Message}");
                SelectedDate = null;
            }
        }

        [RelayCommand]
        public async Task SetFiltrGenre()
        {
            try
            {
                // Преобразуем года в строки и создаем массив
                var answers = Genres.Select(item => item.Name.ToString()).ToArray();

                // Показываем ActionSheet для выбора года
                string result = await Shell.Current.DisplayActionSheet(
                    "Выберите год",
                    "Отмена",
                    null,
                    answers);

                // Обрабатываем результат выбора
                if (result == null || result == "Отмена")
                {
                    SelectedGenre = null;
                }
                else
                {
                    SelectedGenre = new Genre(result);
                }

                // Применяем фильтрацию после выбора
                FilterMedia(Films.Concat(Films));
                FilterMedia(Films.Concat(Serials));
            }
            catch (Exception ex)
            {
                // Обработка возможных ошибок
                Console.WriteLine($"[Отладка] Ошибка при выборе жанра: {ex.Message}");
                SelectedGenre = null;
            }
        }

        [RelayCommand]
        public async Task SetFiltrStudio()
        {
            try
            {
                // Преобразуем года в строки и создаем массив
                var answers = Studios.Select(item => item.Name.ToString()).ToArray();

                // Показываем ActionSheet для выбора года
                string result = await Shell.Current.DisplayActionSheet(
                    "Выберите Студию",
                    "Отмена",
                    null,
                    answers);

                // Обрабатываем результат выбора
                if (result == null || result == "Отмена")
                {
                    SelectedStudio = null;
                }
                else
                {
                    SelectedStudio = new Studio(result);
                }

                // Применяем фильтрацию после выбора
                FilterMedia(Films.Concat(Films));
                FilterMedia(Films.Concat(Serials));
            }
            catch (Exception ex)
            {
                // Обработка возможных ошибок
                Console.WriteLine($"[Отладка] Ошибка при выборе студия: {ex.Message}");
                SelectedStudio = null;
            }
        }

    }
}
