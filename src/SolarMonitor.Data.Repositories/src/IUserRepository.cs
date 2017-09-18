using System;
using System.Collections.Generic;
using SolarMonitor.Data.Models;

namespace SolarMonitor.Data.Repositories
{
    public interface IUserRepository
    {
        bool UserExists(string username);
        bool ValidateAuthToken(Guid token);
        Guid GenerateAndSaveAuthToken(string username, ulong expirySecs);
        void DeleteAuthToken(Guid tokenGuid);
        void DeleteExpiredAuthTokens();

        User GetByUsername(string username);
        IEnumerable<string> RolesForUser(string username);
    }
}