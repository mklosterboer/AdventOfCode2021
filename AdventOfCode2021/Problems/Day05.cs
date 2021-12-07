using AdventOfCode2021.Utilities;
using System.Text.RegularExpressions;

namespace AdventOfCode2021.Problems
{
    public class Day05 : Problem
    {
        protected override string ProblemName => "Day05";
        private string[] InputData { get; set; }

        public Day05()
        {
            InputData = GetInputValue();
        }

        public override object PartOne()
        {
            var lines = InputData.Select(x => new Line(x)).Where(l => l.IsVertical || l.IsHorizontal);

            return GetDangerZonesCount(lines);
        }

        public override object PartTwo()
        {
            var lines = InputData.Select(x => new Line(x));

            return GetDangerZonesCount(lines);
        }

        private static int GetDangerZonesCount(IEnumerable<Line> lines)
        {
            var hitPoints = new Dictionary<(int x, int y), int>();

            foreach (var line in lines)
            {
                foreach (var point in line.Points)
                {
                    if (hitPoints.ContainsKey(point))
                    {
                        hitPoints[point]++;
                    }
                    else
                    {
                        hitPoints.Add(point, 1);
                    }
                }
            }

            return hitPoints.Where(p => p.Value >= 2).Count();
        }
    }

    public class Line
    {
        public (int X, int Y) PointOne { get; set; }
        public (int X, int Y) PointTwo { get; set; }

        public bool IsVertical { get; set; }
        public bool IsHorizontal { get; set; }

        public List<(int X, int Y)> Points { get; set; }

        private static readonly Regex regex = new(@"(?<x1>\d+),(?<y1>\d+) -> (?<x2>\d+),(?<y2>\d+)");

        public Line(string inputString)
        {
            var matches = regex.Match(inputString);
            PointOne = (int.Parse(matches.Groups["x1"].Value), int.Parse(matches.Groups["y1"].Value));
            PointTwo = (int.Parse(matches.Groups["x2"].Value), int.Parse(matches.Groups["y2"].Value));

            IsVertical = PointOne.X == PointTwo.X;
            IsHorizontal = PointOne.Y == PointTwo.Y;

            Points = new List<(int X, int Y)>();

            SetPoints();
        }

        private void SetPoints()
        {
            if (IsVertical)
            {
                var maxY = Math.Max(PointOne.Y, PointTwo.Y);
                var minY = Math.Min(PointOne.Y, PointTwo.Y);

                for (int y = minY; y <= maxY; y++)
                {
                    Points.Add((PointOne.X, y));
                }
            }
            else
            {
                var slope = (PointTwo.Y - PointOne.Y) / (PointTwo.X - PointOne.X);
                var offset = (PointOne.Y - (slope * PointOne.X));

                var maxX = Math.Max(PointOne.X, PointTwo.X);
                var minX = Math.Min(PointOne.X, PointTwo.X);

                for (int x = minX; x <= maxX; x++)
                {
                    Points.Add((x, slope * x + offset));
                }
            }
        }

    }
}
