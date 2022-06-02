using SmartUro.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartUro.Views.AddUroFlow
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectUserWiFi : ContentPage
    {
        public SelectUserWiFi()
        {
            InitializeComponent();
            BindingContext = App.GetViewModel<SelectUserWiFiViewModel>();
        }
    }
}