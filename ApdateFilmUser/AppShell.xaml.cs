using ApdateFilmUser.Views;
using ApdateFilmUser.Views.AuthViews;

namespace ApdateFilmUser
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Регистрация маршрутов
            Routing.RegisterRoute("registration", typeof(RegistrationPage));
            Routing.RegisterRoute("auth", typeof(AuthPage));
            Routing.RegisterRoute("profile", typeof(ProfilePage));
            Routing.RegisterRoute("catalogfiltr", typeof(CatalogFiltrPage));
            Routing.RegisterRoute("media", typeof(MediaPage));
            Routing.RegisterRoute("actors", typeof(ActorsPage));
            Routing.RegisterRoute("actor", typeof(ActorPage));
            Routing.RegisterRoute("director", typeof(DirectorPage));
        }
    }
}
