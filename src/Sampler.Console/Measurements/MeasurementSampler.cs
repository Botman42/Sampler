namespace Sampler.Console.Measurements;

public class MeasurementSampler
{
    private readonly TimeSpan _resolution;

    /// <summary>
    /// Initializes the sampler with a default resolution of 5 minutes
    /// </summary>
    public MeasurementSampler()
     : this(TimeSpan.FromMinutes(5))
    {
    }

    /// <summary>
    /// Initialize the sampler with a custom resolution
    /// </summary>
    public MeasurementSampler(TimeSpan resolution)
    {
        _resolution = resolution;
    }


    /// <summary>
    /// Integrator
    /// Processes time-based data from a medical device, aggregating measurements into x-minute intervals.
    /// The interval is defined within the constructor.
    /// </summary>
    /// <param name="startOfSampling">The starting datetime of the sampling.</param>
    /// <param name="unsampledMeasurements">The list of unsampled measurements.</param>
    /// <returns>A dictionary that maps a MeasurementType to a list of sampled measurements.</returns>
    public Dictionary<MeasurementType, List<Measurement>> Sample(DateTime startOfSampling,
        List<Measurement> unsampledMeasurements)
    {
        var filteredMeasurements = FilterByStartTime(unsampledMeasurements, startOfSampling).ToList();
        
        var result = new Dictionary<MeasurementType, List<Measurement>>();

        foreach (var type in Enum.GetValues<MeasurementType>())
        {
            var measurements = FilterByType(filteredMeasurements, type);
            
            result[type] = SampleMeasurements(
                measurements,
                x => CalculateInterval(x, _resolution, startOfSampling),
                SelectSampledMeasurement);
        }

        return result;
    }
    
    /// <summary>
    /// Operator
    /// Takes a list of unsampled measurements, groups thems into intervals. These are then ordered by time and
    /// only the latest of them inside each interval is returned as sampled measurement.
    /// </summary>
    internal static List<Measurement> SampleMeasurements(
        IEnumerable<Measurement> unsampledMeasurements, 
        Func<DateTime, DateTime> calculateInterval,
        Func<DateTime, IEnumerable<Measurement>, Measurement> aggregateMeasurements)
    {
        var sampledMeasurements = unsampledMeasurements
            .GroupBy(x => calculateInterval(x.MeasurementTime))
            .OrderBy(x => x.Key)
            .Select(x => aggregateMeasurements(x.Key, x))
            .ToList();

        return sampledMeasurements;
    }
    

    /// <summary>
    /// Operator
    /// Rounds up a given DateTime value to the next interval based on a specified TimeSpan interval and start DateTime.
    /// The interval will include the right border but exclude the left border (..]
    /// If the DateTime is part of the interval, the right border will be returned.
    /// </summary>
    /// <param name="dt">The DateTime value to be rounded up.</param>
    /// <param name="interval">The interval to be used for rounding up.</param>
    /// <param name="start">The starting DateTime to determine the interval boundaries.</param>
    /// <returns>The rounded up DateTime value.</returns>
    internal static DateTime CalculateInterval(DateTime dt, TimeSpan interval, DateTime start)
    {
        if (dt < start)
        {
            // Handle Times before the start
            var delta = (start - dt).Ticks % interval.Ticks;
            return dt.AddTicks(delta);
        }
        else
        {
            // Handle Times after the start
            var delta = interval.Ticks - (dt - start).Ticks % interval.Ticks;
            
            // If it's exactly on the interval, don't round up
            if (delta == interval.Ticks)
                return dt;
            
            return dt.AddTicks(delta);
        }
    }
    
    /// <summary>
    /// Operator
    /// Will get the latest samples from the given list.
    /// </summary>
    internal static Measurement SelectSampledMeasurement(DateTime sampleTime, IEnumerable<Measurement> measurements)
    {
        var m = measurements.OrderByDescending(x => x.MeasurementTime).First();
        
        return new Measurement
        {
            MeasurementTime = sampleTime,
            MeasurementValue = m.MeasurementValue,
            Type = m.Type
        };
    }
    
    /// <summary>
    /// Operator
    /// Will order the samples by their measurement time.
    /// </summary>
    internal static IEnumerable<Measurement> FilterByStartTime(IEnumerable<Measurement> measurements, DateTime startTime)
    {
        return measurements.Where(x => x.MeasurementTime >= startTime);
    }

    /// <summary>
    /// Operator
    /// Will only select samples of the given type.
    /// </summary>
    internal static IEnumerable<Measurement> FilterByType(IEnumerable<Measurement> samples, MeasurementType type)
    {
        return samples.Where(x => x.Type == type);
    }
    
}