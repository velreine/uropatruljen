using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using SmartUro.Interfaces;
using SmartUro.Services;
using CommonData.Model.Entity;

namespace SmartUro.ViewModels
{
    internal abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected IRestService<HardwareLayout> RestService;

        public BaseViewModel()
        {
            RestService = new WebAPIService();
        }

        protected virtual void OnPropertyChanged<T>(ref T property, T val, [CallerMemberName] string propertyName = null)
        {
            property = val;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
