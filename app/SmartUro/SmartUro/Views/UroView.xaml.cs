using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SmartUro.ViewModels;

namespace SmartUro.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UroView : ContentPage
    {
        public UroView()
        {
            InitializeComponent();
            this.BindingContext = App.GetViewModel<UroViewModel>();
        }
    }
}