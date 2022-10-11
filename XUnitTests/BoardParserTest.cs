using System.Linq;
using CrossWord;
using FluentAssertions;
using Xunit;

namespace XUnitTests
{
    public class BoardParserTest
    {
        private const string Path = "C:\\VsGitProjects\\CrossWords\\Data\\";
        private const string FileName1 = Path + "cross2_4.csv";
        private const string FileName2 = Path + "cross3.csv";
        private const string CsvSeparator = ";";

        [Fact]
        public void GetFirstHorizontalPlace()
        {
            var board = new CrossBoard().LoadFromCsv(FileName1, CsvSeparator);
            var place = board.GetNextPlace(Orientation.Horizontal, null);
            place.LineNumber.Should().Be(1);
            place.P.StartIdx.Should().Be(1);
            place.P.Length.Should().Be(7);
            place.CrossingCount.Should().Be(2);
        }

        [Fact]
        public void GetFirstVerticalPlace()
        {
            var board = new CrossBoard().LoadFromCsv(FileName1, CsvSeparator);
            var place = board.GetNextPlace(Orientation.Vertical, null);
            place.LineNumber.Should().Be(2);
            place.P.StartIdx.Should().Be(1);
            place.P.Length.Should().Be(7);
            place.CrossingCount.Should().Be(3);
        }

        [Fact]
        public void CountCrossings()
        {
            var board = new CrossBoard().LoadFromCsv(FileName1, CsvSeparator);
            var verticals = board.GetAllOf(Orientation.Vertical).ToList();
            verticals.Count.Should().Be(24);

            verticals[2].LineNumber.Should().Be(4);
            verticals[2].P.StartIdx.Should().Be(3);
            verticals[2].P.Length.Should().Be(17);
            verticals[2].CrossingCount.Should().Be(7);
        }

        [Fact]
        public void PlaceCompare()
        {
            var board = new CrossBoard().LoadFromCsv(FileName1, CsvSeparator);
            var horizontals = board.GetAllOf(Orientation.Horizontal).ToList();
            var verticals = board.GetAllOf(Orientation.Vertical).ToList();
            horizontals[0].ComparePlaces(verticals[0]).Should().Be(-1);
            horizontals[1].ComparePlaces(verticals[0]).Should().Be(1);
            horizontals[1].ComparePlaces(verticals[8]).Should().Be(0);
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
            var board = new CrossBoard().LoadFromCsv(FileName1, CsvSeparator);
            board.GetAllOf(Orientation.Horizontal).Count().Should().Be(26);
            board.GetAllOf(Orientation.Vertical).Count().Should().Be(24);
            var places = board.GetPlaces();
            places.Count.Should().Be(50);
            places[0].Orientation.Should().Be(Orientation.Vertical);
            places[0].CrossingCount.Should().Be(7);
            places[1].CrossingCount.Should().Be(7);
            places[2].CrossingCount.Should().Be(7);
            places[3].CrossingCount.Should().Be(7);
            places[49].Orientation.Should().Be(Orientation.Horizontal);
            places[49].CrossingCount.Should().Be(2);
            
            var board3 = new CrossBoard().LoadFromCsv(FileName2, CsvSeparator);
            var places3 = board3.GetPlaces();
            places3.Count.Should().Be(21);
            places3[0].Orientation.Should().Be(Orientation.Vertical);
            places3[0].PlaceNumber.Should().Be(1);
            places3[0].CrossingCount.Should().Be(3);
            places3[20].Orientation.Should().Be(Orientation.Horizontal);
            places3[20].PlaceNumber.Should().Be(15);
            places3[20].CrossingCount.Should().Be(1);
        }
    }
}
