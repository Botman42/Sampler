using System.Globalization;
using Sampler.Console.Data;
using Sampler.Console.Measurements;

// Print floating point numbers with a dot as decimal separator
Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");


var jsonFilePath = "./Data/SampleData.json";
var unsortedMeasurements = DataLoader.LoadMeasurementsFromJson(jsonFilePath);

// Sample measurements using a 5 minute interval
var sampler = new MeasurementSampler(TimeSpan.FromMinutes(5));
var sampledMeasurements = sampler.Sample(DateTime.Parse("2017-01-03T10:00:00"), unsortedMeasurements);


// Write to console
foreach (var (_, measurements) in sampledMeasurements)
{
    foreach (var measurement in measurements)
        Console.WriteLine($"{{{measurement.MeasurementTime:s}, {measurement.Type}, {measurement.MeasurementValue:F2}}}");
}

