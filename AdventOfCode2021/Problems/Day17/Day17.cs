using AdventOfCode2021.Utilities;
using System.Numerics;
using System.Text.RegularExpressions;

namespace AdventOfCode2021.Problems
{
    internal class Day17 : Problem
    {
        protected override string InputName => "Actual";
        private readonly Regex ParamsRegex = new(@"x=(?<x1>-?[0-9]\d*)..(?<x2>-?[0-9]\d*), y=(?<y1>-?[0-9]\d*)..(?<y2>-?[0-9]\d*)");
        private TargetZone targetZone { get; set; }
        private Result result { get; set; }

        public Day17()
        {
            var input = GetFirstRow();

            var match = ParamsRegex.Match(input).Groups;

            targetZone = new TargetZone(
                int.Parse(match["x1"].Value),
                int.Parse(match["y1"].Value),
                int.Parse(match["x2"].Value),
                int.Parse(match["y2"].Value)); ;

            result = Simulate();
        }

        public override object PartOne()
        {
            return result.HighestY;
        }

        public override object PartTwo()
        {
            return result.VelocityCounts;
        }

        private Result Simulate()
        {
            int yMax = 500;
            int steps = 400;

            float highestY = -1000;
            var velocityCount = 0;

            for (int xVel = 0; xVel <= targetZone.MaxX; xVel++)
            {
                for (int yVel = -500; yVel <= yMax; yVel++)
                {
                    var testProbe = new Probe(new Vector2(xVel, yVel));

                    for (int t = 0; t < steps; t++)
                    {
                        testProbe.Update();

                        if (targetZone.WithinTargetArea(testProbe.Position))
                        {
                            velocityCount++;

                            if (testProbe.HighestY > highestY)
                            {
                                highestY = testProbe.HighestY;
                            }
                            break;
                        }
                        else if (testProbe.Position.Y < targetZone.MinY || testProbe.Position.X > targetZone.MaxX)
                        {
                            break;
                        }
                    }
                }
            }

            return new Result(highestY, velocityCount);
        }

        private record Result(float HighestY, int VelocityCounts);

        private record TargetZone(int MinX, int MinY, int MaxX, int MaxY)
        {
            public bool WithinTargetArea(Vector2 pointToCheck)
            {
                return pointToCheck.X >= MinX
                    && pointToCheck.X <= MaxX
                    && pointToCheck.Y >= MinY
                    && pointToCheck.Y <= MaxY;
            }
        }

        private class Probe
        {
            public Vector2 Position { get; private set; }
            public float HighestY { get; private set; }
            public Vector2 InitialVelocity { get; private set; }

            private Vector2 Velocity;

            public Probe(Vector2 initialVelocity)
            {
                InitialVelocity = initialVelocity;
                Velocity = initialVelocity;
                Position = new Vector2(0, 0);
            }

            public void Update()
            {
                Position += Velocity;
                Velocity.X = Velocity.X > 0 ? Velocity.X - 1 : 0;
                Velocity.Y--;

                if (Position.Y > HighestY)
                {
                    HighestY = Position.Y;
                }
            }
        }
    }
}
