namespace SmartUro.ViewModels
{
    internal class AddUroViewModel : BaseViewModel
    {
        private string _wifiPasswordFromPreviousStep;
        private string _serialNumberInput;

        public string WifiPasswordFromPreviousStep
        {
            get => _wifiPasswordFromPreviousStep;
            set => OnPropertyChanged(ref _wifiPasswordFromPreviousStep, value);
        }

        public string SerialNumberInput
        {
            get => _serialNumberInput;
            set => OnPropertyChanged(ref _serialNumberInput, value);
        }
    }
}