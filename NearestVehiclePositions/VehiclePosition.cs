namespace NearestVehiclePositions;

public class VehiclePosition
{
    private int Id { get; }
    public string Registration { get; }
    public float Latitude { get; }
    public float Longitude { get; }
    public DateTime RecordedAt { get; }

    public VehiclePosition(int id, string registration, float latitude, float longitude, DateTime recordedAt)
    {
        Id = id;
        Registration = registration;
        Latitude = latitude;
        Longitude = longitude;
        RecordedAt = recordedAt;
    }
}