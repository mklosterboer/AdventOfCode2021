namespace AdventOfCode2021.DataStructures
{
    public struct Vector2
    {
        public int X;
        public int Y;

        public Vector2(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public static Vector2 operator +(Vector2 a, Vector2 b) => new Vector2(a.X + b.X, a.Y + b.Y);
        public static Vector2 operator -(Vector2 a, Vector2 b) => new Vector2(a.X - b.X, a.Y - b.Y);

        public bool Equals(Vector2 other)
        {
            return other.X == X && other.Y == Y;
        }

        public override string ToString()
        {
            return "[" + X + ", " + Y + "]";
        }

        public Vector2 Clone()
        {
            return new Vector2(X, Y);
        }

        public int SqrMag()
        {
            return X * X + Y * Y;
        }
    }
}
