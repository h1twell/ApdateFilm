using ApdateFilmUser.Models;
using ApdateFilmUser.Services.API;
using ApdateFilmUser.Servieces;
using ApdateFilmUser.ViewModels;
using System.Threading.Tasks;

namespace ApdateFilmUser.Views;

[QueryProperty(nameof(DirectorsItem), "director")]
public partial class DirectorPage : ContentPage
{

    public List<Director> DirectorsItem { get; set; }
	public DirectorPage()
	{
		InitializeComponent();
	}

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        foreach (var director in DirectorsItem)
        {
            var d = await MediaServiec.GetDirectorAsync(director.Id);
            director.Photo = d.Photo;

            foreach (var media in d.Media)
            {
                media.Preview = media.Preview.Contains("assets")
                    ? $"{ApiClient.GetURL()}/{media.Preview}"
                    : $"{ApiClient.GetURL()}/storage/{media.Preview}";
            }

            director.Media = d.Media;
        }

        var vm = new DirectorsViewModel(DirectorsItem);
        BindingContext = vm;
    }
}