using SmartUro.Interfaces;
using SmartUro.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartUro.ViewModels
{
    internal class LoginViewModel : BaseViewModel
    {
        private string _emailInput;
        private string _passwordInput;
        private string _loginError;

        public string EmailInput 
        { 
            get => _emailInput; 
            set 
            { 
                OnPropertyChanged(ref _emailInput, value); 
                LoginCommand.ChangeCanExecute(); 
            } 
        }
        public string PasswordInput 
        { 
            get => _passwordInput; 
            set 
            { 
                OnPropertyChanged(ref _passwordInput, value); 
                LoginCommand.ChangeCanExecute(); 
            } 
        }
        public string LoginError 
        {
            get => _loginError;
            set => OnPropertyChanged(ref _loginError, value);
        }
        public bool LoginAllowed => !string.IsNullOrEmpty(EmailInput) && !string.IsNullOrEmpty(PasswordInput);

        public Command LoginCommand { get; }
        public ICommand BeginAddUserCommand { get; }

        public LoginViewModel()
        {
            LoginError = "";
            LoginCommand = new Command(async () => await Login(), () => LoginAllowed);
            BeginAddUserCommand = new Command(async () => await NavigateToAddUserView());
        }

        private async Task Login()
        {
            if (await RestService.VerifyLogin(EmailInput, PasswordInput))
            {
                LoginError = "";
                EmailInput = "";
                PasswordInput = "";
                await NavigateToStartView();
            }
            else
                LoginError = "Wrong e-mail or password!";
        }

        private async Task NavigateToStartView()
        {
            var page = new StartView();
            await Application.Current.MainPage.Navigation.PushAsync(page);
        }

        private async Task NavigateToAddUserView()
        {
            var page = new AddUserView();
            await Application.Current.MainPage.Navigation.PushAsync(page);
        }
    }
}
