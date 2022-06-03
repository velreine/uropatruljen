using System.Windows.Input;
using SmartUro.Interfaces;
using SmartUro.Views;
using Xamarin.Essentials;
using Xamarin.Forms;


namespace SmartUro.ViewModels
{
    internal class SelectUserWiFiViewModel : BaseViewModel
    {

        private const string DefaultUnconnected = "<Disconnected>";
        
        private string _wiFiPasswordInput;
        private Color _wifiColorIndicator;
        private string _connectedWiFiName = DefaultUnconnected;

        public bool IsDeviceConnectedToWiFi { get; set; }

        public string ConnectedWiFiName
        {
            get => _connectedWiFiName;
            set => OnPropertyChanged(ref _connectedWiFiName, value);
        }

        public string WiFiPasswordInput
        {
            get => _wiFiPasswordInput;
            set => OnPropertyChanged(ref _wiFiPasswordInput, value);
        }
        
        public ICommand OpenSettingsCommand { get; set; }
        public ICommand ContinueCommand { get; set; }

        public Color WifiColorIndicator
        {
            get => _wifiColorIndicator;
            set => OnPropertyChanged(ref _wifiColorIndicator, value);
        }

        public SelectUserWiFiViewModel(IWiFiObserver _wiFiObserver)
        {
            this._wifiColorIndicator = Color.Blue;            
            
            if (_wiFiObserver != null)
            {
                var status = _wiFiObserver.IsWifiCurrentlyConnected();
                this.ConnectedWiFiName = _wiFiObserver.GetCurrentSSID() ?? DefaultUnconnected;

                if (status == null)
                {
                    this._wifiColorIndicator = Color.Blue;
                } 
                else if (status == true)
                {
                    this._wifiColorIndicator = Color.LightGreen;
                }
                else
                {
                    this._wifiColorIndicator = Color.Red;
                }

                _wiFiObserver.OnDeviceWiFiConnected += (sender, args) =>
                {
                    this.WifiColorIndicator = Color.LightGreen;
                    this.ConnectedWiFiName = _wiFiObserver.GetCurrentSSID();
                    this.IsDeviceConnectedToWiFi = true;
                };

                _wiFiObserver.OnDeviceWiFiDisconnected += (sender, args) =>
                {
                    this.WifiColorIndicator = Color.Red;
                    this.ConnectedWiFiName = DefaultUnconnected;
                    this.IsDeviceConnectedToWiFi = false;
                };
            }
            
            this.OpenSettingsCommand = new Command(async () =>
            {
                // Don't know if this is iOS specific.
                await Launcher.OpenAsync("app-settings:");
            });

            this.ContinueCommand = new Command<string>(async (wifiPasswordInput) =>
            {
                var page = new AddUroView();
                var pageContext = page.BindingContext as AddUroViewModel;
                // ReSharper disable once PossibleNullReferenceException
                pageContext.WifiPasswordFromPreviousStep = wifiPasswordInput;
                
                await Application.Current.MainPage.Navigation.PushAsync(page);
            });
        }        
    }
}