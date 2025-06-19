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
    public partial class CatalogFiltrViewModel : ObservableObject
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

        [ObservableProperty]
        private string searchText;

        Genre SelectedGenre;
        DateTime? SelectedDate;
        Studio SelectedStudio;

        public CatalogFiltrViewModel()
        {
            Films = new ObservableCollection<Media>();
            Serials = new ObservableCollection<Media>();
            Genres = new ObservableCollection<Genre>();
            Yers = new ObservableCollection<DateTime>();
            LoadMedia();
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
                Yers.Clear();

                var list = await MediaServiec.GetMediaAsync();
                if (list == null) return;

                foreach (var media in list)
                {
                    if (media == null) continue;

                    if (media.Type == 1)
                        Serials.Add(media);
                    else
                        Films.Add(media);

                    Yers.Add(media.Release);
                }

                Genres = new ObservableCollection<Genre>(await MediaServiec.GetGenreAsync());
                Studios = new ObservableCollection<Studio>(await MediaServiec.GetStudioAsync());

                ApplyFilters();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Отладка] Ошибка при загрузке медиа: {ex.Message}");
            }
        }

        private void ApplyFilters()
        {
            try
            {
                // Применяем все фильтры последовательно
                var filteredItems = Films.Concat(Serials).AsEnumerable();

                // Фильтрация по названию (поиск)
                if (!string.IsNullOrWhiteSpace(SearchText))
                {
                    filteredItems = filteredItems.Where(m =>
                        m.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
                }

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
                Console.WriteLine($"[Отладка] Ошибка при фильтрации: {ex.Message}");
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
                var answers = Yers.Select(item => item.Year.ToString()).ToArray();

                string result = await Shell.Current.DisplayActionSheet(
                    "Выберите год",
                    "Отмена",
                    null,
                    answers);

                if (result == null || result == "Отмена")
                {
                    SelectedDate = null;
                }
                else
                {
                    if (int.TryParse(result, out int selectedYear))
                    {
                        SelectedDate = new DateTime(selectedYear, 1, 1);
                    }
                    else
                    {
                        SelectedDate = null;
                    }
                }
                ApplyFilters();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Отладка] Ошибка при выборе года: {ex.Message}");
                SelectedDate = null;
            }
        }

        [RelayCommand]
        public async Task SetFiltrGenre()
        {
            try
            {
                var answers = Genres.Select(item => item.Name.ToString()).ToArray();

                string result = await Shell.Current.DisplayActionSheet(
                    "Выберите жанр",
                    "Отмена",
                    null,
                    answers);

                if (result == null || result == "Отмена")
                {
                    SelectedGenre = null;
                }
                else
                {
                    SelectedGenre = new Genre(result);
                }
                ApplyFilters();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Отладка] Ошибка при выборе жанра: {ex.Message}");
                SelectedGenre = null;
            }
        }

        [RelayCommand]
        public async Task SetFiltrStudio()
        {
            try
            {
                var answers = Studios.Select(item => item.Name.ToString()).ToArray();

                string result = await Shell.Current.DisplayActionSheet(
                    "Выберите студию",
                    "Отмена",
                    null,
                    answers);

                if (result == null || result == "Отмена")
                {
                    SelectedStudio = null;
                }
                else
                {
                    SelectedStudio = new Studio(result);
                }
                ApplyFilters();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Отладка] Ошибка при выборе студии: {ex.Message}");
                SelectedStudio = null;
            }
        }
        
        [RelayCommand]
        public void PerformSearch()
        {
            ApplyFilters();
        }

        partial void OnSearchTextChanged(string value)
        {
            ApplyFilters();
        }
    }
}