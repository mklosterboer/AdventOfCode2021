using Xunit;
using static AdventOfCode2021.Problems.Day18;

namespace AdventOfCode2021.Test.Day18Tests
{
    public class SnailFishNumberTests
    {
        [Theory]
        [InlineData("[9,1]", 29)]
        [InlineData("[[1,2],[[3,4],5]]", 143)]
        [InlineData("[[[[0,7],4],[[7,8],[6,0]]],[8,1]]", 1384)]
        [InlineData("[[[[1,1],[2,2]],[3,3]],[4,4]]", 445)]
        [InlineData("[[[[3,0],[5,3]],[4,4]],[5,5]]", 791)]
        [InlineData("[[[[5,0],[7,4]],[5,5]],[6,6]]", 1137)]
        [InlineData("[[[[8,7],[7,7]],[[8,6],[7,7]]],[[[0,7],[6,6]],[8,7]]]", 3488)]
        public void SnailFishNumber_GetMagnitude(string numberValue, int expected)
        {
            // Arrange
            var number = new SnailFishNumber(numberValue);

            // Act
            var actual = number.GetMagnitude();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SnailFishNumber_Add()
        {
            // Arrange
            var first = new SnailFishNumber("[[[0,[4,5]],[0,0]],[[[4,5],[2,6]],[9,5]]]");
            var second = new SnailFishNumber("[7,[[[3,7],[4,3]],[[6,3],[8,8]]]]");
            var third = new SnailFishNumber("[[2,[[0,8],[3,4]]],[[[6,7],1],[7,[1,6]]]]");
            var fourth = new SnailFishNumber("[[[[2,4],7],[6,[0,5]]],[[[6,8],[2,8]],[[2,1],[4,5]]]]");
            var fifth = new SnailFishNumber("[7,[5,[[3,8],[1,4]]]]");

            // Act / Assert
            var firstActual = SnailFishNumber.Add(first, second);
            Assert.Equal("[[[[4,0],[5,4]],[[7,7],[6,0]]],[[8,[7,7]],[[7,9],[5,0]]]]", firstActual.ToString());

            var secondActual = SnailFishNumber.Add(firstActual, third);
            Assert.Equal("[[[[6,7],[6,7]],[[7,7],[0,7]]],[[[8,7],[7,7]],[[8,8],[8,0]]]]", secondActual.ToString());

            var thirdActual = SnailFishNumber.Add(secondActual, fourth);
            Assert.Equal("[[[[7,0],[7,7]],[[7,7],[7,8]]],[[[7,7],[8,8]],[[7,7],[8,7]]]]", thirdActual.ToString());

            var fourthActual = SnailFishNumber.Add(thirdActual, fifth);
            Assert.Equal("[[[[7,7],[7,8]],[[9,5],[8,7]]],[[[6,8],[0,8]],[[9,9],[9,0]]]]", fourthActual.ToString());
        }
    }
}
