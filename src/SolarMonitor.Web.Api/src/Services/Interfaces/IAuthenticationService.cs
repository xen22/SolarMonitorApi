using System.Threading.Tasks;

namespace SolarMonitorApi.Services
{
    public interface IAuthenticationService
    {
        Task Authenticate(string username, string password);
    }

}
