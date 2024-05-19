using FluentAssertions;
using Sampler.Console.Measurements;

namespace Sampler.Test.UnitTests.Measurements;

public class MeasurementSamplerTests
{
    
    [Theory]
    [InlineData("2017-01-03T12:02:00", "2017-01-03T11:51:30", "2017-01-03T11:52:00",  5)]
    [InlineData("2017-01-03T12:02:00", "2017-01-03T11:55:30", "2017-01-03T11:57:00",  5)]
    [InlineData("2017-01-03T12:02:00", "2017-01-03T12:04:30", "2017-01-03T12:07:00",  5)]
    [InlineData("2017-01-03T12:02:00", "2017-01-03T12:07:30", "2017-01-03T12:12:00",  5)]
    public void RoundUpToNextInterval_ShouldRoundUpCorrectly_WhenGivenStartTime(string startTime, string measurementTime, string expected, int intervalInMinutes)
    {
        // Arrange
        var start = DateTime.Parse(startTime);
        var measurement = DateTime.Parse(measurementTime);
        var expectedTime = DateTime.Parse(expected);
        var interval = TimeSpan.FromMinutes(intervalInMinutes);

        // Act
        var result = MeasurementSampler.RoundUpToNextInterval(measurement, interval, start);

        // Assert
        result.Should().Be(expectedTime);
    }


    [Theory]
    [InlineData("2017-01-03T12:02:00", "2017-01-03T11:57:00", "2017-01-03T11:57:00",  5)]
    [InlineData("2017-01-03T12:02:00", "2017-01-03T12:02:00", "2017-01-03T12:02:00",  5)]
    [InlineData("2017-01-03T12:02:00", "2017-01-03T12:07:00", "2017-01-03T12:07:00",  5)]
    [InlineData("2017-01-03T12:00:00", "2017-01-03T11:57:00", "2017-01-03T11:57:00",  3)]
    [InlineData("2017-01-03T12:00:00", "2017-01-03T12:00:00", "2017-01-03T12:00:00",  3)]
    [InlineData("2017-01-03T12:00:00", "2017-01-03T12:03:00", "2017-01-03T12:03:00",  3)]
    public void RoundUpToNextInterval_ShouldNotRoundUp_WhenTimeIsExactlyOnInterval(string startTime, string measurementTime, string expected, int intervalInMinutes)
    {
        // Arrange
        var start = DateTime.Parse(startTime);
        var measurement = DateTime.Parse(measurementTime);
        var expectedTime = DateTime.Parse(expected);
        var interval = TimeSpan.FromMinutes(intervalInMinutes);

        // Act
        var result = MeasurementSampler.RoundUpToNextInterval(measurement, interval, start);

        // Assert
        result.Should().Be(expectedTime);
    }
    
    
}