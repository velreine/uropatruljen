using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartUro.Interfaces
{
    internal interface IRestService
    {
        Task ToggleStateAsync(int state);
    }
}
