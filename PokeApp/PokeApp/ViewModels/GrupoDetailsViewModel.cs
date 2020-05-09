using Acr.UserDialogs;
using PokeApp.FirebaseRepository.Interfaces;
using PokeApp.FireBaseRepository.Interfaces;
using PokeApp.Models.FirebaseDatabase;
using PokeApp.Models.Regions;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PokeApp.ViewModels
{
    public class GrupoDetailsViewModel : ViewModelBase
    {
        #region Variables

        private INavigationService _navigationService;
        private IGruposRegionRepository _gruposRegionRepository;
        private IGrupoPokemonsRepository _grupoPokemonsRepository;
        public static GruposRegion GruposRegion { get; set; }

        #endregion

        public GrupoDetailsViewModel(INavigationService navigationService, IGruposRegionRepository gruposRegionRepository,
            IGrupoPokemonsRepository grupoPokemonsRepository)
            : base(navigationService)
        {
            _navigationService = navigationService;
            _gruposRegionRepository = gruposRegionRepository;
            _grupoPokemonsRepository = grupoPokemonsRepository;

            #region Commands Logic


            DeleteGroup = new Command(async (obj) =>
            {
                var action = await App.Current.MainPage.DisplayAlert("Warning", "Do you want to delete this group?", "Yes", "No");
                if (action)
                {
                    UserDialogs.Instance.ShowLoading(null, MaskType.Clear);

                    //delete pokemons that belong to this group
                    await _grupoPokemonsRepository.DeteleDataByGrupoId(GroupId);

                    //then, delete group
                    await _gruposRegionRepository.DeleteData(GroupId, await SecureStorage.GetAsync("UserId"), string.Empty);

                    UserDialogs.Instance.HideLoading();

                    await App.Current.MainPage.DisplayAlert("Success",
                                                  "Your group was deleted successfully", "ok");

                    await _navigationService.GoBackAsync();
                }
            });

            ModifyGroup = new Command(async (obj) =>
            {
                var navigationParams = new NavigationParameters();
                navigationParams.Add("GruposRegion", GruposRegion);
                navigationParams.Add("IsCreate", false);
                navigationParams.Add("PokemonsAdded", PokemonsAdded);
                await _navigationService.NavigateAsync("PokemonRegionView", navigationParams);

            });


            #endregion
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                UserDialogs.Instance.ShowLoading(null, MaskType.Clear);

                if (parameters != null && parameters.Count != 0)
                {
                    GroupId = (int)parameters["GrupoId"];
                    GruposRegion = await _gruposRegionRepository.GetDataById(GroupId, await SecureStorage.GetAsync("UserId"), string.Empty);
                    GroupName = GruposRegion.GrupoName;
                    GroupType = GruposRegion.GrupoTipo;
                    PokedexDescription = GruposRegion.PokedexDescription;
                    Region = GruposRegion.Region;
                    var poks = await _grupoPokemonsRepository.GetDataByGrupoId(GroupId);
                    if (poks.Count() > 0)
                    {
                        IsEmpty = false;
                        PokemonsCount = poks.Count();
                        PokemonsAdded = new ObservableCollection<GrupoPokemons>(poks);
                        UserDialogs.Instance.HideLoading();

                    }
                    else
                    {
                        IsEmpty = true;
                        PokemonsCount = 0;
                        UserDialogs.Instance.HideLoading();

                    }
                }

                UserDialogs.Instance.HideLoading();
            });

        }

        #region Bindable Properties

        private ObservableCollection<GrupoPokemons> _pokemonsAdded;

        public ObservableCollection<GrupoPokemons> PokemonsAdded
        {
            get { return _pokemonsAdded; }
            set
            {
                _pokemonsAdded = value;
                OnPropertyChanged();
            }
        }

        private string _groupName;

        public string GroupName
        {
            get { return _groupName; }
            set
            {
                _groupName = value;
                OnPropertyChanged();
            }
        }

        private string _groupType;

        public string GroupType
        {
            get { return _groupType; }
            set
            {
                _groupType = value;
                OnPropertyChanged();
            }
        }

        private string _pokedexDescription;

        public string PokedexDescription
        {
            get { return _pokedexDescription; }
            set
            {
                _pokedexDescription = value;
                OnPropertyChanged();
            }
        }

        private string _region;

        public string Region
        {
            get { return _region; }
            set
            {
                _region = value;
                OnPropertyChanged();
            }
        }

        private int _pokemonsCount;

        public int PokemonsCount
        {
            get { return _pokemonsCount; }
            set
            {
                _pokemonsCount = value;
                OnPropertyChanged();
            }
        }



        #endregion

        #region Commands

        public ICommand DeleteGroup { get; private set; }
        public ICommand ModifyGroup { get; private set; }

        #endregion
    }
}
