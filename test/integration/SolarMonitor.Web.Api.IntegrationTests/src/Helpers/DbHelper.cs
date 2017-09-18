using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SolarMonitor.Data.Models;
using CommonTypes = SolarMonitor.Data.CommonTypes;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using Microsoft.EntityFrameworkCore;

namespace SolarMonitor.Api.IntegrationTests
{
    public class DbHelper
    {
        private Data.Repositories.MySql.ApplicationDbContext _dbContext;
        private readonly SensorFactory _sensorFactory;

        public DbHelper(Data.Repositories.MySql.ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _sensorFactory = new SensorFactory();
            RecreateDatabase();
            SeedMainData();
        }

        public void RecreateDatabase()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();
        }

        public void SeedMainData()
        {
            _dbContext.Users.Add(new User { Id = 1, Name = "demo", Username = "demo" });
            _dbContext.SaveChanges();
        }

        public List<Site> GenerateSites(int count)
        {
            var sites = new List<Site>();
            for (int i = 1; i <= count; i++)
            {
                var site = new Site
                {
                    Id = i,
                    Guid = Guid.NewGuid(),
                    Name = $"Site{i}",
                    Timezone = $"Timezone{i}",
                };
                _dbContext.Sites.Add(site);
                sites.Add(site);
            }
            _dbContext.SaveChanges();
            return sites;
        }

        internal object GetSensor(Guid guid)
        {
            return _dbContext.Set<Sensor>().Where(s => s.Guid == guid).SingleOrDefault();
        }

        public Site GetSite(int id)
        {
            return _dbContext.Set<Site>().Where(s => s.Id == id).SingleOrDefault();
        }

        public Site GetSite(Guid guid)
        {
            return _dbContext.Set<Site>().Where(s => s.Guid == guid).SingleOrDefault();
        }

        public int NumSensors()
        {
            return _dbContext.Set<Sensor>().Count();
        }

        public Dictionary<CommonTypes.SensorType, SensorType> GenerateSensorTypes()
        {
            var sensorTypes = new Dictionary<CommonTypes.SensorType, SensorType>();
            var availableSensorTypes = Enum.GetValues(typeof(CommonTypes.SensorType)).Cast<CommonTypes.SensorType>();
            foreach (var sensorType in availableSensorTypes)
            {
                if (sensorType != CommonTypes.SensorType.Unset)
                {
                    var st = new SensorType
                    {
                        Id = (int)sensorType,
                        Name = sensorType.ToString()
                    };
                    sensorTypes[sensorType] = st;
                    _dbContext.SensorTypes.Add(st);
                }
            }
            return sensorTypes;
        }

        public Dictionary<CommonTypes.SensorType, SensorType> GetExistingSensorTypes()
        {
            var sensorTypes = new Dictionary<CommonTypes.SensorType, SensorType>();

            foreach (var entry in _dbContext.SensorTypes)
            {
                sensorTypes[(CommonTypes.SensorType)entry.Id] = entry;
            }
            return sensorTypes;
        }

        // Object DeepClone(Object obj)
        // {
        //     var str = new MemoryStream();
        //     var formatter = new BinaryFormatter();
        //     formatter.Context = new StreamingContext(StreamingContextStates.Clone);
        //     formatter.Serialize(str, obj);

        //     // reset stream
        //     str.Position = 0;
        //     return (formatter.Deserialize(str));
        // }

        public List<Sensor> GenerateSensors(
            int numSites,
            int numSensorsPerSite,
            int startingSensorId = 1,
            CommonTypes.SensorType sensorType = CommonTypes.SensorType.TemperatureSensor,
            bool createSites = true,
            bool createSensorTypes = true)
        {
            var sensors = new List<Sensor>();

            var sensorTypes = createSensorTypes ? GenerateSensorTypes() : GetExistingSensorTypes();

            for (int i = 1; i <= numSites; i++)
            {
                Site site = null;
                if (createSites)
                {
                    // create site
                    site = new Site
                    {
                        Id = i,
                        Guid = Guid.NewGuid(),
                        Name = $"Site{i}",
                        Timezone = $"Timezone{i}"
                    };
                    _dbContext.Sites.Add(site);
                }
                else
                {
                    site = _dbContext.Sites.Where(s => s.Id == i).SingleOrDefault();
                }

                for (int j = startingSensorId; j < startingSensorId + numSensorsPerSite; j++)
                {
                    int id = i * 100 + j;
                    var s = _sensorFactory.CreateSensor(sensorType, id, site, sensorTypes[sensorType]);
                    //var newSensor = (Sensor)DeepClone(s);
                    sensors.Add(s);
                    _dbContext.Sensors.Add(s);

                }
            }

            _dbContext.SaveChanges();
            return sensors;
        }
    }
}
