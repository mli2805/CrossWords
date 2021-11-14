using CrossWordFiller;
using FluentAssertions;
using Xunit;

namespace XUnitTests
{
    public class WordsTest
    {
        private const string Path = "C:\\VsGitProjects\\CrossWords\\CrossWordFiller\\Resources\\";
        private const string FileName = Path + "words.txt";

        [Fact]
        public void LoadWords()
        {
            var words = new Words().LoadFromTxt(FileName);
            words.WLists[6].Count.Should().Be(1653);
        }

        [Fact]
        public void IsMatch()
        {
            "СТАКАН".IsMatch("0А0А00").Should().Be(false);
            "СТАКАН".IsMatch("00А0А0").Should().Be(true);
        }

        [Fact]
        public void SearchWord()
        {
            var words = new Words().LoadFromTxt(FileName);
            var wordInDict = new WordInDict()
            {
                Mask = "00А0А0",
                StartSearchInDictPos = 89,
                FoundInDictPos = 430,
            };
            wordInDict.Search(words).Should().BeTrue();
            wordInDict.Word.Should().Be("КЛАПАН");

            wordInDict.Search(words).Should().BeTrue();
            wordInDict.Word.Should().Be("ПЛАКАТ");

            wordInDict.FoundInDictPos = 1651;
            wordInDict.Search(words).Should().BeTrue();
            wordInDict.Word.Should().Be("АНАНАС");

            wordInDict.Search(words).Should().BeTrue();
            wordInDict.Word.Should().Be("АТАМАН");

            wordInDict.Search(words).Should().BeFalse();
        }

    }
}
