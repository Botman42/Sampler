namespace Sampler.Console.Measurements;

public class Measurement
{
    public DateTime MeasurementTime { get; set; }
    public double MeasurementValue { get; set; }
    public MeasurementType Type { get; set; }
}

public enum MeasurementType
{
    TEMP,
    SpO2,
    
    // Insert more Types here
}