using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode2021.Problems
{
    internal record Cuboid
    {
        public Range X;
        public Range Y;
        public Range Z;

        public Cuboid(Step step)
        {
            X = step.X;
            Y = step.Y;
            Z = step.Z;
        }

        public Cuboid(Range x, Range y, Range z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Total volume of this cubiod
        /// </summary>
        public ulong Volume => (ulong)X.Length * (ulong)Y.Length * (ulong)Z.Length;

        /// <summary>
        /// Try to get an intersection cube on all axis.
        /// </summary>
        public bool TryIntersect(Cuboid other, [NotNullWhen(true)] out Cuboid? intersection)
        {
            if (TryIntersect(X, other.X, out var x)
                && TryIntersect(Y, other.Y, out var y)
                && TryIntersect(Z, other.Z, out var z))
            {
                intersection = new Cuboid(x, y, z);
                return true;
            }

            intersection = null;
            return false;
        }

        /// <summary>
        /// Try to get an intersection range on one axis.
        /// </summary>
        private static bool TryIntersect(Range first, Range second, [NotNullWhen(true)] out Range? intersection)
        {
            if (first.Start > second.Start)
            {
                (first, second) = (second, first);
            }

            if (first.End < second.Start)
            {
                intersection = null;
                return false;
            }

            intersection = new(second.Start, Math.Min(first.End, second.End));
            return true;
        }
    }
}
