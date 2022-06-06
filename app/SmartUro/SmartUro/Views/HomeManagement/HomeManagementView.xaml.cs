using SmartUro.ViewModels.HomeManagement;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartUro.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomeManagementView : ContentPage
    {
        public HomeManagementView()
        {
            InitializeComponent();
            this.BindingContext = App.GetViewModel<HomeManagementViewModel>();
        }
    }
}