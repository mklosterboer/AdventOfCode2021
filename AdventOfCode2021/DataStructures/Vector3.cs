namespace AdventOfCode2021.DataStructures
{
    internal record Vector3(int X, int Y, int Z)
    {
        public static Vector3 operator +(Vector3 a, Vector3 b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        public static Vector3 operator -(Vector3 a, Vector3 b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

        public Vector3(string x, string y, string z)
            : this(int.Parse(x), int.Parse(y), int.Parse(z))
        { }

        public override string ToString()
        {
            return $"[{X}, {Y}, {Z}]";
        }
    }
}
