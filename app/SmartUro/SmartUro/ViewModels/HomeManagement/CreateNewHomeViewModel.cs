using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using CommonData.Model.DTO;
using SmartUro.Interfaces;
using SmartUro.Services;
using Xamarin.Forms;

namespace SmartUro.ViewModels.HomeManagement
{
    public class CreateNewHomeViewModel : BaseViewModel
    {
        private readonly IHomeService _homeService;
        
        public ICommand CreateHomeCommand { get; }

        public string HomeName { get; set; }

        public CreateNewHomeViewModel(IHomeService homeService)
        {
            _homeService = homeService;
            CreateHomeCommand = new Command(() => CreateHome());
        }

        public async Task CreateHome()
        {

            // Mark the viewmodel as busy...
            IsBusy = true;
            Thread.Sleep(2500);
            //var result = await _homeService.CreateHome(new CreateHomeRequestDTO(HomeName));
            IsBusy = false;
        }
    }
}
