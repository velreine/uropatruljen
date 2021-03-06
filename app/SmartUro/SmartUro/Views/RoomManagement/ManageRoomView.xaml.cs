using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartUro.ViewModels.RoomManagement;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartUro.Views.RoomManagement
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ManageRoomView : ContentPage
    {
        public ManageRoomView()
        {
            InitializeComponent();
            BindingContext = App.GetViewModel<ManageRoomViewModel>();
        }
    }
}