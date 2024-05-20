# Measurement Sampler

## Overview
The Measurement Sampler is a C# library designed to process time-based data from medical devices, aggregating measurements into specified time intervals. It supports different types of measurements like Temperature, SpO2, HeartRate, etc., and provides functionalities to sample the data into intervals for easier analysis and visualization.

## Features
- **Customizable Sampling Resolution**: Allows setting a custom time interval for sampling measurements.
- **Type-Specific Sampling**: Samples measurements separately based on their types.
- **Latest Value Selection**: From each interval, only the latest measurement value is selected.
- **Time-Based Filtering**: Filters measurements based on start time and measurement type.

## Getting Started

### Prerequisites
- .NET SDK (version 8.0 or higher)
