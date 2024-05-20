namespace Sampler.Measurements;

public class Measurement
{
    public DateTime MeasurementTime { get; set; }
    public double MeasurementValue { get; set; }
    public MeasurementType Type { get; set; }
}