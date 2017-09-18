using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SolarMonitor.Data.Repositories.MySql.Migrations
{
    public partial class m1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeviceTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LoadTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<string>(type: "varchar(2)", maxLength: 2, nullable: true),
                    Voltage_V = table.Column<float>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoadTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SensorTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SensorTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false),
                    Name = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false),
                    Timezone = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sites", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true),
                    Password = table.Column<string>(type: "longtext", nullable: true),
                    Username = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    BankVoltage_V = table.Column<float>(type: "float", nullable: true),
                    BatteryVoltage_V = table.Column<float>(type: "float", nullable: true),
                    CapacityPerBattery_Ah = table.Column<float>(type: "float", nullable: true),
                    Configuration = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: true),
                    NumBatteries = table.Column<int>(type: "int", nullable: true),
                    TotalCapacity_Ah = table.Column<float>(type: "float", nullable: true),
                    CurrentRating_A = table.Column<float>(type: "float", nullable: true),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    DetailedSpecs = table.Column<string>(type: "longtext", nullable: true),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false),
                    Manufacturer = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true),
                    Model = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true),
                    Name = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false),
                    SiteId = table.Column<int>(type: "int", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    InputVoltage_V = table.Column<float>(type: "float", nullable: true),
                    MaxContinuousPower_W = table.Column<float>(type: "float", nullable: true),
                    MaxSurgePower_W = table.Column<float>(type: "float", nullable: true),
                    OutputVoltage_V = table.Column<float>(type: "float", nullable: true),
                    SolarArray_Configuration = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: true),
                    NumPanels = table.Column<int>(type: "int", nullable: true),
                    PanelMaxPower_W = table.Column<int>(type: "int", nullable: true),
                    PanelOpenCircuitVoltage_V = table.Column<float>(type: "float", nullable: true),
                    PanelShortCircuitCurrent_A = table.Column<float>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Devices_Sites_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Sites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Devices_DeviceTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "DeviceTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sensors",
                columns: table => new
                {
                    LoadTypeId = table.Column<int>(type: "int", nullable: true),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    DetailedSpecs = table.Column<string>(type: "longtext", nullable: true),
                    Device = table.Column<Guid>(type: "char(36)", nullable: true),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false),
                    Manufacturer = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true),
                    Model = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true),
                    Name = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false),
                    SiteId = table.Column<int>(type: "int", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    InternalResistor_mOhm = table.Column<float>(type: "float", nullable: true),
                    InternalVoltage_mV = table.Column<float>(type: "float", nullable: true),
                    MaxCurrent_A = table.Column<float>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sensors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sensors_LoadTypes_LoadTypeId",
                        column: x => x.LoadTypeId,
                        principalTable: "LoadTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sensors_Sites_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Sites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sensors_SensorTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "SensorTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuthTokens",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false),
                    Expiry = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthTokens", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_AuthTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleAssignments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleAssignments_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleAssignments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Measurements",
                columns: table => new
                {
                    BarometricPressure_mBar = table.Column<float>(type: "float", nullable: true),
                    ChargeStage = table.Column<string>(type: "longtext", nullable: true),
                    SOC = table.Column<int>(type: "int", nullable: true),
                    MaxVoltage = table.Column<float>(type: "float", nullable: true),
                    MinVoltage = table.Column<float>(type: "float", nullable: true),
                    On = table.Column<bool>(type: "bit", nullable: true),
                    Current_A = table.Column<float>(type: "float", nullable: true),
                    Interval_s = table.Column<int>(type: "int", nullable: true),
                    AnnualEnergy_kWh = table.Column<float>(type: "float", nullable: true),
                    DailyEnergy_kWh = table.Column<float>(type: "float", nullable: true),
                    MonthlyEnergy_kWh = table.Column<float>(type: "float", nullable: true),
                    TotalEnergy_kWh = table.Column<float>(type: "float", nullable: true),
                    RelativeHumidity = table.Column<float>(type: "float", nullable: true),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Discriminator = table.Column<string>(type: "longtext", nullable: false),
                    SensorId = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    ShuntMeasurement_Current_A = table.Column<float>(type: "float", nullable: true),
                    Voltage_v = table.Column<float>(type: "float", nullable: true),
                    Temperature_C = table.Column<float>(type: "float", nullable: true),
                    WindDirection_degFromN = table.Column<float>(type: "float", nullable: true),
                    WindSpeed_mps = table.Column<float>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Measurements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Measurements_Sensors_SensorId",
                        column: x => x.SensorId,
                        principalTable: "Sensors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthTokens_UserId",
                table: "AuthTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_Guid",
                table: "Devices",
                column: "Guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Devices_SiteId",
                table: "Devices",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_TypeId",
                table: "Devices",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Measurements_Timestamp",
                table: "Measurements",
                column: "Timestamp");

            migrationBuilder.CreateIndex(
                name: "IX_Measurements_SensorId",
                table: "Measurements",
                column: "SensorId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleAssignments_RoleId",
                table: "RoleAssignments",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleAssignments_UserId",
                table: "RoleAssignments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Sensors_Guid",
                table: "Sensors",
                column: "Guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sensors_LoadTypeId",
                table: "Sensors",
                column: "LoadTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Sensors_SiteId",
                table: "Sensors",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_Sensors_TypeId",
                table: "Sensors",
                column: "TypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthTokens");

            migrationBuilder.DropTable(
                name: "Devices");

            migrationBuilder.DropTable(
                name: "Measurements");

            migrationBuilder.DropTable(
                name: "RoleAssignments");

            migrationBuilder.DropTable(
                name: "DeviceTypes");

            migrationBuilder.DropTable(
                name: "Sensors");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "LoadTypes");

            migrationBuilder.DropTable(
                name: "Sites");

            migrationBuilder.DropTable(
                name: "SensorTypes");
        }
    }
}
