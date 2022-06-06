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
    public partial class EditHome : ContentPage
    {
        public EditHome()
        {
            InitializeComponent();
            BindingContext = App.GetViewModel<EditHomeViewModel>();
        }
    }
}