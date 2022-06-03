using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MQTTnet.Client;

namespace SmartUro.Interfaces
{
    public interface IMqttService
    {
        Task SendRequest();
        
        MqttClient Client { get; }
    }
}
