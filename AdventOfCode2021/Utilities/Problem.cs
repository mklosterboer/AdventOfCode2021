namespace AdventOfCode2021.Utilities
{
    public interface IProblem
    {
        object PartOne();

        object? PartTwo() => null;
    }

    abstract public class Problem : IProblem
    {
        protected abstract string InputName { get; }

        private string InputFilePath { get; init; }

        public Problem()
        {
            var problemName = this.GetType().Name;
            InputFilePath = $"Problems/{problemName}/{InputName}.txt";
        }

        protected string[] GetInputValue()
        {
            return File.ReadAllLines(InputFilePath);
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
