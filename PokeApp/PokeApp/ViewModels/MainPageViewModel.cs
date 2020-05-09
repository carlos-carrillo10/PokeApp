using Acr.UserDialogs;
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
using System.Windows.Input;
using Xamarin.Essentials;
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


            Logout = new Command(async () =>
            {
                SecureStorage.RemoveAll();
                App.IsLogged = false;
                await NavigationService.NavigateAsync("/LoginView");
            });
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
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
                OnPropertyChanged(nameof(RegionsList));
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
                OnPropertyChanged(nameof(RegionSelected));
            }
        }


        #endregion

        #region Methods

        public void GetRegions()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                UserDialogs.Instance.ShowLoading(null, MaskType.Clear);

                if (CrossConnectivity.Current.IsConnected)
                {

                    if (RegionsList != null && RegionsList.Count > 0)
                        RegionsList.Clear();

                    var values = await _apiService.GetAsync<RegionRequest>(Constants.Regions);
                    if (values != null)
                    {
                        RegionsList = new ObservableCollection<Models.Regions.Region>(values.results);
                        IsEmpty = false;
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
                    navigationParams.Add("RegionName", values.name);
                    await _navigationService.NavigateAsync("RegionGruposView", navigationParams);
                }
                else
                    ErrorAlert();
              
            });
           
        }

        #endregion

        #region Commands

        public ICommand Logout { get; private set; }

        #endregion
    }
}
