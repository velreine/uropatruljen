using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using CommonData.Model.Entity;
using SmartUro.Interfaces;
using Xamarin.Forms;

namespace SmartUro.ViewModels.ProfileManagement
{
    public class ProfileManagementViewModel : BaseViewModel
    {
        private readonly IUserAuthenticator _userAuthenticator;

        private Person _currentAuthenticatedUser = null;
        
        public Person CurrentAuthenticatedUser { get => _currentAuthenticatedUser;
            set => OnPropertyChanged(ref _currentAuthenticatedUser, value);
        }

        private bool _isUserLoaded = false;

        public bool IsUserLoaded
        {
            get => _isUserLoaded;
            set
            {
                //_isUserLoaded = value;
                OnPropertyChanged(ref _isUserLoaded, value);
                OnPropertyChanged(ref _showLoader, !value);
            }
        }

        private bool _showLoader = true;

        public bool ShowLoader
        {
            get => _showLoader;
            set => OnPropertyChanged(ref _showLoader, value);
        }

        public ICommand LogoutCommand { get; set; }

        public ProfileManagementViewModel(IUserAuthenticator userAuthenticator)
        {
            _userAuthenticator = userAuthenticator;
            LogoutCommand = new Command(async () => await Logout());
            
            LoadUser();
        }

        private async void LoadUser()
        {
            CurrentAuthenticatedUser = await _userAuthenticator.GetAuthenticatedUser();
            IsUserLoaded = true;
            ShowLoader = false;
        }
        
        private async Task Logout()
        {
            // Logout user from app.
            await _userAuthenticator.Logout();

            // Pop back to login page.
            await Application.Current.MainPage.Navigation.PopToRootAsync(true);
        }
        
    }
}
