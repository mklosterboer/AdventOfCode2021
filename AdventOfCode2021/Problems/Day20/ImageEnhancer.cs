using AdventOfCode2021.DataStructures;
using System.Collections.ObjectModel;

namespace AdventOfCode2021.Problems
{
    internal class ImageEnhancer
    {
        private static readonly IEnumerable<Vector2> surroundingPoints = new Vector2[]
        {
            // Top
            new (-1, -1),
            new (0, -1),
            new (1, -1),

            // Middle
            new (-1, 0),
            new (0, 0),
            new (1, 0),

            // Bottom
            new (-1, 1),
            new (0, 1),
            new (1, 1),
        };

        private readonly ReadOnlyCollection<bool> Algorithm;
        private bool SurroundingColor = false;

        public ImageEnhancer(ReadOnlyCollection<bool> enhancementAlg)
        {
            Algorithm = enhancementAlg;
        }

        public bool[][] Enhance(bool[][] image)
        {
            var height = image.Length;
            var width = image[0].Length;
            int boundary = 2;

            var result = new bool[height + boundary * 2][];

            for (int y = -boundary; y < height + boundary; y++)
            {
                result[y + boundary] = new bool[width + boundary * 2];

                for (int x = -boundary; x < width + boundary; x++)
                {
                    result[y + boundary][x + boundary] = ResolvePixel(image, y, x);
                }
            }

            // Check the infinite surroundings
            // If it is dark and 9 darks turn into a light => flip the background
            // If it is lit and 9 lights turn into dark => also flip
            if ((!SurroundingColor && Algorithm[0]) || (SurroundingColor && !Algorithm.Last()))
            {
                SurroundingColor = !SurroundingColor;
            }

            return result;
        }

        private bool ResolvePixel(bool[][] image, int y, int x)
        {
            var stringValue = "";
            var position = new Vector2(x, y);

            foreach (var c in surroundingPoints)
            {
                var coord = position + c;
                if (coord.Y >= 0 && coord.Y < image.Length
                    && coord.X >= 0 && coord.X < image[0].Length)
                {
                    // Within bounds, we look at the input image
                    stringValue += image[coord.Y][coord.X] ? 1 : 0;
                }
                else
                {
                    // Outside the bounds, use the fake infinite surroundings
                    stringValue += SurroundingColor ? 1 : 0;
                }
            }

            var numericValue = Convert.ToInt32(stringValue, 2);

            return Algorithm[numericValue];
        }
    }
}
