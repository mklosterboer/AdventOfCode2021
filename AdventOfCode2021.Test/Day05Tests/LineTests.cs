using AdventOfCode2021.Problems;
using Xunit;

namespace AdventOfCode2021.Test.Day05Tests
{
    public class LineTests
    {
        [Theory]
        [InlineData("0,9 -> 5,9", true)]
        [InlineData("7,0 -> 7,4", false)]
        [InlineData("8,0 -> 0,8", false)]
        public void Line_IsHorizontal(string input, bool expected)
        {
            // Arrange
            var line = new Line(input);

            // Assert
            Assert.Equal(expected, line.IsHorizontal);
        }

        [Theory]
        [InlineData("0,9 -> 5,9", false)]
        [InlineData("7,0 -> 7,4", true)]
        [InlineData("8,0 -> 0,8", false)]
        public void Line_IsVertical(string input, bool expected)
        {
            // Arrange
            var line = new Line(input);

            // Assert
            Assert.Equal(expected, line.IsVertical);
        }

        [Fact]
        public void Line_SetPoints_Vertical()
        {
            // Arrange
            var line = new Line("7,0 -> 7,4");

            // Assert
            Assert.Contains((7, 0), line.Points);
            Assert.Contains((7, 1), line.Points);
            Assert.Contains((7, 2), line.Points);
            Assert.Contains((7, 3), line.Points);
            Assert.Contains((7, 4), line.Points);
            Assert.DoesNotContain((7, 5), line.Points);
            Assert.DoesNotContain((8, 1), line.Points);
        }

        [Fact]
        public void Line_SetPoints_Horizontal()
        {
            // Arrange
            var line = new Line("0,5 -> 4,5");

            // Assert
            Assert.Contains((0, 5), line.Points);
            Assert.Contains((1, 5), line.Points);
            Assert.Contains((2, 5), line.Points);
            Assert.Contains((3, 5), line.Points);
            Assert.Contains((4, 5), line.Points);
            Assert.DoesNotContain((7, 5), line.Points);
            Assert.DoesNotContain((8, 1), line.Points);
        }

        [Fact]
        public void Line_SetPoints_Diagonal()
        {
            // Arrange
            var line = new Line("1,1 -> 3,3");

            // Assert
            Assert.Contains((1, 1), line.Points);
            Assert.Contains((2, 2), line.Points);
            Assert.Contains((3, 3), line.Points);
            Assert.DoesNotContain((1, 2), line.Points);
            Assert.DoesNotContain((1, 3), line.Points);
        }
    }
}
