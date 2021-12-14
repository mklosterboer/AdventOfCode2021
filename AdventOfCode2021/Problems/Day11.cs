using AdventOfCode2021.Utilities;

namespace AdventOfCode2021.Problems
{
    internal class Day11 : Problem
    {
        protected override string ProblemName => "Day11";

        public override object PartOne()
        {
            var input = GetInputValue();

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

        private bool AtEdgeLeft { get; init; }
        private bool AtEdgeRight { get; init; }
        private bool AtEdgeTop { get; init; }
        private bool AtEdgeBottom { get; init; }


        public Octopus(int initialEnergyLevel, int x, int y, int height, int width)
        {
            EnergyLevel = initialEnergyLevel;
            X = x;
            Y = y;

            AtEdgeLeft = X == 0;
            AtEdgeTop = Y == 0;
            AtEdgeRight = X == width - 1;
            AtEdgeBottom = Y == height - 1;
        }

        public void ResetFlashed()
        {
            IsFlashing = false;
        }

        public void IncrementEnergyLevel(
            Dictionary<(int x, int y), Octopus> allOctopuses)
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

        public void IncrementNeighbors(
            Dictionary<(int x, int y), Octopus> allOctopuses)
        {
            if (!AtEdgeLeft)
            {
                allOctopuses[(X - 1, Y)].IncrementEnergyLevel(allOctopuses); // Left

                if (!AtEdgeBottom)
                {
                    allOctopuses[(X - 1, Y + 1)].IncrementEnergyLevel(allOctopuses); // Bottom Left
                }
                if (!AtEdgeTop)
                {
                    allOctopuses[(X - 1, Y - 1)].IncrementEnergyLevel(allOctopuses); // Top Left
                }
            }

            if (!AtEdgeRight)
            {
                allOctopuses[(X + 1, Y)].IncrementEnergyLevel(allOctopuses); // Right

                if (!AtEdgeBottom)
                {
                    allOctopuses[(X + 1, Y + 1)].IncrementEnergyLevel(allOctopuses); // Bottom Right
                }
                if (!AtEdgeTop)
                {
                    allOctopuses[(X + 1, Y - 1)].IncrementEnergyLevel(allOctopuses); // Top Right
                }
            }

            if (!AtEdgeTop)
            {
                allOctopuses[(X, Y - 1)].IncrementEnergyLevel(allOctopuses); // Top
            }

            if (!AtEdgeBottom)
            {
                allOctopuses[(X, Y + 1)].IncrementEnergyLevel(allOctopuses); // Bottom
            }
        }
    }
}
