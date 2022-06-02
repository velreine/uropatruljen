using System;
using System.Collections.Generic;
using System.Text;

namespace SmartUro.Interfaces
{
    public interface IAppMessage
    {
        void LongAlert(string message);
        void ShortAlert(string message);
    }
}
