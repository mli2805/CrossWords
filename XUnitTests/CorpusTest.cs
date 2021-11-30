using System.Collections.Generic;
using CrossWordFiller;
using FluentAssertions;
using Xunit;

namespace XUnitTests
{
    public class CorpusTest
    {
        private const string Path = "C:\\VsGitProjects\\CrossWords\\CrossWordFiller\\Resources\\";
        private const string FileName = Path + "words.txt";

        [Fact]
        public void LoadCorpus()
        {
            var words = new Corpus().LoadFromTxt(FileName);
            words.WLists[2].Count.Should().Be(22);
            words.WLists[3].Count.Should().Be(266);
            words.WLists[6].Count.Should().Be(1653);
            words.WLists[9].Count.Should().Be(855);
            words.WLists[13].Count.Should().Be(88);
            words.WLists[15].Count.Should().Be(14);
            words.WLists[16].Count.Should().Be(11);
            words.WLists[17].Count.Should().Be(1);
            words.WLists[18].Count.Should().Be(1);
            words.WLists[19].Count.Should().Be(1);
            words.WLists[20].Count.Should().Be(0);
            words.WLists[21].Count.Should().Be(0);
        }

        [Fact]
        public void SearchWord()
        {
            var words = new Corpus().LoadFromTxt(FileName);
            var wordInDict = new WordInDict()
            {
                Mask = "00А0А0",
                StartSearchInDictPos = 519,
                FoundInDictPos = -1,
            };
            var empty = new List<string>();

            wordInDict.Search(words, empty).Should().BeTrue();
            wordInDict.Word.Should().Be("КЛАПАН");

            wordInDict.Search(words, empty).Should().BeTrue();
            wordInDict.Word.Should().Be("ПЛАКАТ");

            for (int i = 0; i < 10; i++)
            {
                wordInDict.Search(words, empty).Should().BeTrue();
            }
            wordInDict.Word.Should().Be("ЕРАЛАШ");

            wordInDict.Search(words, empty).Should().BeFalse();
        }
    }
}
