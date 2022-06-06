using System;
using System.Threading.Tasks;
using System.Windows.Input;
using CommonData.Model.DTO;
using CommonData.Model.Entity;
using SmartUro.Interfaces;
using SmartUro.Views;
using Xamarin.Forms;

// ReSharper disable IdentifierTypo

namespace SmartUro.ViewModels
{
    public class RegisterUserViewModel : BaseViewModel
    {
        private readonly IUserRegistrator _userRegistrator;
        private readonly IDialogService _dialogService;
        private readonly IUserAuthenticator _userAuthenticator;


        private string _desiredName;
        private string _desiredEmail;
        private string _desiredPassword;
        private string _confirmDesiredPassword;

        public string DesiredName
        {
            get => _desiredName;
            set
            {
                OnPropertyChanged(ref _desiredName, value);
                CheckFormIsValid();
            }
        }

        public string DesiredEmail
        {
            get => _desiredEmail;
            set { 
                OnPropertyChanged(ref _desiredEmail, value);
                CheckFormIsValid();
            }
        }

        public string DesiredPassword
        {
            get => _desiredPassword;
            set { 
                OnPropertyChanged(ref _desiredPassword, value);
                CheckFormIsValid();
            }
        }

        public string ConfirmDesiredPassword
        {
            get => _confirmDesiredPassword;
            set { 
                OnPropertyChanged(ref _confirmDesiredPassword, value);
                CheckFormIsValid();
            }
        }

        public ICommand RegisterUserCommand { get; }

        
        private bool _isFormValid = false;
        
        public bool IsFormValid
        {
            get => _isFormValid;
            set => OnPropertyChanged(ref _isFormValid, value);
        }


        public RegisterUserViewModel(IUserRegistrator userRegistrator, IDialogService dialogService, IUserAuthenticator userAuthenticator)
        {
            _userRegistrator = userRegistrator;
            _dialogService = dialogService;
            _userAuthenticator = userAuthenticator;
            RegisterUserCommand = new Command(async () => await RegisterUser());
        }

        private void CheckFormIsValid()
        {
            if (string.IsNullOrEmpty(DesiredEmail) ||
                string.IsNullOrEmpty(DesiredPassword) ||
                string.IsNullOrEmpty(ConfirmDesiredPassword)
               )
            {
                // The fields should not be null.
                IsFormValid = false;
                return;
            }

            if (DesiredPassword != ConfirmDesiredPassword)
            {
                IsFormValid = false;
                return;
            }

            // If all checks pass, the form is considered valid.
            IsFormValid = true;
        }

        public async Task RegisterUser()
        {
            IsBusy = true;
            
            // Create the data for the service.
            var requestData = new RegisterRequestDTO(DesiredName, DesiredEmail, DesiredPassword);

            // Ask the service to do its thing.
            var registrationResult = await _userRegistrator.RegisterUserAsync(requestData);

            // If the response is null, then registration failed.
            if (registrationResult == null)
            {
                await _dialogService.ShowDialogAsync("User registration failed", "Error", "Ok..");
                return;
            }

            // Otherwise, it was successful!
            await _dialogService.ShowDialogAsync("User registration success!", "Success", "Ok..", async () =>
            {
                // Since the user was successfully registered, login automatically.
                await _userAuthenticator.Login(DesiredEmail, DesiredPassword);
                
                // Then redirect to the start page.
                var page = new StartView();
                await Application.Current.MainPage.Navigation.PushAsync(page);
            });
            
            IsBusy = false;
        }
    }
}