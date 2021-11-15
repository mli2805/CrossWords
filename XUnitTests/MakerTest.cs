using CrossWordFiller;
using FluentAssertions;
using Xunit;

namespace XUnitTests
{
    public class MakerTest
    {
        private const string Path = "C:\\VsGitProjects\\CrossWords\\CrossWordFiller\\Resources\\";
        private const string FileName = Path + "words.txt";
        private const string FileName2 = Path + "cross2_5.csv";
        private const string FileName3 = Path + "cross3.csv";
        private const char CsvSeparator = ';';

        [Fact]
        public void FillWord()
        {
            var board = new CrossBoard().LoadFromCsv(FileName3, CsvSeparator);
            var places = board.GetPlaces();
            board.GetMask(places[0]).Should().Be("00000");
            board.GetMask(places[1]).Should().Be("00000");

            var wordOnBoard = new WordOnBoard()
            {
                Place = places[0],
                Word = new WordInDict() {Word = "ТОПОР"}
            };
            board.FillWordIntoPlace(wordOnBoard);
            board.GetMask(places[0]).Should().Be("ТОПОР");
            board.GetMask(places[8]).Should().Be("Т000000");
        }

        [Fact]
        public void FillBoard()
        {
            var board = new CrossBoard().LoadFromCsv(FileName2, CsvSeparator);
            var corpus = new Corpus().LoadFromTxt(FileName);
            var result = board.Fill(corpus);
            result.Should().BeNull();
        }
    }
}
