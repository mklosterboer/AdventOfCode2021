namespace AdventOfCode2021.DataStructures
{
    public record Vector2(int X, int Y)
    {
        public static Vector2 operator +(Vector2 a, Vector2 b) => new(a.X + b.X, a.Y + b.Y);
        public static Vector2 operator -(Vector2 a, Vector2 b) => new(a.X - b.X, a.Y - b.Y);

        public override string ToString()
        {
            return $"[{X}, {Y}]";
        }
    }
}
