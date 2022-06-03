using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartUro.ViewModels.HomeManagement;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartUro.Views.HomeManagement
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ManageHomeView : ContentPage
    {
        public ManageHomeView()
        {
            InitializeComponent();
            this.BindingContext = App.GetViewModel<ManageHomeViewModel>();
        }
    }
}