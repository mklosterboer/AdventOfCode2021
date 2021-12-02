using Xunit;
using static AdventOfCode2021.Problems.Day02;

namespace AdventOfCode2021.Test.Day02Tests
{
    public class InstructionTests
    {
        [Theory]
        [InlineData("up 5", Direction.Up, 5)]
        [InlineData("down 5", Direction.Down, 5)]
        [InlineData("forward 5", Direction.Foward, 5)]
        [InlineData("forward 10", Direction.Foward, 10)]
        public void Instruction_Handles_AllDirections(
            string instructionString, Direction direction, int amount)
        {
            // Act
            var actual = new Instruction(instructionString);

            //Assert
            Assert.Equal(direction, actual.Direction);
            Assert.Equal(amount, actual.Amount);
        }
    }
}
