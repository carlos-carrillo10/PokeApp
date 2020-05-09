using Acr.UserDialogs;
using PokeApp.FireBaseRepository.Interfaces;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PokeApp.ViewModels
{
    public class AddGroupCopiedViewModel : ViewModelBase
    {
        #region Variables

        private INavigationService _navigationService;
        private IGruposRegionRepository _gruposRegionRepository;

        public static string Region { get; set; }

        #endregion

        public AddGroupCopiedViewModel(INavigationService navigationService, IGruposRegionRepository gruposRegionRepository)
           : base(navigationService)
        {
            _navigationService = navigationService;
            _gruposRegionRepository = gruposRegionRepository;

            #region MyRegion

            SendToken = new Command(async() =>
            {
                UserDialogs.Instance.ShowLoading(null, MaskType.Clear);

                if (string.IsNullOrEmpty(Token))
                {
                    UserDialogs.Instance.HideLoading();
                    await App.Current.MainPage.DisplayAlert("Error",
                                                    "You must add a group's token", "ok");
                    return;
                }


                if (!(await _gruposRegionRepository.ItExist(Token, await SecureStorage.GetAsync("UserId"), Region)))
                {
                    UserDialogs.Instance.HideLoading();
                    await App.Current.MainPage.DisplayAlert("Error",
                                                    "You can't add this group. The Group's owner could have removed it", "ok");
                    return;
                }

                if (await _gruposRegionRepository.IsMyGroup(Token, await SecureStorage.GetAsync("UserId"), Region))
                {
                    UserDialogs.Instance.HideLoading();
                    await App.Current.MainPage.DisplayAlert("Error",
                                                    "You can't add a group that belongs you", "ok");
                    return;
                }

                if (!(await _gruposRegionRepository.IsSameRegion(Token, await SecureStorage.GetAsync("UserId"), Region)))
                {
                    UserDialogs.Instance.HideLoading();
                    await App.Current.MainPage.DisplayAlert("Error",
                                                    "You can't add a group that belongs to another region", "ok");
                    return;
                }

                

                var result = await _gruposRegionRepository.SaveGroupByToken(Token, await SecureStorage.GetAsync("UserId"), Region);

                if (result)
                {
                    UserDialogs.Instance.HideLoading();
                    await App.Current.MainPage.DisplayAlert("Success",
                                                           "This group was created successfully", "ok");

                    var navigationParams = new NavigationParameters();
                    navigationParams.Add("regionName", Region);
                    await _navigationService.GoBackAsync(navigationParams);
                }
                else
                {
                    UserDialogs.Instance.HideLoading();
                    return;
                }

            });
            #endregion
        }


        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            Region = (string)parameters["regionName"];
        }



        #region Bindable Properties

        private string _token;

        public string Token
        {
            get { return _token; }
            set { SetProperty(ref _token, value); }
        }


        #endregion

        #region Commands

        public ICommand SendToken { get; set; }

        #endregion

    }
}
