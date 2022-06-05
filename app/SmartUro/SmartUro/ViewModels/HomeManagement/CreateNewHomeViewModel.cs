using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using CommonData.Model.DTO;
using CommonData.Model.Entity;
using SmartUro.Interfaces;
using SmartUro.Services;
using Xamarin.Forms;

namespace SmartUro.ViewModels.HomeManagement
{
    public class CreateNewHomeViewModel : BaseViewModel
    {
        private readonly IHomeService _homeService;
        private readonly IUserAuthenticator _userAuthenticator;

        public ObservableCollection<Home> UserHomes { get; set; }

        public ICommand CreateHomeCommand { get; }

        public string HomeName { get; set; }

        public CreateNewHomeViewModel(IHomeService homeService, IUserAuthenticator userAuthenticator)
        {
            _homeService = homeService;
            _userAuthenticator = userAuthenticator;
            CreateHomeCommand = new Command(() => CreateHome());
        }

        public async Task CreateHome()
        {

            // Indicate that work is being done...
            IsBusy = true;
            
            var result = await _homeService.CreateHome(new CreateHomeRequestDTO(HomeName));
            
            var currentUser = await _userAuthenticator.GetAuthenticatedUser();

            if (currentUser != null)
            {
                var newHome = new Home()
                {
                    Id = result.Id,
                    Name = result.Name
                };
                
                currentUser.AddHome(newHome);
            }
            
            // Indicate that work is done.
            IsBusy = false;
        }
    }
}
