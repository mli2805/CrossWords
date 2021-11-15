using CrossWordFiller;
using FluentAssertions;
using Xunit;

namespace XUnitTests
{
    public class StringTest
    {
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
        public void IsMatch()
        {
            "СТАКАН".IsMatch("0А0А00").Should().Be(false);
            "СТАКАН".IsMatch("00А0А0").Should().Be(true);
        }
    }
}
