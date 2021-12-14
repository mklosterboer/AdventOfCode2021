using AdventOfCode2021.Problems;
using Xunit;

namespace AdventOfCode2021.Test.Day10Tests
{
    public class CalculateScoresTests
    {
        [Theory]
        [InlineData("[)")]
        [InlineData("[}")]
        [InlineData("[>")]

        [InlineData("{]")]
        [InlineData("{>")]
        [InlineData("{)")]

        [InlineData("(}")]
        [InlineData("(]")]
        [InlineData("(>")]

        [InlineData("<}")]
        [InlineData("<)")]
        [InlineData("<]")]

        [InlineData("{([(<{}[<>[]}>{[]{[(<()>")]
        [InlineData("[[<[([]))<([[{}[[()]]]")]
        [InlineData("[{[{({}]{}}([{[{{{}}([]")]
        [InlineData("[<(<(<(<{}))><([]([]()")]
        [InlineData("<{([([[(<>()){}]>(<<{{")]
        public void CalculateScores_FindsCorruptedLines(string testString)
        {
            // Arrange 
            var testLines = new string[1] { testString };

            // Act
            var (partOneScore, partTwoScore) = Day10.CalculateScores(testLines);

            // Assert
            Assert.True(partOneScore > 0);
            Assert.Equal(0, partTwoScore);
        }

        [Theory]
        [InlineData("(")]
        [InlineData("[")]
        [InlineData("<")]
        [InlineData("{")]

        [InlineData("((")]

        [InlineData("[({(<(())[]>[[{[]{<()<>>")]
        [InlineData("[(()[<>])]({[<{<<[]>>(")]
        [InlineData("(((({<>}<{<{<>}{[]{[]{}")]
        [InlineData("{<[[]]>}<{[{[{[]{()[[[]")]
        [InlineData("<{([{{}}[<[[[<>{}]]]>[]]")]
        public void CalculateScores_FindsIncompleteLines(string testString)
        {
            // Arrange 
            var testLines = new string[1] { testString };

            // Act
            var (partOneScore, partTwoScore) = Day10.CalculateScores(testLines);

            // Assert
            Assert.True(partTwoScore > 0);
            Assert.Equal(0, partOneScore);
        }
    }
}
