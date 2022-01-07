using AdventOfCode2021.DataStructures;

namespace AdventOfCode2021.Problems
{
    internal record Scanner(Vector3 Center, int Rotation, List<Vector3> BeaconsInLocal)
    {
        public Scanner Rotate()
        {
            return new Scanner(Center, Rotation + 1, BeaconsInLocal);
        }

        public Scanner Translate(Vector3 translation)
        {
            var newCenter = new Vector3(
                Center.X + translation.X,
                Center.Y + translation.Y,
                Center.Z + translation.Z);
            return new Scanner(newCenter, Rotation, BeaconsInLocal);
        }

        public Vector3 Transform(Vector3 changes)
        {
            var (x, y, z) = changes;

            // Rotate the coordinates so that x points in the possible 6 directions
            (x, y, z) = (Rotation % 6) switch
            {
                0 => (x, y, z),
                1 => (-x, y, -z),
                2 => (y, -x, z),
                3 => (-y, x, z),
                4 => (z, y, -x),
                5 => (-z, y, x),
                _ => throw new NotImplementedException()
            };

            // Rotate around the x-axis
            (x, y, z) = ((Rotation / 6) % 4) switch
            {
                0 => (x, y, z),
                1 => (x, -z, y),
                2 => (x, -y, -z),
                3 => (x, z, -y),
                _ => throw new NotImplementedException()
            };

            return new Vector3(Center.X + x, Center.Y + y, Center.Z + z);
        }

        public IEnumerable<Vector3> GetBeaconsInWorld()
        {
            return BeaconsInLocal.Select(Transform);
        }
    }
}
