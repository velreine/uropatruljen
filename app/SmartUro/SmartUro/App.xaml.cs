using System;
using SmartUro.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SmartUro.Views;
using Microsoft.Extensions.DependencyInjection;
using SmartUro.Interfaces;
using SmartUro.ViewModels;
using CommonData.Model.Entity;

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

            // Add core services
            services.AddSingleton<IMqttService, MqttService>();
            services.AddSingleton<IRestService, WebAPIService>();

            AppServiceProvider = services.BuildServiceProvider();
        }

        public static BaseViewModel GetViewModel<TViewModel>()
            where TViewModel : BaseViewModel
            => ServiceProviderServiceExtensions.GetService<TViewModel>(AppServiceProvider);
    }
}
