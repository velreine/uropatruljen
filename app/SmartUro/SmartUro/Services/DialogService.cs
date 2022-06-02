using System;
using System.Threading.Tasks;
using SmartUro.Interfaces;
using Xamarin.Forms;

namespace SmartUro.Services
{
    public class DialogService : IDialogService
    {
        public async Task ShowDialogAsync(string message, string title, string buttonText)
        {
            await Application.Current.MainPage.DisplayAlert(title, message, buttonText);
        }

        public async Task ShowDialogAsync(string message, string title, string buttonText, Action callbackAfterHide)
        {
            await Application.Current.MainPage.DisplayAlert(title, message, buttonText);
            callbackAfterHide?.Invoke();
        }
    }
}