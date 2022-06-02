using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartUro.Interfaces
{
    public interface IMqttService
    {
        Task SendRequest();
    }
}
