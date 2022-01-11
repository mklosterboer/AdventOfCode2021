namespace AdventOfCode2021.Problems
{
    internal class Reactor
    {
        private readonly int[,,] Cubes = new int[101, 101, 101];

        public void ApplyStep(Step step)
        {
            // Loop over all the points in the step and turn the cube on or off
            for (var x = step.X.Start; x <= step.X.End; x++)
            {
                for (var y = step.Y.Start; y <= step.Y.End; y++)
                {
                    for (int z = step.Z.Start; z <= step.Z.End; z++)
                    {
                        if (step.Action)
                        {
                            TurnOnCube(x, y, z);
                        }
                        else
                        {
                            TurnOffCube(x, y, z);
                        }
                    }
                }
            }
        }

        public int NumberOfOnCubes => Cubes.Cast<int>().Sum();

        private void TurnOnCube(int x, int y, int z)
        {
            // The array is -50 to 50 in all directions.
            // offset the instructions by 50, so to a x = 50 means x = 0.
            Cubes[x + 50, y + 50, z + 50] = 1;
        }

        private void TurnOffCube(int x, int y, int z)
        {
            // The array is -50 to 50 in all directions.
            // offset the instructions by 50, so to a x = 50 means x = 0.
            Cubes[x + 50, y + 50, z + 50] = 0;
        }
    }
}
