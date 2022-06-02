using System.Threading.Tasks;
using CommonData.Model.Entity;

namespace SmartUro.Interfaces
{
    public interface IUserAuthenticator
    {
        Task<bool> Login(string email, string plainTextPassword);
    }
}