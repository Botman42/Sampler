using FluentAssertions;
using Sampler.Measurements;
using Sampler.Test.UnitTests.Factories;

namespace Sampler.Test.UnitTests.Measurements;

public class MeasurementSamplerIntegratorTests
{
    [Fact]
    public void Sample_ShouldReturnEmptyLists_WhenNoSamplesArePresent()
    {
        // Arrange
        var measurements = new List<Measurement>
        {
        };

        var sampler = new MeasurementSampler();
        var startOfSampling = DateTime.Parse("2017-01-03T10:00:00");

        // Act
        var result = sampler.Sample(startOfSampling, measurements);

        // Assert
        result.Should().ContainKey(MeasurementType.TEMP);
        result.Should().ContainKey(MeasurementType.SpO2);

        var tempResults = result[MeasurementType.TEMP];
        tempResults.Should().HaveCount(0);

        var spo2Results = result[MeasurementType.SpO2];
        spo2Results.Should().HaveCount(0);
    }

    [Fact]
    public void Sample_ShouldSampleMeasurementsCorrectly_WhenInputIsValid()
    {
        // Arrange
        var measurements = new List<Measurement>
        {
            MeasurementFactory.CreateMeasurement("2017-01-03T09:04:45", 35.79, MeasurementType.TEMP),
            MeasurementFactory.CreateMeasurement("2017-01-03T10:04:45", 35.79, MeasurementType.TEMP),
            MeasurementFactory.CreateMeasurement("2017-01-03T10:01:18", 98.78, MeasurementType.SpO2),
            MeasurementFactory.CreateMeasurement("2017-01-03T10:09:07", 35.01, MeasurementType.TEMP),
            MeasurementFactory.CreateMeasurement("2017-01-03T10:03:34", 96.49, MeasurementType.SpO2),
            MeasurementFactory.CreateMeasurement("2017-01-03T10:02:01", 35.82, MeasurementType.TEMP),
            MeasurementFactory.CreateMeasurement("2017-01-03T10:05:00", 97.17, MeasurementType.SpO2),
            MeasurementFactory.CreateMeasurement("2017-01-03T10:05:01", 95.08, MeasurementType.SpO2)
        };

        var sampler = new MeasurementSampler();
        var startOfSampling = DateTime.Parse("2017-01-03T10:00:00");

        // Act
        var result = sampler.Sample(startOfSampling, measurements);

        // Assert
        result.Should().ContainKey(MeasurementType.TEMP);
        result.Should().ContainKey(MeasurementType.SpO2);

        var tempResults = result[MeasurementType.TEMP];
        tempResults.Should().HaveCount(2);
        tempResults[0].MeasurementTime.Should().Be(DateTime.Parse("2017-01-03T10:05:00"));
        tempResults[0].MeasurementValue.Should().Be(35.79);
        tempResults[1].MeasurementTime.Should().Be(DateTime.Parse("2017-01-03T10:10:00"));
        tempResults[1].MeasurementValue.Should().Be(35.01);

        var spo2Results = result[MeasurementType.SpO2];
        spo2Results.Should().HaveCount(2);
        spo2Results[0].MeasurementTime.Should().Be(DateTime.Parse("2017-01-03T10:05:00"));
        spo2Results[0].MeasurementValue.Should().Be(97.17);
        spo2Results[1].MeasurementTime.Should().Be(DateTime.Parse("2017-01-03T10:10:00"));
        spo2Results[1].MeasurementValue.Should().Be(95.08);
    }

    [Fact]
    public void Sample_ShouldReturnEmptyLists_WhenMeasurementsAreInThePast()
    {
        // Arrange
        var measurements = new List<Measurement>
        {
            MeasurementFactory.CreateMeasurement("2017-01-03T09:04:45", 35.79, MeasurementType.TEMP),
            MeasurementFactory.CreateMeasurement("2017-01-03T10:04:45", 35.79, MeasurementType.TEMP),
            MeasurementFactory.CreateMeasurement("2017-01-03T10:01:18", 98.78, MeasurementType.SpO2),
            MeasurementFactory.CreateMeasurement("2017-01-03T10:09:07", 35.01, MeasurementType.TEMP),
            MeasurementFactory.CreateMeasurement("2017-01-03T10:03:34", 96.49, MeasurementType.SpO2),
            MeasurementFactory.CreateMeasurement("2017-01-03T10:02:01", 35.82, MeasurementType.TEMP),
            MeasurementFactory.CreateMeasurement("2017-01-03T10:05:00", 97.17, MeasurementType.SpO2),
            MeasurementFactory.CreateMeasurement("2017-01-03T10:05:01", 95.08, MeasurementType.SpO2)
        };

        var sampler = new MeasurementSampler();
        var startOfSampling = DateTime.Parse("2017-01-04T10:00:00");

        // Act
        var result = sampler.Sample(startOfSampling, measurements);

        // Assert
        result.Should().ContainKey(MeasurementType.TEMP);
        result.Should().ContainKey(MeasurementType.SpO2);

        var tempResults = result[MeasurementType.TEMP];
        tempResults.Should().HaveCount(0);

        var spo2Results = result[MeasurementType.SpO2];
        spo2Results.Should().HaveCount(0);
    }

}