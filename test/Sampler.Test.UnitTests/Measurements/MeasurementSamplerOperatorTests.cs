using FluentAssertions;
using Sampler.Console.Measurements;
using Sampler.Test.UnitTests.Factories;

namespace Sampler.Test.UnitTests.Measurements;

public class MeasurementSamplerOperatorTests
{
    [Theory]
    [InlineData("2017-01-03T12:02:00", "2017-01-03T11:51:30", "2017-01-03T11:52:00", 5)]
    [InlineData("2017-01-03T12:02:00", "2017-01-03T11:55:30", "2017-01-03T11:57:00", 5)]
    [InlineData("2017-01-03T12:02:00", "2017-01-03T12:04:30", "2017-01-03T12:07:00", 5)]
    [InlineData("2017-01-03T12:02:00", "2017-01-03T12:07:30", "2017-01-03T12:12:00", 5)]
    public void RoundUpToNextInterval_ShouldRoundUpCorrectly_WhenGivenStartTime(string startTime,
        string measurementTime, string expected, int intervalInMinutes)
    {
        // Arrange
        var start = DateTime.Parse(startTime);
        var measurement = DateTime.Parse(measurementTime);
        var expectedTime = DateTime.Parse(expected);
        var interval = TimeSpan.FromMinutes(intervalInMinutes);

        // Act
        var result = MeasurementSampler.CalculateInterval(measurement, interval, start);

        // Assert
        result.Should().Be(expectedTime);
    }


    [Theory]
    [InlineData("2017-01-03T12:02:00", "2017-01-03T11:57:00", "2017-01-03T11:57:00", 5)]
    [InlineData("2017-01-03T12:02:00", "2017-01-03T12:02:00", "2017-01-03T12:02:00", 5)]
    [InlineData("2017-01-03T12:02:00", "2017-01-03T12:07:00", "2017-01-03T12:07:00", 5)]
    [InlineData("2017-01-03T12:00:00", "2017-01-03T11:57:00", "2017-01-03T11:57:00", 3)]
    [InlineData("2017-01-03T12:00:00", "2017-01-03T12:00:00", "2017-01-03T12:00:00", 3)]
    [InlineData("2017-01-03T12:00:00", "2017-01-03T12:03:00", "2017-01-03T12:03:00", 3)]
    public void RoundUpToNextInterval_ShouldNotRoundUp_WhenTimeIsExactlyOnInterval(string startTime,
        string measurementTime, string expected, int intervalInMinutes)
    {
        // Arrange
        var start = DateTime.Parse(startTime);
        var measurement = DateTime.Parse(measurementTime);
        var expectedTime = DateTime.Parse(expected);
        var interval = TimeSpan.FromMinutes(intervalInMinutes);

        // Act
        var result = MeasurementSampler.CalculateInterval(measurement, interval, start);

        // Assert
        result.Should().Be(expectedTime);
    }


    [Fact]
    public void SelectSampledMeasurement_ShouldReturnLatestMeasurement_WhenGivenListOfMeasurements()
    {
        // Arrange
        var measurements = new List<Measurement>
        {
            MeasurementFactory.CreateMeasurement("2017-01-03T10:04:45", 35.79, MeasurementType.TEMP),
            MeasurementFactory.CreateMeasurement("2017-01-03T10:01:18", 98.78, MeasurementType.SpO2),
            MeasurementFactory.CreateMeasurement("2017-01-03T10:09:07", 35.01, MeasurementType.TEMP)
        };
        var sampleTime = DateTime.Parse("2017-01-03T10:10:00");

        // Act
        var result = MeasurementSampler.SelectSampledMeasurement(sampleTime, measurements);

        // Assert
        result.MeasurementTime.Should().Be(sampleTime);
        result.MeasurementValue.Should().Be(35.01);
        result.Type.Should().Be(MeasurementType.TEMP);
    }
    
