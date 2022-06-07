using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommonData.Model.Entity;
using SmartUro.Interfaces;
using SmartUro.Services;
using SmartUro.Views.HomeManagement;
using Xamarin.Forms;

namespace SmartUro.ViewModels.HomeManagement
{
    public class ManageHomeViewModel : BaseViewModel
    {
        private Home _home;
        private readonly IHomeService _homeService;
        private readonly IDialogService _dialogService;
        private ObservableCollection<Home> _homes;
        private const string DeleteHomeConfirmationMessage = "Are you sure you want to delete the home?";
        private const string DeleteHomeConfirmationTitle = "Delete Home";
        private const string DeleteHomeConfirmButtonText = "Yes";
        private const string DeleteHomeCancelButtonText = "No";

        public ICommand DeleteHomeCommand { get; }
        
        public ICommand GoToEditHomeCommand { get; }


        public ManageHomeViewModel(IHomeService homeService, IDialogService dialogService)
        {
            _homeService = homeService;
            _dialogService = dialogService;
            GoToEditHomeCommand = new Command(async () => await GoToEditHome());
            DeleteHomeCommand = new Command(async () => await DeleteHome());
        }

        public Home Home
        {
            get => _home; 
            set => OnPropertyChanged(ref _home, value);
        }
        public ObservableCollection<Home> Homes { 
            get => _homes;
            set => OnPropertyChanged(ref _homes, value);
        }

        public async Task GoToEditHome()
        {
            Debug.WriteLine("GotEditHomeView invoked");

            var page = new EditHome();
            var viewModel = (EditHomeViewModel)page.BindingContext;
            viewModel.Home = Home;
            await Application.Current.MainPage.Navigation.PushAsync(page);
        }

        public async Task DeleteHome()
        {
            // Indicate that work is being done...
            IsBusy = true;
            var shouldDelete = await _dialogService.ShowConfirmDialog(DeleteHomeConfirmationMessage, DeleteHomeConfirmationTitle,
                DeleteHomeConfirmButtonText, DeleteHomeCancelButtonText);
            if (Home.Id != null && shouldDelete)
            {
                if (await _homeService.DeleteHome((int)Home.Id))
                {
                    Homes.Remove(Home); // Remove hove form global state.
                }
                else
                {
                    await _dialogService.ShowDialogAsync("Could not delete home", DeleteHomeConfirmationTitle, "Close");
                }
            }
            // Indicate that work is done.
            IsBusy = false;
        }

    }
}
