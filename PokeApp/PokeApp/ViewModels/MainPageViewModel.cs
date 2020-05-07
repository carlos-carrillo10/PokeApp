using Plugin.Connectivity;
using PokeApp.FireBaseRepository.Interfaces;
using PokeApp.Models;
using PokeApp.Models.FirebaseDatabase;
using PokeApp.Models.Regions;
using PokeApp.Services;
using PokeApp.Services.Interfaces;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace PokeApp.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        #region Variables

        private IAPIService _apiService;
        private INavigationService _navigationService;
        private IGruposRegionRepository _gruposRegionRepository;


        #endregion

        public MainPageViewModel(INavigationService navigationService, IAPIService apiService, IGruposRegionRepository gruposRegionRepository)
            : base(navigationService)
        {
            Title = "Regions";
            _apiService = apiService;
            _navigationService = navigationService;
            _gruposRegionRepository = gruposRegionRepository;
            GetRegions();
        }

        #region Bindable Properties

        private ObservableCollection<Models.Regions.Region> _regionsList;

        public ObservableCollection<Models.Regions.Region> RegionsList
        {
            get { return _regionsList; }
            set
            {
                _regionsList = value;
                OnPropertyChanged();
            }
        }

        private Models.Regions.Region _regionSelected;

        public Models.Regions.Region RegionSelected
        {
            get { return _regionSelected; }
            set { _regionSelected = value;
                if (value != null)
                {
                    GoToView(value);
                    RegionSelected = null;
                }
                OnPropertyChanged();
            }
        }


        #endregion

        #region Methods

        public void GetRegions()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    var values = await _apiService.GetAsync<RegionRequest>(Constants.Regions);
                    if (values != null)
                    {
                        RegionsList = new ObservableCollection<Models.Regions.Region>(values.results);
                        IsEmpty = false;
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

        public void GoToView(Models.Regions.Region value)
        {
            Device.BeginInvokeOnMainThread(async()=>
            {
                var url = value.url.Split('/');
                if (CrossConnectivity.Current.IsConnected)
                {
                    var values = await _apiService.GetAsync<RegionInfo>(string.Format("{0}/{1}", url[url.Length-3], url[url.Length - 2]));
             
                    var navigationParams = new NavigationParameters();
                    navigationParams.Add("pokedexes", values.pokedexes);
                    await _navigationService.NavigateAsync("NavigationPage/PokemonRegionView", navigationParams);
                }
                else
                    ErrorAlert();
              
            });
           
        }
        #endregion
    }
}
