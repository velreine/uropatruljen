using System;
using SmartUro.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SmartUro.Views;
using Microsoft.Extensions.DependencyInjection;
using SmartUro.Interfaces;
using SmartUro.ViewModels;
using CommonData.Model.Entity;
using RestSharp;
using RestSharp.Authenticators;
using SmartUro.ViewModels.HomeManagement;
using SmartUro.ViewModels.ProfileManagement;
using SmartUro.ViewModels.RoomManagement;

[assembly: ExportFont("fa6brands-regular.otf", Alias = "FA6BRANDSREGULAR")]
[assembly: ExportFont("fa6regular.otf", Alias = "FA6REGULAR")]
[assembly: ExportFont("fa6solid.otf", Alias = "FA6SOLID")]
namespace SmartUro
{
    public partial class App : Application
    {
        protected static IServiceProvider AppServiceProvider { get; set; }

        public App(Action<IServiceCollection> addPlatformServices = null)
        {
            InitializeComponent();

            SetupServices(addPlatformServices);

            MainPage = new NavigationPage(new LoginView());
            //MainPage = new ColorPickerView();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        void SetupServices(Action<IServiceCollection> addPlatformServices = null)
        {
            var services = new ServiceCollection();

            // Add platform specific services
            addPlatformServices?.Invoke(services);

            // Add ViewModels , (Maybe this could be done using reflection, so we don't need to remember to add view models here.)
            services.AddTransient<SelectUserWiFiViewModel>();
            services.AddTransient<StartViewModel>();
            services.AddTransient<UroViewModel>();
            services.AddTransient<LoginViewModel>();
            services.AddTransient<RegisterUserViewModel>();
            services.AddTransient<ProfileManagementViewModel>();
            services.AddTransient<RoomManagementViewModel>();
            services.AddTransient<HomeManagementViewModel>();
            services.AddTransient<ColorPickerViewModel>();
            services.AddTransient<CreateNewHomeViewModel>();
            services.AddTransient<CreateNewRoomViewModel>();
            services.AddTransient<ManageRoomViewModel>();
            services.AddTransient<EditHomeViewModel>();
            services.AddTransient<ManageHomeViewModel>();
            services.AddTransient<ColorPickerViewModel>();

            // Add core services
            services.AddSingleton<IMqttService, MqttService>();
            
            // Configure RestSharp client.
            services.AddSingleton(provider =>
            {
                var baseurl = new Uri("https://uroapp.dk");
                var options = new RestClientOptions(baseurl)
                {
                    RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
                };
                var restClient = new RestClient(options);
                return restClient;
            });

            // Configure Dialog service for displaying errors, messages etc.
            services.AddSingleton<IDialogService, DialogService>();
            
            // Configure Authentication services.
            services.AddSingleton<AuthenticationService>();
            services.AddSingleton(provider =>
                (IUserAuthenticator)provider.GetService(typeof(AuthenticationService)));
            services.AddSingleton(provider =>
                (IUserRegistrator)provider.GetService(typeof(AuthenticationService)));
            
            // Configure data fetching/manipulating services.
            services.AddSingleton<IDeviceService, DeviceService>();
            services.AddTransient<IHomeService, HomeService>();
            services.AddTransient<IHardwareLayoutService, HardwareLayoutService>();
            services.AddTransient<IRoomService, RoomService>();


            AppServiceProvider = services.BuildServiceProvider();
        }

        public static BaseViewModel GetViewModel<TViewModel>()
            where TViewModel : BaseViewModel
            => ServiceProviderServiceExtensions.GetService<TViewModel>(AppServiceProvider);
    }
}