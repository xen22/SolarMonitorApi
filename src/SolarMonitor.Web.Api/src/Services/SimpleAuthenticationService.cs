using System;
using System.Text;
using System.Threading.Tasks;
using SolarMonitor.Data.Repositories;
using SolarMonitorApi.Exceptions;

namespace SolarMonitorApi.Services
{
    internal class SimpleAuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;

        public SimpleAuthenticationService(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public Task Authenticate(string username, string password)
        {
            var user = _userRepository.GetByUsername(username);
            //var encodedPassword = Convert.ToBase64String(Encoding.Unicode.GetBytes(password));
            var encodedPassword = password;

            if (encodedPassword == user.Password)
            {
                return Task.FromResult(0);
            }

            // TODO: we need to move the credentials into a secrets file 
            // if ("demo".Equals(username, StringComparison.OrdinalIgnoreCase) &&
            //     "demo".Equals(password))
            // {
            //     return Task.FromResult(0);
            // }

            throw new InvalidCredentialsException();
        }
    }
}
