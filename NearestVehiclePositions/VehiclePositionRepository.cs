namespace NearestVehiclePositions;

public static class VehiclePositionRepository
{
    public static IReadOnlyList<VehiclePosition> GetAll()
    {
        // read the positions from file
        const string path = "VehiclePositions.dat";
        var allBytes = File.ReadAllBytes(path);
        var vehiclePositions = VehiclePositionsReader.Read(allBytes);
        return vehiclePositions;
    }
}