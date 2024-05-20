# Measurement Sampler

## Overview
The Measurement Sampler is a C# library designed to process time-based data from medical devices, aggregating measurements into specified time intervals. It supports different types of measurements like Temperature, SpO2, HeartRate, etc., and provides functionalities to sample the data into intervals for easier analysis and visualization.

## Features
 - Customizable Sampling Resolution*: Allows setting a custom time interval for sampling measurements.
 - Type-Specific Sampling*: Samples measurements separately based on their types.
 - Latest Value Selection*: From each interval, only the latest measurement value is selected.
 - Time-Based Filtering*: Filters measurements based on start time and measurement type.

## Getting Started

### Prerequisites
- .NET SDK (version 8.0 or higher)

### Building the Project
To build the project, run the following command:
```bash
dotnet build
```

To run the unit tests, use the following command:
```bash
dotnet test
```

### Run the Demo

To see a demo of the library in action, run the following command:
```bash
cd src/Sampler.ConsoleDemo ; dotnet run
```

The Demo application can be found in the `src/Sampler.ConsoleDemo` directory. It demonstrates how to use the library to sample measurements.