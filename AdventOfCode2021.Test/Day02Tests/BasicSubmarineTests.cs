using Xunit;
using static AdventOfCode2021.Problems.Day02;

namespace AdventOfCode2021.Test.Day02Tests
{
    public class BasicSubmarineTests
    {
        [Theory]
        [InlineData(Direction.Up, 5, -5, 0)]
        [InlineData(Direction.Down, 5, 5, 0)]
        [InlineData(Direction.Foward, 5, 0, 5)]
        public void BasicSubmarine_Moves(
            Direction direction, int amount, int depth, int horizontalPosition)
        {
            // Arrange 
            var submarine = new BasicSubmarine();
            var instruction = new Instruction(direction, amount);


            // Act
            submarine.Move(instruction);

            // Assert
            Assert.Equal(depth, submarine.Depth);
            Assert.Equal(horizontalPosition, submarine.HorizontalPosition);
        }
    }
}
