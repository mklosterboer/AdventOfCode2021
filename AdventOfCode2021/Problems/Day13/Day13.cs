using AdventOfCode2021.Utilities;
using System.Text.RegularExpressions;

namespace AdventOfCode2021.Problems
{
    internal class Day13 : Problem
    {
        protected override string InputName => "Actual";

        private readonly Regex PointRegex = new(@"(?<x>\d*),(?<y>\d*)");
        private readonly Regex InstructionRegex = new(@"(?<d>\S)=(?<l>\d*)$");

        public override object PartOne()
        {
            (var points, var instructions) = GetPointsAndInstructions();

            var singleInstructionList = new List<Instruction>() { instructions.First() };

            var foldedPoints = FoldPoints(points, singleInstructionList);

            return foldedPoints.Count;
        }

        public override object PartTwo()
        {
            (var points, var instructions) = GetPointsAndInstructions();

            var foldedPoints = FoldPoints(points, instructions);

            PrintGrid(foldedPoints);

            return "";
        }

        private (HashSet<Point> points, List<Instruction> instructions) GetPointsAndInstructions()
        {
            var input = GetInputValue();

            var points = new HashSet<Point>();
            var instructions = new List<Instruction>();

            // Parse out input into points and instructions
            foreach (var row in input)
            {
                var pointMatch = PointRegex.Match(row);
                if (pointMatch.Success)
                {
                    var x = pointMatch.Groups["x"].Value;
                    var y = pointMatch.Groups["y"].Value;
                    points.Add(new Point(x, y));
                }
                else
                {
                    var instructionMatch = InstructionRegex.Match(row);
                    if (instructionMatch.Success)
                    {
                        var direction = instructionMatch.Groups["d"].Value;
                        var foldLine = instructionMatch.Groups["l"].Value;
                        instructions.Add(new Instruction(direction, foldLine));
                    }
                }
            }

            return (points, instructions);
        }

        private static HashSet<Point> FoldPoints(HashSet<Point> points, List<Instruction> instructions)
        {
            var newPoints = new HashSet<Point>(points);

            foreach (var instruction in instructions)
            {
                if (instruction.Direction == "x")
                {
                    // Fold "left" along X
                    var pointsToMove = newPoints
                        .Where(p => p.X > instruction.Line)
                        .ToList();

                    foreach (var point in pointsToMove)
                    {
                        // Remove this point from the main list
                        newPoints.Remove(point);

                        // Translate the point and attempt to add to list of points
                        var newPoint = new Point(Translate(instruction.Line, point.X), point.Y);
                        newPoints.Add(newPoint);
                    }
                }
                else
                {
                    // Fold "Up" along Y
                    var pointsToMove = newPoints
                        .Where(p => p.Y > instruction.Line)
                        .ToList();

                    foreach (var point in pointsToMove)
                    {
                        // Remove this point from the main list
                        newPoints.Remove(point);

                        // Translate the point and attempt to add to list of points
                        var newPoint = new Point(point.X, Translate(instruction.Line, point.Y));
                        newPoints.Add(newPoint);
                    }
                }
            }

            return newPoints;
        }

        private static int Translate(int foldLine, int point) =>
            foldLine - (point - foldLine);

        private static void PrintGrid(HashSet<Point> points)
        {
            var height = points.Max(p => p.Y);
            var width = points.Max(p => p.X);

            Console.WriteLine("");

            for (var y = 0; y < height + 1; y++)
            {
                var stringToPrint = "";
                for (var x = 0; x < width + 1; x++)
                {
                    if (points.Contains(new Point(x, y)))
                    {
                        // unicode square for the letters
                        stringToPrint += '\u25A0';
                    }
                    else
                    {
                        // Empty space for every other space
                        stringToPrint += " ";
                    }
                }
                Console.WriteLine(stringToPrint);
            }

            Console.WriteLine("");
        }
    }

    internal record Point
    {
        public int X;
        public int Y;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Point(string x, string y)
        {
            X = int.Parse(x);
            Y = int.Parse(y);
        }
    }

    internal record Instruction
    {
        public string Direction;
        public int Line;

        public Instruction(string direction, string line)
        {
            Direction = direction;
            Line = int.Parse(line);
        }
    }
}
