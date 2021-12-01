using System.Diagnostics;

namespace AdventOfCode2021.Utilities
{
    public class Runner
    {
        private readonly IProblem Problem;
        private readonly Stopwatch Stopwatch;

        public Runner(IProblem problem)
        {
            Problem = problem;
            Stopwatch = Stopwatch.StartNew();
        }

        public void Run()
        {
            StartStopwatch();
            var partOneOutput = Problem.PartOne();
            var partOneTime = GetExecutionTime();

            StartStopwatch();
            var partTwoOutput = Problem.PartTwo();
            var partTwoTime = GetExecutionTime();

            Console.WriteLine($"Part One - {partOneTime} ms");
            Console.WriteLine($"    {partOneOutput}");

            Console.WriteLine("");
            Console.WriteLine($"Part Two - {partTwoTime} ms");
            Console.WriteLine($"    {partTwoOutput}");
        }

        private void StartStopwatch()
        {
            Stopwatch.Restart();
        }

        private string GetExecutionTime()
        {
            Stopwatch.Stop();
            return Stopwatch.ElapsedMilliseconds.ToString("F3");
        }
    }
}
