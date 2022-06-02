using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommonData.Model.Entity;
using Xamarin.Forms;

namespace SmartUro.ViewModels.ProfileManagement
{
    public class ProfileManagementViewModel
    {
        public Person CurrentAuthenticatedUser { get; set; }
        
        public ICommand LogoutCommand { get; set; }

        public ProfileManagementViewModel()
        {
            LogoutCommand = new Command(async () => await Logout());
        }

        public async Task Logout()
        {
            // TODO: Logout the user from the app,
            // Destroy token.
            
        }
        
    }
}
