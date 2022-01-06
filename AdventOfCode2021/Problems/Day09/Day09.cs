using AdventOfCode2021.DataStructures;
using AdventOfCode2021.Utilities;

namespace AdventOfCode2021.Problems
{
    internal class Day09 : Problem
    {
        protected override string InputName => "Actual";
        private int[,] HeightMap { get; init; }

        public Day09()
        {
            var input = GetInputValue();
            HeightMap = new int[input[0].Length, input.Length];
            for (int y = 0; y < input.Length; y++)
            {
                for (int x = 0; x < input[y].Length; x++)
                {
                    HeightMap[x, y] = Convert.ToInt32(char.GetNumericValue(input[y][x]));
                }
            }
        }

        public override object PartOne()
        {
            var dangerScore = 0;

            for (int y = 0; y < HeightMap.GetLength(1); y++)
            {
                for (int x = 0; x < HeightMap.GetLength(0); x++)
                {
                    if (LowestRelative(HeightMap, x, y))
                    {
                        dangerScore += HeightMap[x, y] + 1;
                    }
                }
            }

            return dangerScore;
        }

        public override object PartTwo()
        {
            var lowPoints = new List<Vector2>();
            for (int y = 0; y < HeightMap.GetLength(1); y++)
            {
                for (int x = 0; x < HeightMap.GetLength(0); x++)
                {
                    if (LowestRelative(HeightMap, x, y))
                    {
                        lowPoints.Add(new Vector2(x, y));
                    }
                }
            }

            var basins = new List<Basin>();
            foreach (var lowPoint in lowPoints)
            {
                var basin = new Basin();
                basin.SetLowPoint(lowPoint);
                RecursiveBuildBasin(basin, lowPoint);
                basins.Add(basin);
            }

            return basins.OrderByDescending(x => x.GetSize())
                .Take(3)
                .Aggregate(1, (acc, val) => acc * val.GetSize());
        }

        private void RecursiveBuildBasin(Basin b, Vector2 point)
        {
            if (!b.Contains(point))
            {
                b.AddPoint(point);
                foreach (var neighbor in GetNeighbors(point, HeightMap))
                {
                    int val = HeightMap[neighbor.X, neighbor.Y];
                    if (val >= HeightMap[point.X, point.Y] && val < 9)
                    {
                        RecursiveBuildBasin(b, neighbor);
                    }
                }
            }
        }

        private static bool LowestRelative(int[,] heightMap, int x, int y)
        {
            foreach (var neighbor in GetNeighbors(new Vector2(x, y), heightMap))
            {
                if (heightMap[neighbor.X, neighbor.Y] <= heightMap[x, y])
                {
                    return false;
                }

            }
            return true;
        }

        private static List<Vector2> GetNeighbors(Vector2 point, int[,] grid)
        {
            var result = new List<Vector2>();

            if (point.X - 1 >= 0)
            {
                result.Add(new Vector2(point.X - 1, point.Y));
            }
            if (point.X + 1 < grid.GetLength(0))
            {
                result.Add(new Vector2(point.X + 1, point.Y));
            }
            if (point.Y - 1 >= 0)
            {
                result.Add(new Vector2(point.X, point.Y - 1));
            }
            if (point.Y + 1 < grid.GetLength(1))
            {
                result.Add(new Vector2(point.X, point.Y + 1));
            }

            return result;
        }
    }
}
