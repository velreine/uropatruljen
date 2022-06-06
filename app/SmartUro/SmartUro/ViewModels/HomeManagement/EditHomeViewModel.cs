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

        private Home _home;

        private string _newHomeName; 
        public ICommand EditHomeCommand { get; }
        
        public Home Home { get => _home;
            set => OnPropertyChanged(ref _home, value);
        }

        public EditHomeViewModel(IHomeService homeService)
        {
            _homeService = homeService;
            EditHomeCommand = new Command(() => EditHome());
        }
        
        public async Task EditHome()
        {

            // Indicate that work is being done...
            IsBusy = true;
            if (Home.Id != null)
            {
                var result = await _homeService.UpdateHome(new UpdateHomeRequestDTO((int)Home.Id, Home.Name));
            }
            // Indicate that work is done.
            IsBusy = false;
        }
    }
}