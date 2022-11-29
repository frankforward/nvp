using System.Collections.Concurrent;
using System.Diagnostics;
using NearestVehiclePositions;

/*
 * Tried a couple of approaches, and here are some of the ideas i ran through
 * 1. split the vehicle positions into chunks and process each chuck in parallel, then combine the results
 * 2. similar to 1. but then recursively chunk the list of results, i.e. start with 2 million records,
 *    chunck up the records to get say 200K results, recursively chunk the result till 1 position remains. Chose not to
 *    pursue this route as the results where not favourable
 * 3. similar to 1. however, instead of processing one location of the 10 at a time, calculate the distances for all 10 in a go
 *
 * This branch contains option 1. Option 3 will be in a separate branch
 */


var vehiclePositions=VehiclePositionRepository.GetAll();
var vehiclePositionsCount = vehiclePositions.Count;
Console.WriteLine($"Found {vehiclePositionsCount} positions");

var stopwatch = new Stopwatch();
stopwatch.Start();

// get the 10 positions
var positions = PositionsToCalculate.GetPositions();

// chunk up the calculations to speed up the process. 
const int chunks = 10;
var chunkSize =
    (int)Math.Ceiling((float)vehiclePositionsCount /
                      chunks);

var nearestVehiclePositions = new List<VehiclePosition>();
foreach (var position in positions)
{
    var nearestInChunks = new ConcurrentBag<VehiclePosition>();
    Parallel.For(0, chunks,
        i =>
        {
            // calculate the nearest location in each chunk
            var nearest = DistanceCalculator.GetNearest(position, vehiclePositions, i, chunkSize, vehiclePositionsCount);
            nearestInChunks.Add(nearest);
        });

    // now get the nearest from the list generated in the step above
    var nearestPosition = DistanceCalculator.GetNearest(position, nearestInChunks.ToList(), 0, chunks, chunks);
    nearestVehiclePositions.Add(nearestPosition);
}
stopwatch.Stop();
Console.WriteLine($"Total time spent calculating: {stopwatch.ElapsedMilliseconds}ms");

foreach (var nearestVehiclePosition in nearestVehiclePositions)
{
    Console.WriteLine($"Vehicle: {nearestVehiclePosition.Registration}");

}
