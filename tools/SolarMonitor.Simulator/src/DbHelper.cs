using System;
using SolarMonitor.Data.Repositories.MySql;
using SolarMonitor.Data.Models;

namespace SolarMonitor.Simulator
{
    public class DbHelper
    {
        ApplicationDbContext _dbContext;

        public DbHelper()
        {

        }

        public void ReCreateDb()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();
        }

        public void SeedMainData()
        {
            AddUsers();

            AddBaseData();

            AddDevices();
            AddSensors();
        }

        private void AddUsers()
        {
            var users = new User[] {
                new User { Id = 1, Username = "user1"},
                new User { Id = 2, Username = "user2"},
                new User { Id = 3, Username = "demo"},
            };
            foreach (var u in users)
            {
                _dbContext.Users.Add(u);
            }
            _dbContext.SaveChanges();
        }
        private void AddBaseData()
        {
            var sites = new Site[]
            {
                new Site { Id = 1, Name = "Lake Ohau", Timezone = "NZST" },
                new Site { Id = 21, Name = "Test system", Timezone = "NZST" },
                new Site { Id = 32, Name = "Simulation 1", Timezone = "NZST" },
            };
            foreach (var s in sites)
            {
                _dbContext.Sites.Add(s);
            }



            _dbContext.SaveChanges();
        }

        private void AddSensors()
        {
            throw new NotImplementedException();
        }

        private void AddDevices()
        {
            throw new NotImplementedException();
        }

    }
}