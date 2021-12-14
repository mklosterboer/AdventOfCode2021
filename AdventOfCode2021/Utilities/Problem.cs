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

        protected string[] GetInputValue()
        { 

            return File.ReadAllLines($"Data/{ProblemName}.txt");
        }

        protected IEnumerable<int> GetInputNumberList()
        {
            var inputValues = GetInputValue();

            return inputValues.Select(int.Parse).ToList();
        }

        protected IEnumerable<string> GetInputStringList()
        {
            var inputValues = GetInputValue();

            return inputValues.Select(x => x.Replace("\n", "").Replace("\r", "")).ToList();
        }

        protected string GetFirstRow()
        {
            return GetInputValue().ElementAt(0);
        }

        abstract public object PartOne();
        abstract public object PartTwo();
    }
}
