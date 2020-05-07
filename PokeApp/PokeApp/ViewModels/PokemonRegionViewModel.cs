using Plugin.Connectivity;
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
using Xamarin.Forms;

namespace PokeApp.ViewModels
{
    public class PokemonRegionViewModel : ViewModelBase
    {
        #region Variables

        private IAPIService _apiService;
        private INavigationService _navigationService;
        private static List<PokemonSpecies> PokemonsSelected;
        #endregion

        public PokemonRegionViewModel(INavigationService navigationService, IAPIService apiService)
            : base(navigationService)
        {
            Title = "Select your pokemons";
            _apiService = apiService;
            _navigationService = navigationService;

            PokemonsSelected = new List<PokemonSpecies>();
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var pokedexes = (IList<Pokedex>)parameters["pokedexes"];
                if (CrossConnectivity.Current.IsConnected)
                {
                    if (pokedexes.Count() > 0)
                    {
                        var pokemons = new List<PokemonSpecies>();

                        //add all pokemons of region selected
                        foreach (var item in pokedexes)
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
                        PokemonsSelected.Add(_pokemonSelected);
                    else
                        PokemonsSelected.Remove(_pokemonSelected);

                }

                OnPropertyChanged();
            }
        }



        #endregion
    }
}
