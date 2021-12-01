namespace AdventOfCode2021.Utilities
{
    public interface IProblem
    {
        object PartOne();

        object? PartTwo() => null;
    }

    abstract public class Problem : IProblem
    {
        protected abstract string ProblemName { get; }

        public Problem()
        {
        }

        protected string GetInputValue()
        {
            return File.ReadAllText($"Data/{ProblemName}.txt");
        }

        protected IEnumerable<int> GetInputNumberList()
        {
            var inputValues = GetInputValue();

            return inputValues.Split('\n').Select(int.Parse).ToList();
        }

        protected IEnumerable<string> GetInputStringList()
        {
            var inputValues = GetInputValue();

            return inputValues.Split('\n').Select(x => x.Replace("\n", "").Replace("\r", "")).ToList();
        }

        abstract public object PartOne();
        abstract public object PartTwo();
    }
}
