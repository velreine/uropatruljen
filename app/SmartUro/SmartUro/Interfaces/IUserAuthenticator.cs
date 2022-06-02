using System.Threading.Tasks;
using CommonData.Model.Entity;

namespace SmartUro.Interfaces
{
    public interface IUserAuthenticator
    {
        Task<bool> Login(string email, string plainTextPassword);

        Task Logout();

        /// <summary>
        /// Gets the current authenticated user, or NULL if no user is authenticated.
        /// </summary>
        /// <returns>Person or null if no user is authenticated.</returns>
        Task<Person> GetAuthenticatedUser();
    }
}