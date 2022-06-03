using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartUro.ViewModels.HomeManagement
{
    public class CreateNewHomeViewModel : BaseViewModel
    {

        public ICommand CreateHomeCommand { get; set; }

        public string HomeName { get; set; }

        public CreateNewHomeViewModel()
        {
            CreateHomeCommand = new Command(async () => await CreateHome());
        }

        public Task CreateHome()
        {
            // 1. Invoke HomeService,HomeManager or what-ever, and have that service/manager call the cloud api to add the home.
            // 2. Then return to HomeManagementView (with an updated list showing this new home).
            return Task.CompletedTask;
        }
    }
}
