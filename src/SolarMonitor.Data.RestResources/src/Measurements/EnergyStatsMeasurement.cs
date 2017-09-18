namespace SolarMonitor.Data.Resources
{
  public class EnergyStatsMeasurement : Measurement {
    public float DailyEnergy_kWh { get; set; }
    public float MonthlyEnergy_kWh { get; set; }
    public float AnnualEnergy_kWh { get; set; }
    public float TotalEnergy_kWh { get; set; }
  }
}
