using System;
using System.Threading.Tasks;
using System.Windows.Input;
using CommonData.Model.Entity;
using SmartUro.Interfaces;
using Xamarin.Forms;
// ReSharper disable IdentifierTypo

namespace SmartUro.ViewModels
{
    public class RegisterUserViewModel
    {
        private readonly IUserRegistrator _userRegistrator;
        private readonly IDialogService _dialogService;
        public string DesiredName { get; set; }
        public string DesiredEmail { get; set; }
        public string DesiredPassword { get; set; }
        
        public string ConfirmDesiredPassword { get; set; }
        
        public ICommand RegisterUserCommand { get; set; }

        public bool IsFormInputValid => DoCheckFormIsValid();
        
        
        public RegisterUserViewModel(IUserRegistrator userRegistrator, IDialogService dialogService)
        {
            _userRegistrator = userRegistrator;
            _dialogService = dialogService;
            RegisterUserCommand = new Command(async () => await RegisterUser());
        }

        private bool DoCheckFormIsValid()
        {

            if (string.IsNullOrEmpty(DesiredEmail) ||
                string.IsNullOrEmpty(DesiredPassword) ||
                string.IsNullOrEmpty(ConfirmDesiredPassword)
                )
            {
                // The fields should not be null.
                return false;
            }

            if (DesiredPassword != ConfirmDesiredPassword)
            {
                return false;
            }

            return true;
        }

        public async Task RegisterUser()
        {
            // TODO: Actually register the user here.

            var person = new Person()
            {
                Name = DesiredName,
                Email = DesiredEmail
            };

            var plainTextPassword = DesiredPassword;

            var registrationResult = await _userRegistrator.RegisterUserAsync(person, plainTextPassword);

            if (registrationResult == false)
            {
                _dialogService.ShowDialogAsync("User registration failed", "Error", "Ok..");
                return;
            }

            _dialogService.ShowDialogAsync("User registration success!", "Success", "Ok..", () =>
            {
                // Update global state ....
            });


        }
        
        
        
        
        
    }
}