    [Fact]
    public void FilterByStartTime_ShouldReturnMeasurementsAfterStartTime_WhenGivenListOfMeasurements()
    {
        // Arrange
        var measurements = new List<Measurement>
        {
            MeasurementFactory.CreateMeasurement("2017-01-03T09:59:59", 35.79, MeasurementType.TEMP),
            MeasurementFactory.CreateMeasurement("2017-01-03T10:01:18", 98.78, MeasurementType.SpO2),
            MeasurementFactory.CreateMeasurement("2017-01-03T10:09:07", 35.01, MeasurementType.TEMP)
        };
        var startTime = DateTime.Parse("2017-01-03T10:00:00");

        // Act
        var result = MeasurementSampler.FilterByStartTime(measurements, startTime);

        // Assert
        result = result.ToList();
        result.Should().HaveCount(2);
        result.Should().Contain(m => m.MeasurementTime == DateTime.Parse("2017-01-03T10:01:18"));
        result.Should().Contain(m => m.MeasurementTime == DateTime.Parse("2017-01-03T10:09:07"));
    }
    
    [Fact]
    public void FilterByType_ShouldReturnMeasurementsOfSpecifiedType_WhenGivenListOfMeasurements()
    {
        // Arrange
        var measurements = new List<Measurement>
        {
            MeasurementFactory.CreateMeasurement("2017-01-03T10:04:45", 35.79, MeasurementType.TEMP),
            MeasurementFactory.CreateMeasurement("2017-01-03T10:01:18", 98.78, MeasurementType.SpO2),
            MeasurementFactory.CreateMeasurement("2017-01-03T10:09:07", 35.01, MeasurementType.TEMP)
        };

        // Act
        var result = MeasurementSampler.FilterByType(measurements, MeasurementType.TEMP);

        // Assert
        result = result.ToList();
        result.Should().HaveCount(2);
        result.Should().Contain(m => m.MeasurementTime == DateTime.Parse("2017-01-03T10:04:45"));
        result.Should().Contain(m => m.MeasurementTime == DateTime.Parse("2017-01-03T10:09:07"));
    }
    
     [Fact]
    public void SampleMeasurements_ShouldGroupAndAggregateMeasurements()
    {
        // Arrange
        var unsampledMeasurements = new List<Measurement>
        {
            MeasurementFactory.CreateMeasurement("2017-01-03T10:01:01", 1),
            MeasurementFactory.CreateMeasurement("2017-01-03T10:01:00", 2),
            MeasurementFactory.CreateMeasurement("2017-01-03T10:02:00", 3),
            MeasurementFactory.CreateMeasurement("2017-01-03T10:02:01", 4),
            MeasurementFactory.CreateMeasurement("2017-01-03T10:03:01", 5),
            MeasurementFactory.CreateMeasurement("2017-01-03T10:03:00", 6),
        };

        // Group by minute
        Func<DateTime, DateTime> calculateInterval = (dt) => dt.AddSeconds(-dt.Second);
        
        // Aggregate by second 1
        Func<DateTime, IEnumerable<Measurement>, Measurement> aggregateMeasurements = (sampleTime, measurements) => measurements.First(x => x.MeasurementTime.Second == 1);

        // Act
        var result = MeasurementSampler.SampleMeasurements(unsampledMeasurements, calculateInterval, aggregateMeasurements);

        // Assert
        result.Should().HaveCount(3);
        result[0].MeasurementTime.Should().Be(DateTime.Parse("2017-01-03T10:01:01"));
        result[0].MeasurementValue.Should().Be(1);
        result[1].MeasurementTime.Should().Be(DateTime.Parse("2017-01-03T10:02:01"));
        result[1].MeasurementValue.Should().Be(4);
        result[2].MeasurementTime.Should().Be(DateTime.Parse("2017-01-03T10:03:01"));
        result[2].MeasurementValue.Should().Be(5);

    }
}