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
        public async Task<bool> ShowConfirmDialog(string message, string title, string confirmText = "yes",
            string cancelText = "no")
        {
            return await Application.Current.MainPage.DisplayAlert(title, message, confirmText, cancelText);
        }

        public async Task<string> PromptUserInput(string message, string title, string acceptText, string cancelText)
        {
            return await Application.Current.MainPage.DisplayPromptAsync(title, message, acceptText,cancelText);
        }

    }
}