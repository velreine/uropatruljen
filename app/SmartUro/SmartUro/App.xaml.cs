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
using SmartUro.ViewModels.ProfileManagement;

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

            // Add ViewModels
            services.AddTransient<SelectUserWiFiViewModel>();
            services.AddTransient<StartViewModel>();
            services.AddTransient<UroViewModel>();
            services.AddTransient<LoginViewModel>();
            services.AddTransient<RegisterUserViewModel>();
            services.AddTransient<ProfileManagementViewModel>();

            // Add core services
            services.AddSingleton<IMqttService, MqttService>();
            
            // Configure RestSharp client.
            services.AddSingleton(provider =>
            {
                return new RestClient()
                {
                    Options =
                    {
                        RemoteCertificateValidationCallback = (sender, certificate, chain, errors) => true,
                        //BaseUrl = new Uri("https://api.uroapp.dk")
                        BaseUrl = new Uri("https://uroapp.dk")
                    }
                };
                
            });

            // Configure Dialog service for displaying errors, messages etc.
            services.AddSingleton<IDialogService, DialogService>();
            
            // Configure Authentication services.
            services.AddSingleton<AuthenticationService>();
            services.AddSingleton(provider =>
                (IUserAuthenticator)provider.GetService(typeof(AuthenticationService)));
            services.AddSingleton(provider =>
                (IUserRegistrator)provider.GetService(typeof(AuthenticationService)));
            
            // Configure data fetching services.
            services.AddSingleton<IDeviceService, DeviceService>();


            AppServiceProvider = services.BuildServiceProvider();
        }

        public static BaseViewModel GetViewModel<TViewModel>()
            where TViewModel : BaseViewModel
            => ServiceProviderServiceExtensions.GetService<TViewModel>(AppServiceProvider);
    }
}