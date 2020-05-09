using Acr.UserDialogs;
using Plugin.Connectivity;
using PokeApp.Services.Interfaces;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;


namespace PokeApp.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        #region Variables

        private INavigationService _navigationService;

        #endregion

        public LoginViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            _navigationService = navigationService;

#if DEBUG
            Email = "test@pokeapp.com";
            Password = "Test123!";
#endif

            #region Commands logic

            Login = new Command(async () =>
            {
                UserDialogs.Instance.ShowLoading(null, MaskType.Clear);
                if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
                {
                    UserDialogs.Instance.HideLoading();
                    await App.Current.MainPage.DisplayAlert("Error",
                                                      "Email y Password son requeridos", "ok");
                    return;
                }

                if (!CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.HideLoading();
                    NoInternetAlert();
                    return;
                }

                if (await MakeLogin())
                    await _navigationService.NavigateAsync("NavigationPage/MainPage");

            });

            #endregion
        }

        #region Bindable Properties

        private string _email;

        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        private string _password;

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }


        #endregion

        #region Methods

        private async Task<bool> MakeLogin()
        {
            try
            {
                //save to secure storage
                var service = DependencyService.Get<IFirebaseAuth>();
                var result = await service.LoginWithEmailPassword(Email, Password);

                if (result != null)
                {
                    await SecureStorage.SetAsync("Token", result["Token"]);
                    await SecureStorage.SetAsync("UserId", result["UserId"]);
                    UserDialogs.Instance.HideLoading();

                    return true;
                }
                else 
                {
                    UserDialogs.Instance.HideLoading();

                    await App.Current.MainPage.DisplayAlert("Error",
                                                     "Email or Password is wrong", "ok");
                    return false;
                }
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                ErrorAlert();
                return false;
            }

        }

        #endregion

        #region Commands

        public ICommand Login { get; private set; }

        #endregion
    }
}
