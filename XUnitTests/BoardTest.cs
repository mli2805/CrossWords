using CrossWordFiller;
using FluentAssertions;
using Xunit;

namespace XUnitTests
{
    public class BoardTest
    {
        private const string Path = "C:\\VsGitProjects\\CrossWords\\CrossWordFiller\\Resources\\";
        private const string FileName1 = Path + "cross2_4.csv";
        private const string FileName2 = Path + "cross2_5.csv";
        private const string CsvSeparator = ";";

        [Fact]
        public void LoadFromCsv()
        {
            var board = new CrossBoard().LoadFromCsv(FileName1, CsvSeparator);
            Assert.Equal(23, board.Rows.Length);
            Assert.Equal(25, board.Rows[0].Length);
            Assert.Equal('1', board.Rows[3][7]);
        }

        [Fact]
        public void LoadSaveLoad()
        {
            var board = new CrossBoard().LoadFromCsv(FileName1, CsvSeparator);
            board.SaveToCsv(FileName2, CsvSeparator);
            var board2 = new CrossBoard().LoadFromCsv(FileName2, CsvSeparator);
            Assert.Equal(23, board2.Rows.Length);
            Assert.Equal(25, board2.Rows[0].Length);
            Assert.Equal('1', board2.Rows[3][7]);
        }

        [Fact]
        public void GetColumnAsString()
        {
            var board = new CrossBoard().LoadFromCsv(FileName1, CsvSeparator);
            board.GetColumnAsString(5).Should().Be("10110110101010101101101");
        }

        [Fact]
        public void FillWord()
        {
            var board = new CrossBoard().LoadFromCsv(Path + "cross3.csv", CsvSeparator);
            var places = board.GetPlaces();
            board.GetMask(places[0]).Should().Be("00000");
            board.GetMask(places[1]).Should().Be("00000");

            var wordOnBoard = new WordOnBoard()
            {
                Place = places[0],
                Word = new WordInDict() { Word = "рнонп" }
            };
            board.FillWordIntoPlace(wordOnBoard);
            board.GetMask(places[0]).Should().Be("рнонп");
            board.GetMask(places[8]).Should().Be("р000000");
        }

    }
}
