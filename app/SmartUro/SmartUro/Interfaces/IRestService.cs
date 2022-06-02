using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartUro.Interfaces
{
    public interface IRestService
    {
        Task ToggleState(int state);

        Task<bool> VerifyLogin(string email, string pass);

        Task<ICollection> GetPairedUros();
    }
}
