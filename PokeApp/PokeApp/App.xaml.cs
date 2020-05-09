using Prism;
using Prism.Ioc;
using PokeApp.ViewModels;
using PokeApp.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PokeApp.Services.Interfaces;
using PokeApp.Services;
using PokeApp.FireBaseRepository.Interfaces;
using PokeApp.FireBaseRepository.Repositories;
using PokeApp.FirebaseRepository.Interfaces;
using PokeApp.Models.FirebaseDatabase;
using PokeApp.FirebaseRepository;
using Xamarin.Essentials;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace PokeApp
{
    public partial class App
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */

        public static bool IsMainView { get; set; }

        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            if (await SecureStorage.GetAsync("UserId") != null)
                await NavigationService.NavigateAsync("NavigationPage/MainPage");
            else
                await NavigationService.NavigateAsync("LoginView");

        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

            #region Navigation 

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<LoginView, LoginViewModel>();
            containerRegistry.RegisterForNavigation<PokemonRegionView, PokemonRegionViewModel>();
            containerRegistry.RegisterForNavigation<RegionGruposView, RegionGruposViewModel>();
            containerRegistry.RegisterForNavigation<GrupoDetailsView, GrupoDetailsViewModel>();
            containerRegistry.RegisterForNavigation<AddGroupCopiedView, AddGroupCopiedViewModel>();

            #endregion

            #region Dependency Injection

            containerRegistry.RegisterSingleton<IAPIService, APIService>();
            containerRegistry.RegisterSingleton<IGruposRegionRepository, GruposRegionRepository>();
            containerRegistry.RegisterSingleton<IGrupoPokemonsRepository,GrupoPokemonsRepository>();


            #endregion
        }
    }
}
