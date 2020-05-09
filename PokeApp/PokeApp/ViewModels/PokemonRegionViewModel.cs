using Acr.UserDialogs;
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
using Xamarin.Forms.Internals;

namespace PokeApp.ViewModels
{
    public class PokemonRegionViewModel : ViewModelBase
    {
        #region Variables

        private IAPIService _apiService;
        private INavigationService _navigationService;
        private IGruposRegionRepository _gruposRegionRepository;
        private IGrupoPokemonsRepository _grupoPokemonsRepository;
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

            #region Commands Logic

            CancelCreation = new Command(async () =>
            {
                await _navigationService.GoBackAsync();
            });

            SaveGroup = new Command(async () =>
            {
                try
                {
                    UserDialogs.Instance.ShowLoading(null, MaskType.None);


                    //validate pokemons number
                    if (PokemonsCounter < 3 || PokemonsCounter > 6)
                    {
                        UserDialogs.Instance.HideLoading();

                        await App.Current.MainPage.DisplayAlert("Error",
                                                            "You must add at min. 3 pokemons or max. 6 pokemons", "ok");
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
                                UserId = await SecureStorage.GetAsync("UserId"),
                                Token = Convert.ToBase64String(Guid.NewGuid().ToByteArray())
                        };

                            result1 = await _gruposRegionRepository.SaveData(group);

                            if (result1)
                            {
                                //then we add pokemons related
                                var data = PokemonList.Where(x => x.IsSelected).Select(x =>
                                             new GrupoPokemons
                                             {
                                                 GroupId = group.GrupoId,
                                                 Pokemon = x.name
                                             });

                                result2 = await _grupoPokemonsRepository.SaveDataRange(data);
                            }

                            if (result1 && result2)
                            {
                                await App.Current.MainPage.DisplayAlert("Success",
                                                             "Your group was created successfully", "ok");

                                var navigationParams = new NavigationParameters();
                                navigationParams.Add("RegionName", group.Region);
                                await navigationService.GoBackAsync(navigationParams);

                            }
                            else
                            {
                                UserDialogs.Instance.HideLoading();

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
                                //then we add pokemons related and delete pokemons non related

                                var oldValues = (await _grupoPokemonsRepository.GetDataByGrupoId(GruposRegion.GrupoId)).ToList();

                                var data = PokemonList.Where(x => x.IsSelected).Select(x =>
                                             new GrupoPokemons
                                             {
                                                 GroupId = GruposRegion.GrupoId,
                                                 Pokemon = x.name
                                             });

                                //if old data does not appear, it means it was no unselected
                                foreach (var item in oldValues)
                                {
                                    if (!data.Select(x => x.Pokemon).Contains(item.Pokemon))
                                        await _grupoPokemonsRepository.DeleteData(item.Id, string.Empty, string.Empty);

                                }

                                //if new data does not appear, it means it must be added

                                foreach (var item in data)
                                {
                                    if (!oldValues.Select(x => x.Pokemon).Contains(item.Pokemon))
                                    {
                                        item.Id = await _gruposRegionRepository.GetLastID(await SecureStorage.GetAsync("UserId"), string.Empty) + 1;
                                        await _grupoPokemonsRepository.SaveData(item);
                                    }
                                }

                                result2 = true;
                            }

                            if (result1 && result2)
                            {
                                UserDialogs.Instance.HideLoading();

                                await App.Current.MainPage.DisplayAlert("Success",
                                                             "Your group was modified successfully", "ok");

                                var navigationParams = new NavigationParameters();
                                navigationParams.Add("GrupoId", GruposRegion.GrupoId);
                                await navigationService.GoBackAsync(navigationParams);

                            }
                            else
                            {
                                UserDialogs.Instance.HideLoading();

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
                UserDialogs.Instance.ShowLoading(null, MaskType.Clear);

                if (parameters != null && parameters.Count > 0)
                {
                    if (PokedexInfo == null)
                        PokedexInfo = new List<Pokedex>((IList<Pokedex>)parameters["pokedexes"]);

                    if (CrossConnectivity.Current.IsConnected)
                    {
                        if (PokedexInfo.Count() > 0)
                        {
                            //Get all pokemons from region selected

                            var pokemons = new List<PokemonSpecies>();

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

                                var pokemonsAdded = (ObservableCollection<GrupoPokemons>)parameters["PokemonsAdded"];

                                //set selected pokemons added

                                foreach (var item in pokemonsAdded)
                                {
                                    var value = pokemons.Where(x => x.name.Contains(item.Pokemon)).FirstOrDefault();
                                    if (value != null)
                                        value.IsSelected = true;

                                }

                                PokemonsCounter = pokemonsAdded.Count();
                            }


                            PokemonList = new ObservableCollection<PokemonSpecies>();

                            //avoid repeated data
                            foreach (var item in pokemons.OrderBy(x => x.name))
                            {
                                if (!PokemonList.Any(x => x.name.Contains(item.name)))
                                    PokemonList.Add(item);
                            }

                            UserDialogs.Instance.HideLoading();

                        }
                        else
                            IsEmpty = true;
                        UserDialogs.Instance.HideLoading();

                    }
                    else
                    {
                        UserDialogs.Instance.HideLoading();

                        ErrorAlert();
                        IsEmpty = true;
                    }

                }
                UserDialogs.Instance.HideLoading();


            });

        }

        #region Bindable Properties

        private ObservableCollection<PokemonSpecies> _pokemonList;

        public ObservableCollection<PokemonSpecies> PokemonList
        {
            get { return _pokemonList; }
            set
            {
                SetProperty(ref _pokemonList, value);

            }
        }

        private PokemonSpecies _pokemonSelected;

        public PokemonSpecies PokemonSelected
        {
            get { return _pokemonSelected; }
            set
            {
                SetProperty(ref _pokemonSelected, value);
                if (_pokemonSelected != null)
                {
                    PokemonList.ForEach(x => x.IsSelected = x.name == value.name ? !_pokemonSelected.IsSelected : x.IsSelected);
                    PokemonsCounter = PokemonList.Where(x => x.IsSelected).Count();
                }
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
