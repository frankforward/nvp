using GeoCoordinatePortable;

namespace NearestVehiclePositions;

public static class DistanceCalculator
{
    public static VehiclePosition GetNearest(GeoCoordinate geoCoordinate, IReadOnlyList<VehiclePosition> vehiclePositions, int chunk,
        int chunkSize, int vehiclePositionsCount)
    {
        var position = chunk * chunkSize;
        var endBefore = Math.Min(position + chunkSize, vehiclePositionsCount);
        var nearestVehiclePosition = vehiclePositions[position];
        var distanceToNearestVehicle = GetDistance(nearestVehiclePosition, geoCoordinate);

        position++;

        // if (ogchunck == -1)
        // {
        //     endBefore = Math.Min(position + chunkSize, vehiclePositions.Count);
        // }

        while (position < endBefore)
        {
            // the following 2 lines are a repeat of the above , refactor?
            var vehiclePosition = vehiclePositions[position];
            var distance = GetDistance(vehiclePosition, geoCoordinate);

            if (distance < distanceToNearestVehicle)
            {
                nearestVehiclePosition = vehiclePosition;
                distanceToNearestVehicle = distance;
            }

            position++;
        }

        return nearestVehiclePosition;
    }

    private static double GetDistance(VehiclePosition vehiclePosition, GeoCoordinate geoCoordinate)
    {
        var current = new GeoCoordinate(vehiclePosition.Latitude, vehiclePosition.Longitude);
        var distance = geoCoordinate.GetDistanceTo(current);
        return distance;
    }
}