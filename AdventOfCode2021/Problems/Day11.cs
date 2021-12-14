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
                // increase each energyLevel
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        var currentOctopus = octopuses[(x, y)];
                        currentOctopus.IncreaseEnergyLevel(octopuses);
                    }
                }

                //PrintStep(step, octopuses, width, height);

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
                // increase each energyLevel
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        var currentOctopus = octopuses[(x, y)];
                        currentOctopus.IncreaseEnergyLevel(octopuses);
                    }
                }

                //PrintStep(step, octopuses, width, height);

                octopuses.Where(x => x.Value.IsFlashing)
                    .ForEach(x =>
                    {
                        x.Value.ResetFlashed();
                    });
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
        public bool HasFlashedNeighbors { get; set; }
        public int X { get; init; }
        public int Y { get; init; }

        private int Height { get; init; }
        private int Width { get; init; }

        public Octopus(int initialEnergyLevel, int x, int y, int height, int width)
        {
            EnergyLevel = initialEnergyLevel;
            X = x;
            Y = y;
            Height = height;
            Width = width;
        }

        public void ResetFlashed()
        {
            IsFlashing = false;
        }

        public void IncreaseEnergyLevel(
            Dictionary<(int x, int y), Octopus> allOctopuses)
        {
            if (!IsFlashing)
            {
                EnergyLevel++;
                TryFlash(allOctopuses);
            }
        }

        public void TryFlash(
            Dictionary<(int x, int y), Octopus> allOctopuses)
        {
            if (EnergyLevel > 9)
            {
                //flashCount++;
                IsFlashing = true;
                EnergyLevel = 0;

                IncrementNeighbors(allOctopuses);
            }
        }

        public void IncrementNeighbors(
            Dictionary<(int x, int y), Octopus> allOctopuses)
        {
            // "touch" adjacent octopuses
            if (X > 0)
            {
                allOctopuses[(X - 1, Y)].ReceiveFlashFromNeighbor(allOctopuses); // Left

                if (Y < Height - 1)
                {
                    allOctopuses[(X - 1, Y + 1)].ReceiveFlashFromNeighbor(allOctopuses); // Bottom Left
                }
                if (Y > 0)
                {
                    allOctopuses[(X - 1, Y - 1)].ReceiveFlashFromNeighbor(allOctopuses); // Top Left
                }
            }
            if (X < Width - 1)
            {
                allOctopuses[(X + 1, Y)].ReceiveFlashFromNeighbor(allOctopuses); // Right

                if (Y < Height - 1)
                {
                    allOctopuses[(X + 1, Y + 1)].ReceiveFlashFromNeighbor(allOctopuses); // Bottom Right
                }
                if (Y > 0)
                {
                    allOctopuses[(X + 1, Y - 1)].ReceiveFlashFromNeighbor(allOctopuses); // Top Right
                }
            }

            if (Y > 0)
            {
                allOctopuses[(X, Y - 1)].ReceiveFlashFromNeighbor(allOctopuses); // Top
            }

            if (Y < Height - 1)
            {
                allOctopuses[(X, Y + 1)].ReceiveFlashFromNeighbor(allOctopuses); // Bottom
            }
        }

        public void ReceiveFlashFromNeighbor(
            Dictionary<(int x, int y), Octopus> allOctopuses)
        {
            if (!IsFlashing)
            {
                IncreaseEnergyLevel(allOctopuses);
            }
        }
    }
}
