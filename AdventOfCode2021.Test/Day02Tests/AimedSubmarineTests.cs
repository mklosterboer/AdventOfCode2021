using Xunit;
using static AdventOfCode2021.Problems.Day02;

namespace AdventOfCode2021.Test.Day02Tests
{
    public class AimedSubmarineTests
    {
        [Fact]
        public void AimedSubmarine_MovesUpWithAim()
        {
            // Arrange 
            var submarine = new AimedSubmarine();

            // Act
            submarine.Move(new Instruction(Direction.Up, 5));
            submarine.Move(new Instruction(Direction.Foward, 5));

            // Assert
            Assert.Equal(-25, submarine.Depth);
            Assert.Equal(5, submarine.HorizontalPosition);
        }

        [Fact]
        public void AimedSubmarine_MovesDownWithAim()
        {
            // Arrange 
            var submarine = new AimedSubmarine();

            // Act
            submarine.Move(new Instruction(Direction.Down, 5));
            submarine.Move(new Instruction(Direction.Foward, 5));

            // Assert
            Assert.Equal(25, submarine.Depth);
            Assert.Equal(5, submarine.HorizontalPosition);
        }
    }
}
