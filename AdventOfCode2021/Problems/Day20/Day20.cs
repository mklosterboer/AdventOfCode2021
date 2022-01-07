using AdventOfCode2021.Utilities;
using System.Collections.ObjectModel;

namespace AdventOfCode2021.Problems
{
    internal class Day20 : Problem
    {
        protected override string InputName => "Actual";
        private bool[][] Image { get; init; }
        private ReadOnlyCollection<bool> EnhancementAlg { get; init; }

        public Day20()
        {
            EnhancementAlg = GetFirstRow().Select(x => x == '#').ToList().AsReadOnly();

            Image = GetInputValue()
                .Skip(2)
                .Select(line => line.Select(x => x == '#').ToArray())
                .ToArray();
        }

        public override object PartOne()
        {
            var enchancer = new ImageEnhancer(EnhancementAlg);

            var currentImage = Image;
            for (int i = 1; i <= 2; i++)
            {
                currentImage = enchancer.Enhance(currentImage);
            }

            return currentImage.SelectMany(row => row).Count(p => p);
        }

        public override object PartTwo()
        {
            var enchancer = new ImageEnhancer(EnhancementAlg);

            var currentImage = Image;
            for (int i = 1; i <= 50; i++)
            {
                currentImage = enchancer.Enhance(currentImage);
            }

            return currentImage.SelectMany(row => row).Count(p => p);
        }

        private static void PrintImage(bool[][] image)
        {
            var height = image.Length;
            var width = image[0].Length;

            for (int y = 0; y < height; y++)
            {
                var line = string.Empty;

                for (int x = 0; x < width; x++)
                {
                    line += image[y][x] ? "#" : ".";
                }

                Console.WriteLine(line);
            }
        }
    }
}
