using System.Linq;
using CrossWordFiller;
using FluentAssertions;
using Xunit;

namespace XUnitTests
{
    public class BoardParserTest
    {
        private const string Path = "C:\\VsGitProjects\\CrossWords\\CrossWordFiller\\Resources\\";
        private const string FileName1 = Path + "cross2_4.csv";
        private const string FileName2 = Path + "cross3.csv";
        private const char CsvSeparator = ';';

        [Fact]
        public void IndexOfNot()
        {
            var str = "11111111111111111111111111";
            Assert.Equal(-1, str.IndexOfNot('1', 0));
            var str2 = "1101100001111111111111111";
            Assert.Equal(2, str2.IndexOfNot('1', 0));
            Assert.Equal(5, str2.IndexOfNot('1', 3));
        }

        [Fact]
        public void FindFirstPlaceForWord()
        {
            var str = "11111111111111111111111111";
            Assert.Null(str.FindFirstPlaceForWord(0));
            var str2 = "110110АБ01111111111111111";
            str2.FindFirstPlaceForWord(0).Length.Should().Be(4);
            str2.FindFirstPlaceForWord(13).Should().BeNull();
        }

        [Fact]
        public void GetColumnAsString()
        {
            var board = new CrossBoard().LoadFromCsv(FileName1, CsvSeparator);
            board.GetColumnAsString(5).Should().Be("10110110101010101101101");
        }

        [Fact]
        public void GetFirstHorizontalPlace()
        {
            var board = new CrossBoard().LoadFromCsv(FileName1, CsvSeparator);
            var placeInLine = board.GetNextPlace(Orientation.Horizontal, null);
            placeInLine.LineNumber.Should().Be(1);
            placeInLine.P.StartIdx.Should().Be(1);
            placeInLine.P.Length.Should().Be(7);
        }

        [Fact]
        public void GetFirstVerticalPlace()
        {
            var board = new CrossBoard().LoadFromCsv(FileName1, CsvSeparator);
            var placeInLine = board.GetNextPlace(Orientation.Vertical, null);
            placeInLine.LineNumber.Should().Be(2);
            placeInLine.P.StartIdx.Should().Be(1);
            placeInLine.P.Length.Should().Be(7);
        }

        [Fact]
        public void GetAllOf()
        {
            var board = new CrossBoard().LoadFromCsv(FileName2, CsvSeparator);
            var horizontals = board.GetAllOf(Orientation.Horizontal).ToList();
            horizontals.Count.Should().Be(13);

            var verticals = board.GetAllOf(Orientation.Vertical).ToList();
            verticals.Count.Should().Be(8);
        }

        [Fact]
        public void GetPlaces()
        {
            var board = new CrossBoard().LoadFromCsv(FileName2, CsvSeparator);
            var places = board.GetPlaces();
            places.Count.Should().Be(21);
            places[5].Orientation.Should().Be(Orientation.Horizontal);
            places[5].PlaceNumber.Should().Be(4);
            places[19].Orientation.Should().Be(Orientation.Vertical);
            places[19].PlaceNumber.Should().Be(8);
        }
    }
}
