using Plugin.Connectivity;
using PokeApp.FirebaseRepository.Interfaces;
using PokeApp.FireBaseRepository.Interfaces;
using PokeApp.Models.FirebaseDatabase;
using PokeApp.Models.Pokedex;
using PokeApp.Models.Regions;
using PokeApp.Services;
using PokeApp.Services.Interfaces;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PokeApp.ViewModels
{
    public class PokemonRegionViewModel : ViewModelBase
    {
        #region Variables

        private IAPIService _apiService;
        private INavigationService _navigationService;
        private IGruposRegionRepository _gruposRegionRepository;
        private IGrupoPokemonsRepository _grupoPokemonsRepository;
        private static List<PokemonSpecies> PokemonsSelected;
        private static bool IsCreate { get; set; }

        #endregion

        public PokemonRegionViewModel(INavigationService navigationService, IAPIService apiService, IGruposRegionRepository gruposRegionRepository,
                                      IGrupoPokemonsRepository grupoPokemonsRepository)
            : base(navigationService)
        {

            _apiService = apiService;
            _navigationService = navigationService;
            _gruposRegionRepository = gruposRegionRepository;
            _grupoPokemonsRepository = grupoPokemonsRepository;

            PokemonsSelected = new List<PokemonSpecies>();

            #region Commands Logic

            CancelCreation = new Command(async () =>
            {
                await _navigationService.GoBackAsync();
            });

            SaveGroup = new Command(async () =>
            {
                try
                {
                    if (PokemonsSelected.Count < 3)
                    {
                        await App.Current.MainPage.DisplayAlert("Error",
                                                            "You must add at least 3 pokemons", "ok");
                        return;
                    }

                    var result1 = false;
                    var result2 = false;
                    if (CrossConnectivity.Current.IsConnected)
                    {
                        if (IsCreate)
                        {
                            //Create Group group first
                            var group = new GruposRegion
                            {
                                GrupoId = await _gruposRegionRepository.GetLastID(await SecureStorage.GetAsync("UserId"), string.Empty) + 1,
                                GrupoName = GroupName,
                                GrupoTipo = GroupType,
                                PokedexDescription = PokedexDescription,
                                Image = "",
                                Region = PokedexInfo.FirstOrDefault().name,
                                UserId = await SecureStorage.GetAsync("UserId")
                            };

                            result1 = await _gruposRegionRepository.SaveData(group);

                            if (result1)
                            {
                                //then we add pokemons related
                                var data = PokemonsSelected.Select(x =>
                                            new GrupoPokemons
                                            {
                                                GroupId = group.GrupoId,
                                                Pokemon = x.name
                                            });

                                result2 = await _grupoPokemonsRepository.SaveDataRange(data);
                            }

                            if (result1 && result1)
                            {
                                await App.Current.MainPage.DisplayAlert("Success",
                                                             "Your group was created successfully", "ok");
                                await navigationService.GoBackAsync();

                            }
                            else
                            {
                                ErrorAlert();
                                await navigationService.GoBackAsync();
                            }

                        }
                        else
                        {
                            GruposRegion.GrupoName = GroupName;
                            GruposRegion.GrupoTipo = GroupType;
                            GruposRegion.PokedexDescription = PokedexDescription;

                            result1 = await _gruposRegionRepository.UpdateData(GruposRegion);

                            if (result1)
                            {
                                ////then we add pokemons related
                                //var data = PokemonsSelected.Select(x =>
                                //            new GrupoPokemons
                                //            {
                                //                GroupId = GruposRegion.GrupoId,
                                //                Pokemon = x.name
                                //            });

                                //result2 = await _grupoPokemonsRepository.SaveDataRange(data);
                            }

                            if (result1 && result1)
                            {
                                await App.Current.MainPage.DisplayAlert("Success",
                                                             "Your group was modified successfully", "ok");
                                await navigationService.GoBackAsync();

                            }
                            else
                            {
                                ErrorAlert();
                                await navigationService.GoBackAsync();
                            }
                        }
                    }
                    else
                    {
                        NoInternetAlert();
                    }



                }
                catch (Exception ex)
                {
                    ErrorAlert();
                    await navigationService.GoBackAsync();
                }



            });
            #endregion
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (parameters != null && parameters.Count > 0)
                {
                    IsCreate = (bool)parameters["IsCreate"];

                    if (IsCreate)
                    {
                        Title = "Create your group";
                    }
                    else
                    {
                        Title = "Modify";
                        GruposRegion = (GruposRegion)parameters["GruposRegion"];
                        GroupName = GruposRegion.GrupoName;
                        GroupType = GruposRegion.GrupoTipo;
                        PokedexDescription = GruposRegion.PokedexDescription;
                        Region = GruposRegion.Region;
                    }

                    if (PokedexInfo == null)
                        PokedexInfo = new List<Pokedex>((IList<Pokedex>)parameters["pokedexes"]);

                    if (CrossConnectivity.Current.IsConnected)
                    {
                        if (PokedexInfo.Count() > 0)
                        {
                            var pokemons = new List<PokemonSpecies>();

                            //add all pokemons of region selected
                            foreach (var item in PokedexInfo)
                            {
                                IsEmpty = false;

                                var url = item.url.Split('/');
                                var values = await _apiService.GetAsync<PokedexRequest>(string.Format("{0}/{1}", url[url.Length - 3], url[url.Length - 2]));
                                if (values != null)
                                {
                                    pokemons.AddRange(values.pokemon_entries.Select(x => x.pokemon_species));
                                }
                            }
                            PokemonList = new ObservableCollection<PokemonSpecies>(pokemons.OrderBy(x => x.name));
                        }
                        else
                            IsEmpty = true;
                    }
                    else
                    {
                        ErrorAlert();
                        IsEmpty = true;
                    }
                }


            });

        }

        #region Bindable Properties

        private ObservableCollection<PokemonSpecies> _pokemonList;

        public ObservableCollection<PokemonSpecies> PokemonList
        {
            get { return _pokemonList; }
            set
            {
                _pokemonList = value;
                OnPropertyChanged();
            }
        }

        private PokemonSpecies _pokemonSelected;

        public PokemonSpecies PokemonSelected
        {
            get { return _pokemonSelected; }
            set
            {
                _pokemonSelected = value;

                if (_pokemonSelected != null)
                {
                    _pokemonSelected.IsSelected = !_pokemonSelected.IsSelected;
                    if (_pokemonSelected.IsSelected)
                    {
                        if (PokemonsSelected.Count <= 5)
                            PokemonsSelected.Add(_pokemonSelected);
                    }
                    else
                        PokemonsSelected.Remove(_pokemonSelected);

                    PokemonsCounter = PokemonsSelected.Count;
                }

                OnPropertyChanged();
            }
        }

        private int _pokemonsCounter;

        public int PokemonsCounter
        {
            get { return _pokemonsCounter; }
            set
            {
                _pokemonsCounter = value;
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


        #endregion

        #region Commands

        public ICommand SaveGroup { get; private set; }
        public ICommand CancelCreation { get; private set; }
        #endregion
    }
}
