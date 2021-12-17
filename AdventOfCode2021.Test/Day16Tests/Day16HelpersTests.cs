using AdventOfCode2021.Problems;
using Xunit;

namespace AdventOfCode2021.Test.Day16Tests
{
    public class Day16HelpersTests
    {
        [Theory]
        [InlineData("D2FE28", "110100101111111000101000")]
        [InlineData("38006F45291200", "00111000000000000110111101000101001010010001001000000000")]
        public void HexToBinary_TranslatesStrings(string input, string expected)
        {
            // Act
            var actual = Day16Helpers.HexToBinary(input);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
