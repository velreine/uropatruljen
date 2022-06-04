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
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        private bool _isBusy;

        // Indicates if the ViewModel is busy, (fetching data, performing some other operation.)
        public bool IsBusy
        {
            get => _isBusy;
            set => OnPropertyChanged(ref _isBusy, value);
        }
        
        protected BaseViewModel()
        {
        }

        protected virtual void OnPropertyChanged<T>(ref T property, T val, [CallerMemberName] string propertyName = null)
        {
            property = val;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
