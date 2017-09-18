using Microsoft.EntityFrameworkCore;
using SolarMonitor.Data.Models;

namespace SolarMonitor.Data.Repositories.MySql
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
          : base(options)
        {
        }

        public DbSet<Device> Devices { get; set; }
        public DbSet<Sensor> Sensors { get; set; }
        public DbSet<Measurement> Measurements { get; set; }


        //#region solar components
        public DbSet<Site> Sites { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleAssignment> RoleAssignments { get; set; }
        public DbSet<AuthToken> AuthTokens { get; set; }
        // public DbSet<SolarSystem> SolarSystems { get; set; }
        public DbSet<LoadType> LoadTypes { get; set; }
        public DbSet<SensorType> SensorTypes { get; set; }
        public DbSet<DeviceType> DeviceTypes { get; set; }
        public DbSet<BatteryBank> BatteryBanks { get; set; }
        public DbSet<ChargeController> ChargeControllers { get; set; }
        public DbSet<Inverter> Inverters { get; set; }

        public DbSet<Shunt> Shunts { get; set; }
        public DbSet<CurrentSensor> CurrentSensors { get; set; }
        public DbSet<BatterySocSensor> BatterySocSensors { get; set; }
        public DbSet<BatteryChargeStageSensor> BatteryChargeStageSensors { get; set; }
        public DbSet<BatteryStatsSensor> BatteryStatsSensors { get; set; }
        public DbSet<ChargeControllerLoadOutput> ChargeControllerLoadOutputs { get; set; }
        public DbSet<EnergyStatsSensor> EnergyStatsSensors { get; set; }
        // #endregion

        // #region solar measurements
        public DbSet<ShuntMeasurement> ShuntMeasurements { get; set; }
        public DbSet<CurrentSensorMeasurement> CurrentSensorMeasurements { get; set; }
        public DbSet<BatterySocMeasurement> BatterySocMeasurements { get; set; }
        public DbSet<BatteryChargeStageMeasurement> BatteryChargeStageMeasurements { get; set; }
        public DbSet<BatteryStatsMeasurement> BatteryStatsMeasurements { get; set; }
        public DbSet<ChargeControllerLoadOutputMeasurement> ChargeControllerLoadOutputMeasurements { get; set; }
        public DbSet<EnergyStatsMeasurement> EnergyStatsMeasurements { get; set; }
        // #endregion

        // #region weather components
        // public DbSet<WeatherBase> WeatherBases { get; set; }
        public DbSet<HumiditySensor> HumiditySensors { get; set; }
        public DbSet<TemperatureSensor> TemperatureSensors { get; set; }
        public DbSet<BarometricPressureSensor> BarometricPressureSensors { get; set; }
        public DbSet<WindSensor> WindSensors { get; set; }
        public DbSet<WeatherStation> WeatherStations { get; set; }
        // #endregion

        // #region weather measurements
        public DbSet<HumidityMeasurement> HumidityMeasurements { get; set; }
        public DbSet<TemperatureMeasurement> TemperatureMeasurements { get; set; }
        public DbSet<BarometricPressureMeasurement> BarometricPressureMeasurements { get; set; }
        public DbSet<WindMeasurement> WindMeasurements { get; set; }
        // #endregion

        struct FieldLengths
        {
            public static int Short = 45;
            public static int Long = 255;
            public static int Timezone = 10;

            // Format is "<n>x<m>", n parallel strings of m panels, n, m <= 99
            public static int PanelConfiguration = 5;
            public static int BatteryConfiguration = 5;
            public static int LoadType = 2;
            public static int DeviceType = 45;
            public static int SensorType = 45;
        };
        //FieldLengths fieldLengths;

        ModelBuilder ConfigureStandardFields<EntityType>(ModelBuilder builder) where EntityType : class, IDetailedDescriptor
        {
            builder.Entity<EntityType>(b =>
            {
                //b.Property(s => s.Id).IsRequired().ValueGeneratedOnAdd();
                b.Property(s => s.Name).HasMaxLength(FieldLengths.Short);
                b.Property(s => s.Description).HasMaxLength(FieldLengths.Long);
                b.Property(s => s.Model).HasMaxLength(FieldLengths.Short);
                b.Property(s => s.Manufacturer).HasMaxLength(FieldLengths.Short);
                //b.Property(s => s.DetailedSpecs).HasMaxLength(); // max length
            }
            );

            return builder;
        }

        ModelBuilder ConfigureSensor<SensorType>(ModelBuilder builder) where SensorType : class, ISensor, IDetailedDescriptor
        {
            builder.Entity<SensorType>(b =>
            {
                //          b.Property(s => s.Guid).HasColumnType("BINARY(16)").HasMaxLength(16).HasDefaultValue((new System.Guid()).ToByteArray());
                //          b.Property(s => s.Guid).HasDefaultValue(System.Guid.NewGuid());
                // b.HasBaseType("Sensor");
                b.Property(s => s.Guid).IsRequired();
                b.HasIndex(s => s.Guid).IsUnique();
                //b.Property(s => s.Device).IsRequired(false).HasDefaultValueSql("NULL");

                // this would be nice but MySQL does not support expressions such as "uuid()" as a DEFAULT value for a field
                // (we have to use triggers instead, in SQL scripts)
                //b.Property(s => s.Guid).IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql("uuid()");

            }
            );
            ConfigureStandardFields<SensorType>(builder);

            return builder;
        }

        ModelBuilder ConfigureDevice<DeviceType>(ModelBuilder builder) where DeviceType : class, IDevice, IDetailedDescriptor
        {
            builder.Entity<DeviceType>(b =>
            {
                b.Property(s => s.Id).IsRequired().ValueGeneratedOnAdd();
                b.Property(s => s.Guid).IsRequired();
                b.HasIndex(s => s.Guid).IsUnique();
            }
            );
            ConfigureStandardFields<DeviceType>(builder);

            return builder;
        }


        ModelBuilder ConfigureMeasurement<EntityType>(ModelBuilder builder) where EntityType : class, IMeasurement
        {
            builder.Entity<EntityType>().HasIndex(s => s.Timestamp);
            builder.Entity<EntityType>(b =>
            {
                b.Property(s => s.Id).IsRequired().ValueGeneratedOnAdd();
                // Note: Autogenerating DATETIME values is not supported in MySql v. 5.5, which is the current version on RPi
                // (MySql v. 5.6 supports it, so when this becomes available on Debian Jessie/Raspbian, we can re-enable this line)
                //b.Property(s => s.Timestamp).IsRequired().ValueGeneratedOnAdd();  // < Note, this works with MySql 5.7.16
                b.Property(s => s.Timestamp).HasColumnType("DATETIME").IsRequired();
            }
            );

            return builder;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // By default the Pomelo db provider for MySQL uses longtext for .NET strings, which is inefficient.
            // Ideally we would set up a generic conversion of string to nvarchar(45) as shown below, however
            // this EF6 generic convention configuration is not supported in EF Core (as of v. 1.1):
            // builder.Properties<string>()
            //   .Configure(s => s.HasMaxLength(45));

            // set up conversion rules manually:
            builder.Entity<Site>(b =>
            {
                b.Property(s => s.Name).HasMaxLength(FieldLengths.Short);
                b.Property(s => s.Timezone).HasMaxLength(FieldLengths.Timezone);
            }
            );

            builder.Entity<User>(b =>
            {
                b.Property(s => s.Name).HasMaxLength(FieldLengths.Short);
                b.Property(s => s.Username).HasMaxLength(FieldLengths.Short);
            }
            );

            builder.Entity<LoadType>(b =>
            {
                b.Property(s => s.Type).HasMaxLength(FieldLengths.LoadType);
            }
            );

            builder.Entity<SensorType>(b =>
            {
                b.Property(s => s.Name).HasMaxLength(FieldLengths.SensorType);
            }
            );

            builder.Entity<DeviceType>(b =>
            {
                b.Property(s => s.Name).HasMaxLength(FieldLengths.DeviceType);
            }
            );

            // builder.Entity<SolarSystem>(b =>
            // {
            //     b.Property(s => s.Name).HasMaxLength(FieldLengths.Short);
            //     b.Property(s => s.Description).HasMaxLength(FieldLengths.Long);
            // }
            // );

            // builder.Entity<WeatherBase>(b =>
            // {
            //     b.Property(s => s.Name).HasMaxLength(FieldLengths.Short);
            //     b.Property(s => s.Description).HasMaxLength(FieldLengths.Long);
            // }
            // );

            // Devices
            builder.Entity<Device>()
                .HasDiscriminator(d => d.TypeId)
                .HasValue<Device>((int)CommonTypes.DeviceType.Unset)
                .HasValue<ChargeController>((int)CommonTypes.DeviceType.ChargeController)
                .HasValue<BatteryBank>((int)CommonTypes.DeviceType.BatteryBank)
                .HasValue<SolarArray>((int)CommonTypes.DeviceType.SolarArray)
                .HasValue<Inverter>((int)CommonTypes.DeviceType.Inverter)
                .HasValue<WeatherStation>((int)CommonTypes.DeviceType.WeatherStation);

            ConfigureDevice<BatteryBank>(builder)
              .Entity<BatteryBank>(b =>
              {
                  b.Property(s => s.Configuration).HasMaxLength(FieldLengths.BatteryConfiguration);
              });
            ConfigureDevice<ChargeController>(builder);
            ConfigureDevice<Inverter>(builder);
            ConfigureDevice<SolarArray>(builder)
              .Entity<SolarArray>(b =>
              {
                  b.Property(s => s.Configuration).HasMaxLength(FieldLengths.PanelConfiguration);
              });
            ConfigureDevice<WeatherStation>(builder);


            builder.Entity<Sensor>()
                .HasDiscriminator(s => s.TypeId)
                .HasValue<Sensor>((int)CommonTypes.SensorType.Unset)
                .HasValue<Shunt>((int)CommonTypes.SensorType.Shunt)
                .HasValue<CurrentSensor>((int)CommonTypes.SensorType.CurrentSensor)
                .HasValue<BatterySocSensor>((int)CommonTypes.SensorType.BatterySocSensor)
                .HasValue<BatteryChargeStageSensor>((int)CommonTypes.SensorType.BatteryChargeStageSensor)
                .HasValue<ChargeControllerLoadOutput>((int)CommonTypes.SensorType.ChargeControllerLoadOutput)
                .HasValue<BatteryStatsSensor>((int)CommonTypes.SensorType.BatteryStatsSensor)
                .HasValue<EnergyStatsSensor>((int)CommonTypes.SensorType.EnergyStatsSensor)
                .HasValue<TemperatureSensor>((int)CommonTypes.SensorType.TemperatureSensor)
                .HasValue<HumiditySensor>((int)CommonTypes.SensorType.HumiditySensor)
                .HasValue<BarometricPressureSensor>((int)CommonTypes.SensorType.BarometricPressureSensor)
                .HasValue<WindSensor>((int)CommonTypes.SensorType.WindSensor);

            ConfigureSensor<Shunt>(builder);
            ConfigureSensor<CurrentSensor>(builder);
            ConfigureSensor<BatterySocSensor>(builder);
            ConfigureSensor<BatteryChargeStageSensor>(builder);
            ConfigureSensor<BatteryStatsSensor>(builder);
            ConfigureSensor<ChargeControllerLoadOutput>(builder);
            ConfigureSensor<EnergyStatsSensor>(builder);
            ConfigureSensor<TemperatureSensor>(builder);
            ConfigureSensor<HumiditySensor>(builder);
            ConfigureSensor<BarometricPressureSensor>(builder);
            ConfigureSensor<WindSensor>(builder);

            // Measurements
            ConfigureMeasurement<ShuntMeasurement>(builder);
            ConfigureMeasurement<CurrentSensorMeasurement>(builder);
            ConfigureMeasurement<BatterySocMeasurement>(builder);
            ConfigureMeasurement<BatteryChargeStageMeasurement>(builder);
            ConfigureMeasurement<BatteryStatsMeasurement>(builder);
            ConfigureMeasurement<ChargeControllerLoadOutputMeasurement>(builder);
            ConfigureMeasurement<EnergyStatsMeasurement>(builder);
            ConfigureMeasurement<TemperatureMeasurement>(builder);
            ConfigureMeasurement<HumidityMeasurement>(builder);
            ConfigureMeasurement<BarometricPressureMeasurement>(builder);
            ConfigureMeasurement<WindMeasurement>(builder);

            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
    }

}
