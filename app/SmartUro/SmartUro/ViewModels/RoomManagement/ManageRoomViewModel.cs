using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using CommonData.Model.DTO;
using CommonData.Model.Entity;
using SmartUro.Interfaces;
using Xamarin.Forms;

namespace SmartUro.ViewModels.RoomManagement
{
    public class ManageRoomViewModel : BaseViewModel
    {
        private readonly IDialogService _dialogService;
        private readonly IRoomService _roomService;

        private Room _room;
        private ObservableCollection<Room> _rooms;

        public Room Room {
            get => _room;
            set => OnPropertyChanged(ref _room, value);
        }
        
        public ICommand DeleteRoomCommand { get; }
        
        public ICommand EditRoomCommand { get; }


        public ObservableCollection<Room> Rooms
        {
            get => _rooms;
            set => OnPropertyChanged(ref _rooms, value);
        }

        public ManageRoomViewModel(
            IDialogService dialogService,
            IRoomService roomService
            )
        {
            _dialogService = dialogService;
            _roomService = roomService;
            DeleteRoomCommand = new Command(async () => await DeleteRoom());
            EditRoomCommand = new Command(async () => await EditRoom());
        }

        private async Task EditRoom()
        {
            // Prompt the user for the new name of the room.
            var newName = await _dialogService.PromptUserInput("Please input new room name", "Edit Room", "Save", "Cancel");

            // Construct the request to our service.
            var dto = new UpdateRoomRequestDTO((int)Room.Id!, newName);

            // Have our service execute the request.
            var result = await _roomService.UpdateRoom(dto);

            if (result.Id != null)
            {
                // Update the name of the local state Room.
                Room.Name = result.Name;
                
                // Manually trigger update.
                OnPropertyChanged(ref _room, Room, nameof(Room));
                
                // Trigger update of ObservableCollection.
                Rooms.Remove(Room);
                Rooms.Add(Room);

                // Show happy message to user.
                await _dialogService.ShowDialogAsync("The room was successfully updated", "Success!", "Ok.");
            }
            else
            {
                await _dialogService.ShowDialogAsync("The room could not be updated", "Error", "Ok.");
            }
            
            
            
            
            

        }
        
        private async Task DeleteRoom()
        {

            var shouldDelete = await _dialogService.ShowConfirmDialog("Are you sure you want to delete the room?",
                "Please confirm", "Yes", "No");

            if (shouldDelete && Room?.Id != null)
            {

                var wasSuccessful = await _roomService.DeleteRoom((int)Room.Id);

                if (wasSuccessful)
                {
                    await _dialogService.ShowDialogAsync("The room was deleted successfully!", "Success", "Ok.", async () =>
                    {
                        // Update global state.
                        Rooms.Remove(Room);
                        Room = null;
                        
                        // Navigate the user back.
                        await Application.Current.MainPage.Navigation.PopAsync();

                    });
                }
                else
                {
                    await _dialogService.ShowDialogAsync("The room could not be deleted!", "Error", "Ok.");
                }

            }

        }

    }
}