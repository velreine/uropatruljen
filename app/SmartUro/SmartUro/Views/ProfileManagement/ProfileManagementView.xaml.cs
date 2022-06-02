using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartUro.ViewModels.ProfileManagement;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartUro.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfileManagementView : ContentPage
    {
        public ProfileManagementView()
        {
            InitializeComponent();
            this.BindingContext = App.GetViewModel<ProfileManagementViewModel>();
        }
    }
}