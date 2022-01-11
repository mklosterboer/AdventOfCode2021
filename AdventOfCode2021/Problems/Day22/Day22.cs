using AdventOfCode2021.Utilities;

namespace AdventOfCode2021.Problems
{
    internal class Day22 : Problem
    {
        protected override string InputName => "Actual";

        public override object PartOne()
        {
            // Create list of steps
            var steps = GetInputValue()
                .Select(line => new Step(line))
                .Where(s => s.IsInInitializationRegion);

            var reactor = new Reactor();

            // Brute force by applying each step
            foreach (var step in steps)
            {
                reactor.ApplyStep(step);
            }

            return reactor.NumberOfOnCubes;
        }

        public override object PartTwo()
        {
            // Get all cuboids from the steps
            var cuboids = GetInputValue()
                .Select(line => new Step(line))
                .Select(s => (IsOn: s.Action, Cube: new Cuboid(s)));

            var allCuboids = new List<(bool IsOn, Cuboid Cube)>();

            // Loop over existing cubiods from steps
            // Find intersection points with all cuboids to create
            // cuboids with ranges as on or off
            foreach (var cuboid in cuboids)
            {
                var count = allCuboids.Count;
                for (var i = 0; i < count; i++)
                {
                    var otherCuboid = allCuboids[i];
                    if (cuboid.Cube.TryIntersect(otherCuboid.Cube, out Cuboid? intersection))
                    {
                        allCuboids.Add((!otherCuboid.IsOn, intersection));
                    }
                }

                if (cuboid.IsOn)
                {
                    allCuboids.Add(cuboid);
                }
            }

            // Calculate the total volume of the on and off cuboids
            ulong volume = 0;

            foreach (var (IsOn, Cube) in allCuboids)
            {
                if (IsOn)
                {
                    volume += Cube.Volume;
                }
                else
                {
                    volume -= Cube.Volume;
                }
            }

            return volume;
        }
    }
}
