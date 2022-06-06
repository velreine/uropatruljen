using System;
using System.Threading.Tasks;

namespace SmartUro.Interfaces
{
    public interface IDialogService
    {
        Task ShowDialogAsync(string message, string title, string buttonText);
        Task ShowDialogAsync(string message, string title, string buttonText, Action callbackAfterHide);

        Task<bool> ShowConfirmDialog(string message, string title, string confirmText = "yes",
            string cancelText = "no");
    }
}