using Acr.UserDialogs;
using Plugin.Connectivity;
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
    public class RegionGruposViewModel : ViewModelBase
    {
        #region Variables

        private INavigationService _navigationService;
        private IGruposRegionRepository _gruposRegionRepository;
        private IGrupoPokemonsRepository _grupoPokemonsRepository;

        #endregion

        public RegionGruposViewModel(INavigationService navigationService, IGruposRegionRepository gruposRegionRepository,
            IGrupoPokemonsRepository grupoPokemonsRepository)
        : base(navigationService)
        {
            _navigationService = navigationService;
            _gruposRegionRepository = gruposRegionRepository;
            _grupoPokemonsRepository = grupoPokemonsRepository;

            #region Commands Logic

            CreateGroup = new Command(async (obj) =>
            {
                var navigationParams = new NavigationParameters();
                navigationParams.Add("pokedexes", PokedexInfo);
                navigationParams.Add("IsCreate", true);
                await _navigationService.NavigateAsync("PokemonRegionView", navigationParams);
            });

            PasteToken = new Command(async (obj) =>
            {
                var navigationParams = new NavigationParameters();
                navigationParams.Add("regionName", Title);
                await _navigationService.NavigateAsync("AddGroupCopiedView", navigationParams);
            });

            CopyGroup = new Command(async (obj) =>
            {
                if (obj != null)
                {
                    var token = (string)obj;
                    await Clipboard.SetTextAsync(token);
                    Vibration.Vibrate(10);
                    UserDialogs.Instance.Toast("Group's token copied to clipboard.", new TimeSpan(0, 0, 6));

                }
                else
                    UserDialogs.Instance.Toast("There is a problem to get group's token.", new TimeSpan(0, 0, 6));
               
            });

            #endregion
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                App.IsMainView = false;

                UserDialogs.Instance.ShowLoading(null, MaskType.Clear);

                //when comes from deleting a group
                if (parameters != null && parameters.Count != 0 && parameters.ContainsKey("DeletedGrupoId"))
                {
                    GruposRegionList.Remove(GruposRegionList.Where(x => x.GrupoId == (int)parameters["DeletedGrupoId"]).FirstOrDefault());
                }
                else if ((parameters != null && parameters.Count != 0) || !string.IsNullOrEmpty(Title)) //whe comes from another view
                {
                    if ((PokedexInfo == null && parameters.ContainsKey("RegionName")) || parameters.ContainsKey("RegionName"))
                        Title = (string)parameters["RegionName"];

                    if ((PokedexInfo == null && parameters.ContainsKey("pokedexes")) || parameters.ContainsKey("pokedexes"))
                        PokedexInfo = new List<Pokedex>((IList<Pokedex>)parameters["pokedexes"]);

                    var GroupsCreated = await _gruposRegionRepository.GetAllDataByName(Title, await SecureStorage.GetAsync("UserId"));
                    if (GroupsCreated.Count() > 0)
                    {
                        IsEmpty = false;
                        GruposRegionList = new ObservableCollection<GruposRegion>(GroupsCreated);
                        UserDialogs.Instance.HideLoading();
                    }
                    else
                        IsEmpty = true;
                    UserDialogs.Instance.HideLoading();

                }

                UserDialogs.Instance.HideLoading();

            });

        }


        #region Bindable Properties

        private ObservableCollection<GruposRegion> _gruposRegionList;

        public ObservableCollection<GruposRegion> GruposRegionList
        {
            get { return _gruposRegionList; }
            set
            {
                _gruposRegionList = value;
                OnPropertyChanged();
            }
        }

        private GruposRegion grupoRegionSelected;

        public GruposRegion GrupoRegionSelected
        {
            get { return grupoRegionSelected; }
            set
            {
                grupoRegionSelected = value;
                if (value != null)
                    GoToView(value);
                OnPropertyChanged();

            }
        }


        #endregion

        #region Methods

        public void GoToView(GruposRegion values)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var navigationParams = new NavigationParameters();
                navigationParams.Add("GrupoId", values.GrupoId);
                await _navigationService.NavigateAsync("GrupoDetailsView", navigationParams);

            });

        }

        #endregion

        #region Commands

        public ICommand CreateGroup { get; private set; }
        public ICommand CopyGroup { get; private set; }
        public ICommand PasteToken { get; private set; }

        #endregion
    }
}
