using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using CommonData.Model.DTO;
using CommonData.Model.Entity;
using SmartUro.Interfaces;
using Xamarin.Forms;

namespace SmartUro.ViewModels.HomeManagement
{
    public class EditHomeViewModel : BaseViewModel
    {
        private readonly IHomeService _homeService;
        private readonly IDialogService _dialogService;

        private Home _home;

        private string _newHomeName;
        private ObservableCollection<Home> _userHomes;
        public ICommand EditHomeCommand { get; }
        
        public Home Home { 
            get => _home;
            set => OnPropertyChanged(ref _home, value);
        }

        public ObservableCollection<Home> UserHomes
        {
            get => _userHomes;
            set => OnPropertyChanged(ref _userHomes, value);
        }

        public EditHomeViewModel(IHomeService homeService, IDialogService dialogService)
        {
            _homeService = homeService;
            _dialogService = dialogService;
            EditHomeCommand = new Command(() => EditHome());
        }
        
        public async Task EditHome()
        {

            // Indicate that work is being done...
            IsBusy = true;
            
            if (Home.Id != null)
            {
                var result = await _homeService.UpdateHome(new UpdateHomeRequestDTO((int)Home.Id, Home.Name));

                if (result.Id != null && result.Name != null)
                {
                    // Update the global state to match the newly updated home.
                    Home.Name = result.Name;
                    
                    // Trigger update of UserHomes. (Lovely...)
                    UserHomes.Remove(Home);
                    UserHomes.Add(Home);
                    
                    // Inform the user that the home was updated.
                    await _dialogService.ShowDialogAsync("The home was updated!", "Home Updated", "Ok.", async () =>
                    {
                        // Navigate the user back. 
                        // TODO: This is a dirty fix, because the app uses global state.
                        // Have not figured out how to update the "parents" object yet.
                        await Application.Current.MainPage.Navigation.PopAsync(false);
                        await Application.Current.MainPage.Navigation.PopAsync(false);
                    });
                }
                else
                {
                    await _dialogService.ShowDialogAsync("The home could not be updated!", "Error", "Ok.");
                }
                
            }
            // Indicate that work is done.
            IsBusy = false;
        }
    }
}