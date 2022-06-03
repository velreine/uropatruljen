using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommonData.Model.Entity;
using SmartUro.Views.HomeManagement;
using Xamarin.Forms;

namespace SmartUro.ViewModels.HomeManagement
{
    public class HomeManagementViewModel : BaseViewModel
    {
        public ICommand GotoManageHomeCommand { get; }
        public ICommand GotoCreateHomeCommand { get; }
        public ICommand GotoJoinHomeCommand { get; }

        public Home SelectedHome { get; set; }
        
        private ObservableCollection<Home> _homes;
        
        public ObservableCollection<Home> Homes
        {
            get => _homes;
            set => OnPropertyChanged(ref _homes, value);
        }
        
        public HomeManagementViewModel()
        {

            GotoManageHomeCommand = new Command<Home>(async (home) => await GotoManageHome(home));
            GotoCreateHomeCommand = new Command(async () => await GotoCreateHome());
            GotoJoinHomeCommand = new Command(async () => await GotoJoinHome());
        }

        public async Task GotoManageHome(Home home)
        {
            Debug.WriteLine("GotoManageHome invoked...");

            var page = new ManageHomeView();
            page.BindingContext = new ManageHomeViewModel(home);
            await Application.Current.MainPage.Navigation.PushAsync(page);
        }

        public async Task GotoCreateHome()
        {
            var page = new CreateNewHome();
            await Application.Current.MainPage.Navigation.PushAsync(page);
        }

        public async Task GotoJoinHome()
        {
            var page = new JoinHomeView();
            await Application.Current.MainPage.Navigation.PushAsync(page);
        }

    }
}
