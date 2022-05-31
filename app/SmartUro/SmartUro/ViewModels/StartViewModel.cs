using System;
using System.Collections.Generic;
//using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using SmartUro.Services;
using SmartUro.Views;
using System.Runtime.CompilerServices;
using CommonData.Model.Entity;
using CommonData.Model.Static;
using CommonData.Model;
using System.Diagnostics;
using System.Threading;
using MQTTnet;
using MQTTnet.Client;
using SmartUro.Views.AddUroFlow;

namespace SmartUro.ViewModels
{
    internal class StartViewModel : BaseViewModel
    {
        public ICollection<HardwareLayout> HardwareLayouts { get; set; }

        public ICommand Navigate { get; }
        public ICommand BeginAddUroFlowCommand { get; }


        private MqttClient _mqttClient;

        public StartViewModel()
        {
            HardwareLayouts = new List<HardwareLayout>();
            GetListOfUros();

            // Configure MQTT client options.
            var mqttClientOptions = new MqttClientOptionsBuilder()
                .WithTcpServer("mqtt.uroapp.dk")
                .Build();

            var mqttFactory = new MqttFactory();
            var client = mqttFactory.CreateMqttClient();

            // Register event handlers before doing the actual connection..
            client.ConnectedAsync += (eventArgs) =>
            {
                Debug.WriteLine("The client has successfully connected to the server.");
                //eventArgs.DumpToConsole();
                return Task.CompletedTask;
            };

            client.DisconnectedAsync += (eventArgs) =>
            {
                var reason = Enum.GetName(typeof(MqttClientDisconnectReason), eventArgs.Reason);


                Debug.WriteLine($"The client has disconnected, Reason: {reason}");
                Debug.WriteLine(eventArgs.Exception.Message);

                // Keep trying to connect to the server in intervals of 5 seconds.
                /*while (client.IsConnected != true)
                {
                    client.ConnectAsync(mqttClientOptions);
                    Thread.Sleep(5000);
                }*/

                return Task.CompletedTask;
            };

            client.ConnectingAsync += (eventArgs) =>
            {
                Debug.WriteLine("The client is connecting...");
                //eventArgs.DumpToConsole();
                return Task.CompletedTask;
            };

            client.ApplicationMessageReceivedAsync += (eventArgs) =>
            {
                // TODO: Implement handlers...
                Debug.WriteLine("The client received an application message.");
                //eventArgs.DumpToConsole();
                return Task.CompletedTask;
            };
            
            client.ConnectAsync(mqttClientOptions, CancellationToken.None);

            Navigate = new Command<HardwareLayout>(async hw => await NavigateToUroView(hw));
            BeginAddUroFlowCommand = new Command(async () => await NavigateToSelectUserWifi());
        }

        private async Task NavigateToSelectUserWifi()
        {
            var page = new SelectUserWiFi();
            await Application.Current.MainPage.Navigation.PushAsync(page);
        }

        private async Task NavigateToUroView(HardwareLayout hw)
        {
            var page = new UroView();
            page.BindingContext = new UroViewModel(hw);
            await Application.Current.MainPage.Navigation.PushAsync(page);
        }


        private void GetListOfUros()
        {
            //Dummy data for testing
            HardwareLayout hw1 = new HardwareLayout()
            {
                Id = 1,
                ProductName = "Smart Uro V1",
                ModelNumber = "ABC-123-123",
                AttachedComponents = new List<Component>()
                {
                    {
                        new Component()
                        {
                            Id = 1,
                            Name = "RGB_DIODE_1",
                            Type = ComponentType.RgbDiode,
                            Pins = new List<Pin>()
                            {
                                {
                                    new Pin()
                                    {
                                        Id = 1, Direction = PinDirection.Input, Descriptor = "r", HwPinNumber = 10
                                    }
                                },
                                {
                                    new Pin()
                                    {
                                        Id = 2, Direction = PinDirection.Input, Descriptor = "g", HwPinNumber = 11
                                    }
                                },
                                {
                                    new Pin()
                                    {
                                        Id = 3, Direction = PinDirection.Input, Descriptor = "b", HwPinNumber = 12
                                    }
                                },
                            }
                        }
                    },
                    {
                        new Component()
                        {
                            Id = 2,
                            Name = "RGB_DIODE_2",
                            Type = ComponentType.RgbDiode,
                            Pins = new List<Pin>()
                            {
                                {
                                    new Pin()
                                    {
                                        Id = 1, Direction = PinDirection.Input, Descriptor = "r", HwPinNumber = 13
                                    }
                                },
                                {
                                    new Pin()
                                    {
                                        Id = 2, Direction = PinDirection.Input, Descriptor = "g", HwPinNumber = 14
                                    }
                                },
                                {
                                    new Pin()
                                    {
                                        Id = 3, Direction = PinDirection.Input, Descriptor = "b", HwPinNumber = 15
                                    }
                                },
                            }
                        }
                    }
                }
            };
            HardwareLayout hw2 = new HardwareLayout()
            {
                Id = 1,
                ProductName = "Smart Uro V2",
                ModelNumber = "EDF-666-999",
                AttachedComponents = new List<Component>()
                {
                    {
                        new Component()
                        {
                            Id = 1,
                            Name = "DIODE_1",
                            Type = ComponentType.Diode,
                            Pins = new List<Pin>()
                            {
                                {
                                    new Pin()
                                    {
                                        Id = 1, Direction = PinDirection.Input, Descriptor = "d", HwPinNumber = 16
                                    }
                                }
                            }
                        }
                    }
                }
            };
            HardwareLayouts.Add(hw1);
            HardwareLayouts.Add(hw2);
        }
    }
}