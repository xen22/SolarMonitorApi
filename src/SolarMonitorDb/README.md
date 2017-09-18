# Notes on Database schema and generation

## Quick instructions

* To generate a new schema based on the code:

    ```bash
    cd src/SolarMonitor.Web.Api
    ./regenerateDbSchema.sh
    ```
    Note: this script deletes all previous migrations, adds a new one, then exports the schema into a SQL script stored in src/SolarMonitorDb/src/ef/SolarMonitorDbSchema.sql.

* To re-create the SolarMonitorDb MySQL database and test data:

    ```bash
    cd src/SolarMonitorDb/src
    ./recreate_db.sh
    ```

## Database schema

We are using Entity Framework Core code-first approach to generate the database schema.

Here are a few notes on the restrictions placed on the generated tables by EF Core:

* Hierarchical models

  * model class hierarchies are flattened and converted into a single table because EF Core only supports HPT (Hierarchy per table) approach. For this to work properly, the DbContext needs to have DbSets for both the base class as well as derived classes:
  ```cs
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Sensor> Sensors { get; set; }
        public DbSet<Shunt> Shunts { get; set; }
        public DbSet<CurrentSensor> CurrentSensors { get; set; }
        // DbSets for other derived classes
    }
  ```

  * When generating tables, a Discriminator column of type string is added automatically to the table which is set to the class name. The Discriminator column can be set to an existing class property of a different type (e.g. an enum), which makes it easier to work with. This can be done in the ApplicationDbContext.OnModelCreating():
  ```cs
        builder.Entity<Sensor>()
            .HasDiscriminator(s => s.TypeId)
            .HasValue<Sensor>((int) CommonTypes.SensorType.Unknown)
            .HasValue<Shunt>((int)CommonTypes.SensorType.Shunt)
            .HasValue<CurrentSensor>((int)CommonTypes.SensorType.CurrentSensor)
            // ...
  ```
  Note: it is important to have a HasValue() entry for the base class too.

* Basic notes for creating models:
  * When a model has a reference to another model, we need to add a field with the Id as well as a field to the model itself, e.g.:
  ```cs
    class Sensor
    {
        // ...
        [Required]
        public int DeviceId { get; set; }
        public Device Device { get; set; }
    }
  ```
  The generated table will only contain a DeviceId column but when working with the models via EF, we will be able to access the Device oject directly.
