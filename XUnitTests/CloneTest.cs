using CrossWord;
using FluentAssertions;
using Xunit;

namespace XUnitTests
{
    public class CloneTest
    {
        [Fact]
        public void GetCloneTest()
        {
            var place = new Place()
            {
                LineNumber = 7, P = new PlaceInLine() { Length = 4, StartIdx = 3 }
            };
            var place2 = place.Clone();
            place2.P.StartIdx.Should().Be(3);

            place2.P.StartIdx = 5;
            place.P.StartIdx.Should().Be(3);
        }
    }
}
