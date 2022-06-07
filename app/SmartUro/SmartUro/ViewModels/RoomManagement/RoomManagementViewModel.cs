using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommonData.Model.Entity;
using SmartUro.Views.RoomManagement;
using Xamarin.Forms;

namespace SmartUro.ViewModels.RoomManagement
{
    public class RoomManagementViewModel : BaseViewModel
    {

        public ICommand GotoManageRoomCommand { get; set; }
        public ICommand GotoCreateRoomCommand { get; set; }

        private ObservableCollection<Room> _rooms;

        public ObservableCollection<Room> Rooms
        {
            get => _rooms;
            set => OnPropertyChanged(ref _rooms, value);
        }

        /*public List<Room> Rooms { get; set; } = new List<Room>()
        {
          new Room() { Id = 1, Name = "*Room 1" },
          new Room() { Id = 1, Name = "*Room 2" },
        };*/

        private Home _currentHome;

        public Home CurrentHome
        {
            get => _currentHome;
            set => OnPropertyChanged(ref _currentHome, value);
        }
        
        public Room SelectedRoom { get; set; }

        public RoomManagementViewModel()
        {
            GotoManageRoomCommand = new Command<Room>(async (room) => await GotoManageRoom(room));
            GotoCreateRoomCommand = new Command(async () => await GotoCreateRoom());
        }

        public async Task GotoManageRoom(Room room)
        {
            var page = new ManageRoomView();
            var vm = (ManageRoomViewModel)page.BindingContext;
            vm.Room = room;
            vm.Rooms = Rooms;
            await Application.Current.MainPage.Navigation.PushAsync(page);
        }

        public async Task GotoCreateRoom()
        {
            var page = new CreateNewRoomView();
            var vm = (CreateNewRoomViewModel)page.BindingContext;
            vm.CurrentHome = CurrentHome;
            await Application.Current.MainPage.Navigation.PushAsync(page);
        }
        
    }
}
