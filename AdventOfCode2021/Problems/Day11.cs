using AdventOfCode2021.Utilities;

namespace AdventOfCode2021.Problems
{
    internal class Day11 : Problem
    {
        protected override string ProblemName => "Day11";

        public override object PartOne()
        {
            var input = GetInputValue();

            var width = input[0].Length;
            var height = input.Length;

            var octopuses = GetOctopuses(input);

            var flashCount = 0;

            for (int step = 1; step < 101; step++)
            {
                // Increase each energyLevel
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        var currentOctopus = octopuses[(x, y)];
                        currentOctopus.IncrementEnergyLevel(octopuses);
                    }
                }

                //PrintStep(step, octopuses, width, height);

                // Count number of flashing octopus from this step
                // and reset flashed indicator for each octopus
                octopuses.Where(x => x.Value.IsFlashing)
                    .ForEach(x =>
                    {
                        x.Value.ResetFlashed();
                        flashCount++;
                    });
            }

            return flashCount;
        }

        public override object PartTwo()
        {
            var input = GetInputValue();

            var width = input[0].Length;
            var height = input.Length;

            var octopuses = GetOctopuses(input);

            var step = 0;

            while (!octopuses.All(o => o.Value.EnergyLevel == 0))
            {
                step++;
                // Increase each energyLevel
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        var currentOctopus = octopuses[(x, y)];
                        currentOctopus.IncrementEnergyLevel(octopuses);
                    }
                }

                //PrintStep(step, octopuses, width, height);

                // Rest IsFlashed for each flashing octopus
                octopuses.Where(x => x.Value.IsFlashing)
                    .ForEach(x => x.Value.ResetFlashed());
            }


            return step;
        }

        private static Dictionary<(int x, int y), Octopus> GetOctopuses(string[] input)
        {
            var octopuses = new Dictionary<(int x, int y), Octopus>();

            var width = input[0].Length;
            var height = input.Length;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    octopuses[(x, y)] = new Octopus(int.Parse(input[y][x].ToString()), x, y, height, width);
                }
            }

            return octopuses;
        }

        private static void PrintStep(int step, Dictionary<(int x, int y), Octopus> currentDictionary, int width, int height)
        {
            Console.WriteLine("");
            Console.WriteLine($"Step: {step}");
            for (int y = 0; y < height; y++)
            {
                if (y != 0) Console.WriteLine("");
                for (int x = 0; x < width; x++)
                {
                    var currentValue = currentDictionary[(x, y)].EnergyLevel;
                    if (currentValue == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(currentValue);
                        Console.ResetColor();

                    }
                    else
                    {
                        Console.Write(currentValue);
                    }
                }
            }
            Console.WriteLine("");
        }
    }

    internal class Octopus
    {
        public int EnergyLevel { get; private set; }
        public bool IsFlashing { get; private set; }
        public int X { get; init; }
        public int Y { get; init; }

        private List<(int x, int y)> Neighbors { get; init; }

        public Octopus(int initialEnergyLevel, int x, int y, int height, int width)
        {
            EnergyLevel = initialEnergyLevel;
            X = x;
            Y = y;

            Neighbors = InitilizeNeighbors(width, height);
        }

        public void ResetFlashed()
        {
            IsFlashing = false;
        }

        public void IncrementEnergyLevel(Dictionary<(int x, int y), Octopus> allOctopuses)
        {
            // Only increment if NOT already flashing.
            // This is reset externally for each step.
            if (!IsFlashing)
            {
                // Increment EnergyLevel
                EnergyLevel++;

                // Flash this octopus and increment neighbors
                if (EnergyLevel > 9)
                {
                    IsFlashing = true;
                    EnergyLevel = 0;

                    IncrementNeighbors(allOctopuses);
                }
            }
        }

        public void IncrementNeighbors(Dictionary<(int x, int y), Octopus> allOctopuses)
        {
            foreach (var neighbor in Neighbors)
            {
                allOctopuses[neighbor].IncrementEnergyLevel(allOctopuses);
            }
        }

        private List<(int x, int y)> InitilizeNeighbors(int width, int height)
        {
            var neighbors = new List<(int x, int y)>();

            var atEdgeLeft = X == 0;
            var atEdgeTop = Y == 0;
            var atEdgeRight = X == width - 1;
            var atEdgeBottom = Y == height - 1;

            if (!atEdgeLeft)
            {
                neighbors.Add((X - 1, Y)); // Left

                if (!atEdgeBottom)
                {
                    neighbors.Add((X - 1, Y + 1)); // Bottom Left
                }
                if (!atEdgeTop)
                {
                    neighbors.Add((X - 1, Y - 1)); // Top Left
                }
            }

            if (!atEdgeRight)
            {
                neighbors.Add((X + 1, Y)); // Right

                if (!atEdgeBottom)
                {
                    neighbors.Add((X + 1, Y + 1)); // Bottom Right
                }
                if (!atEdgeTop)
                {
                    neighbors.Add((X + 1, Y - 1)); // Top Right
                }
            }

            if (!atEdgeTop)
            {
                neighbors.Add((X, Y - 1)); // Top
            }

            if (!atEdgeBottom)
            {
                neighbors.Add((X, Y + 1)); // Bottom
            }

            return neighbors;
        }
    }
}
