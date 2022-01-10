namespace AdventOfCode2021.Problems
{
    internal interface IDice
    {
        public int Roll();
        public int GetNumberOfRolls();
    }

    internal class DeterministicDice : IDice
    {
        private int CurrentValue = 1;
        private int NumberOfRolls = 0;

        public int Roll()
        {
            NumberOfRolls++;

            if (CurrentValue == 100)
            {
                CurrentValue = 0;
            }

            return CurrentValue++;
        }

        public int GetNumberOfRolls() => NumberOfRolls;
    }
}
