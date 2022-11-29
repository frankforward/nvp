using System.Text;

namespace NearestVehiclePositions;

public static class VehiclePositionsReader
{
    private static int MoveToNext(int current) => current + 4;

    public static List<VehiclePosition> Read(byte[] bytes)
    {
        var list = new List<VehiclePosition>();

        var current = 0;
        while (current < bytes.Length)
        {
            var id = BitConverter.ToInt32(bytes, current);
            current = MoveToNext(current);
            var registration = new StringBuilder();

            while (bytes[current] != 0)
            {
                registration.Append(bytes[current]);
                current++;
            }

            current++;
            var latitude = BitConverter.ToSingle(bytes, current);

            current = MoveToNext(current);
            var longitude = BitConverter.ToSingle(bytes, current);

            current = MoveToNext(current);
            var recordedAt = BitConverter.ToUInt64(bytes, current);
            var recordedAtDateTime = DateTime.UnixEpoch.AddSeconds(recordedAt);

            current = MoveToNext(current);
            current = MoveToNext(current);

            var vehiclePosition =
                new VehiclePosition(id, registration.ToString(), latitude, longitude, recordedAtDateTime);
            list.Add(vehiclePosition);
        }

        // TODO: there are a few repeated registrations, will go with the assumption that all 2 million records are for unique vehicles
        // var unique = list
        //     .GroupBy(x => x.Registration,
        //         (key, vehiclePosition) =>
        //             vehiclePosition.OrderByDescending(p => p.RecordedAt).First())
        //     .ToList(); 
        // return unique;

        return list;
    }
}