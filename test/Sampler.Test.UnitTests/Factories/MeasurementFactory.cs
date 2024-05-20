using System.Diagnostics.Metrics;
using Sampler.Measurements;

namespace Sampler.Test.UnitTests.Factories;

public static class MeasurementFactory
{
    
    
    public static Measurement CreateMeasurement(
        string measurementTime = "2017-01-03T10:00:00", 
        double measurementValue = 0, 
        MeasurementType type = MeasurementType.TEMP)
    {
        return new Measurement
        {
            MeasurementTime = DateTime.Parse(measurementTime),
            MeasurementValue = measurementValue,
            Type = type
        };
    }
}