using System.Text.Json;
using System.Text.Json.Serialization;
using Sampler.Measurements;

namespace Sampler.ConsoleDemo.Data;

public static class DataLoader
{
    public static List<Measurement> LoadMeasurementsFromJson(string filePath)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
        };
        
        if (!File.Exists(filePath))
            throw new FileNotFoundException("File not found", filePath);
        
        
        using var jsonStream = File.OpenRead(filePath);
        var data = JsonSerializer.Deserialize<List<Measurement>>(jsonStream, options);
        if (data is null)
            throw new Exception("Failed to deserialize data. The file is empty.");

        return data;
    }
}