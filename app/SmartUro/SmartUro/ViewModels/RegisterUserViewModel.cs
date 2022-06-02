using System;
using System.Threading.Tasks;
using System.Windows.Input;
using CommonData.Model.Entity;
using Xamarin.Forms;

namespace SmartUro.ViewModels
{
    public class RegisterUserViewModel
    {
        public string DesiredName { get; set; }
        public string DesiredEmail { get; set; }
        public string DesiredPassword { get; set; }

        public string ConfirmDesiredPassword { get; set; }
        
        public ICommand RegisterUserCommand { get; set; }

        public bool IsFormInputValid => DoCheckFormIsValid();
        
        public RegisterUserViewModel()
        {
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
        }
        
        
        
        
        
    }
}