using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartUro.Interfaces
{
    public interface IRestService<T>
    {
        Task ToggleState(int state);

        Task<bool> VerifyLogin(string email, string pass);

        Task<ICollection<T>> GetPairedUros();
    }
}
