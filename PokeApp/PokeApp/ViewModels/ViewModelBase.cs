using PokeApp.Models.FirebaseDatabase;
using PokeApp.Models.Regions;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace PokeApp.ViewModels
{
    public class ViewModelBase : BindableBase, IInitialize, INavigationAware, IDestructible
    {
        protected INavigationService NavigationService { get; private set; }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public ViewModelBase(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        public virtual void Initialize(INavigationParameters parameters)
        {

        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {

        }

        public virtual void Destroy()
        {

        }

        #region Variables

        public static List<Pokedex> PokedexInfo;
        public static int GroupId { get; set; }
        public static GruposRegion GruposRegion { get; set; }

        #endregion

        #region Bindable Properties

        private bool _isLoading;

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }

        private bool _isEmpty;

        public bool IsEmpty
        {
            get { return _isEmpty; }
            set
            {
                _isEmpty = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Methods

        public async void NoInternetAlert()
        {
            await App.Current.MainPage.DisplayAlert("No hay Conexión a Internet",
                                                      "There is a problem with you internet connection. Please, try again", "ok");
        }

        public async void ErrorAlert()
        {
            await App.Current.MainPage.DisplayAlert("Error",
                                                     "There is a problem to process this action. Please, try again", "ok");
        }

        #endregion

        #region Commands



        #endregion

    }
}
