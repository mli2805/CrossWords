using CrossWordFiller;
using FluentAssertions;
using Xunit;

namespace XUnitTests
{
    public class MakerTest
    {
        private const string Path = "C:\\VsGitProjects\\CrossWords\\CrossWordFiller\\Resources\\";
        private const string FileName = Path + "words.txt";
        private const string FileName2 = Path + "cross3.csv";
        private const string FileName3 = Path + "cross3res.csv";
        private const char CsvSeparator = ';';

        [Fact]
        public void FillBoard()
        {
            var board = new CrossBoard().LoadFromCsv(FileName2, CsvSeparator);
            var corpus = new Corpus().LoadFromTxt(FileName);

            var places = board.GetPlaces();
            board.GetMask(places[0]).Should().Be("0000000");
            board.GetMask(places[1]).Should().Be("00000");

            var wordOnBoard = new WordOnBoard()
            {
                Place = places[0],
                Word = new WordInDict() {Word = "АНТЕННА"}
            };
            board.FillWord(wordOnBoard);
            board.GetMask(places[0]).Should().Be("АНТЕННА");
            board.GetMask(places[1]).Should().Be("А0000");
            var wordOnBoardV = new WordOnBoard()
            {
                Place = places[1],
                Word = new WordInDict() {Word = "АББАТ"}
            };
            board.FillWord(wordOnBoardV);
            board.GetMask(places[1]).Should().Be("АББАТ");


            var board2 = new CrossBoard().LoadFromCsv(FileName2, CsvSeparator);
            var result = board2.Fill(corpus);
            result.Should().NotBeNull();
            result.Count.Should().Be(21);

            board2.SaveToCsv(FileName3, CsvSeparator);
        }
    }
}
