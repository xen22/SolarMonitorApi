
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AD.Common;
using SolarMonitor.Data.Models;

namespace SolarMonitor.Data.Repositories.MySql
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private IDateTime _dateAdapter;
        private Object _criticalSection = new Object();

        public UserRepository(ApplicationDbContext dbContext, IDateTime dateAdapter)
        {
            _context = dbContext;
            _dateAdapter = dateAdapter;
        }

        public void DeleteAuthToken(Guid tokenGuid)
        {
            lock (_criticalSection)
            {
                var token = _context.AuthTokens.Where(t => t.Guid == tokenGuid).FirstOrDefault();
                if (token != null)
                {
                    _context.AuthTokens.Remove(token);
                    _context.SaveChanges();
                }
            }
        }

        public void DeleteExpiredAuthTokens()
        {
            lock (_criticalSection)
            {
                var expiredTokens = _context.AuthTokens.Where(t => t.Expiry < _dateAdapter.UtcNow);
                if (expiredTokens != null)
                {
                    foreach (var t in expiredTokens)
                    {
                        _context.AuthTokens.Remove(t);
                    }
                    _context.SaveChanges();
                }
            }
        }

        public Guid GenerateAndSaveAuthToken(string username, ulong expirySecs)
        {
            DeleteExpiredAuthTokens();

            lock (_criticalSection)
            {
                var user = _context.Users.FirstOrDefault(u => u.Username == username);

                if (user != null)
                {
                    Guid guid = Guid.NewGuid();
                    DateTime expiryDate = _dateAdapter.UtcNow + TimeSpan.FromSeconds(expirySecs);
                    var authToken = new AuthToken();
                    authToken.Guid = guid;
                    authToken.Expiry = expiryDate;
                    authToken.UserId = user.Id;
                    authToken.User = user;

                    _context.AuthTokens.Add(authToken);
                    _context.SaveChanges();
                    return guid;
                }
                else
                {
                    return Guid.Empty;
                }
            }
        }

        public User GetByUsername(string username)
        {
            return _context.Set<User>().Where(u => u.Username == username).SingleOrDefault();
        }

        public IEnumerable<string> RolesForUser(string username)
        {
            var roles = new List<string>();
            var user = _context.Set<User>()
                .Include(u => u.RoleAssignments)
                .Where(u => u.Username == username).SingleOrDefault();
            if (user?.RoleAssignments?.Count > 0)
            {
                roles = user.RoleAssignments
                    .Join(_context.Roles, ra => ra.RoleId, r => r.Id, (ra, r) => r)
                    .Select(r => r.Name).ToList();
            }
            return roles;
        }

        public bool UserExists(string username)
        {
            lock (_criticalSection)
            {
                var user = _context.Users.FirstOrDefault(u => u.Username == username);
                return (user != null);
            }
        }

        public bool ValidateAuthToken(Guid tokenGuid)
        {
            lock (_criticalSection)
            {
                var authToken = _context.AuthTokens.FirstOrDefault(t => t.Guid == tokenGuid);
                return (authToken != null) && (authToken.Expiry > _dateAdapter.UtcNow);
            }
        }
    }
}
