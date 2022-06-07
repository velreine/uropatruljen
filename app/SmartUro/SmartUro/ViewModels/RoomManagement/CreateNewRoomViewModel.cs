using System.Threading.Tasks;
using System.Windows.Input;
using CommonData.Model.DTO;
using CommonData.Model.Entity;
using SmartUro.Interfaces;
using Xamarin.Forms;

namespace SmartUro.ViewModels.RoomManagement
{
    public class CreateNewRoomViewModel : BaseViewModel
    {
        private readonly IRoomService _roomService;
        private readonly IDialogService _dialogService;

        private Home _currentHome;

        public Home CurrentHome
        {
            get => _currentHome;
            set => OnPropertyChanged(ref _currentHome, value);
        }

        private string _roomName;

        public string RoomName
        {
            get => _roomName;
            set => OnPropertyChanged(ref _roomName, value);
        }
        
        public ICommand CreateRoomCommand { get; }
        
        public CreateNewRoomViewModel(IRoomService roomService, IDialogService dialogService)
        {
            _roomService = roomService;
            _dialogService = dialogService;
            CreateRoomCommand = new Command(async () => await CreateRoom());
        }

        public async Task CreateRoom()
        {
            
            // Indicate that work is being done...
            IsBusy = true;

            // Create the request to the API/Service.
            var requestData = new CreateRoomRequestDTO(RoomName, (int)CurrentHome.Id!);
            
            // Fetch the response.
            var result = await _roomService.CreateRoom(requestData);

            if (result.Id != null)
            {
                // Construct the domain-model object from the response.
            var room = new Room(result.Id, result.Name, result.HomeId);
            
            // Add it to the current home.
            CurrentHome.AddRoom(room);
            
            // Inform the user of the success.
            await _dialogService.ShowDialogAsync("The room was created!", "Success!", "Ok.", async () =>
            {
                await Application.Current.MainPage.Navigation.PopAsync(true);
            });
            
            }
            else
            {
                await _dialogService.ShowDialogAsync("The room could not be created!", "Error", "Ok.");
            }
            
            
            // Indicate that work is done.
            IsBusy = false;
        }
        
    }
}