using CrossWordFiller;
using FluentAssertions;
using Xunit;

namespace XUnitTests
{
    public class MakerTest
    {
        private const string Path = "C:\\VsGitProjects\\CrossWords\\Data\\";
        private const string CorpusFileName = Path + "words.txt";
        private const string CsvSeparator = ";";

        [Fact]
        public void Fill2Words17Letters()
        {
            var board = new CrossBoard().LoadFromCsv(Path + "cross2po17.csv", CsvSeparator);
            var corpus = new Corpus().LoadFromTxt(CorpusFileName);
            var result = board.Fill(corpus, null);
            result.Should().BeNull();
        }

        [Fact]
        public void Fill4Words16Letters()
        {
            var board = new CrossBoard().LoadFromCsv(Path + "cross16.csv", CsvSeparator);
            var corpus = new Corpus().LoadFromTxt(CorpusFileName);
            var result = board.Fill(corpus, null);
            if (result != null) 
                board.SaveToCsv(Path + "cross16res.csv", CsvSeparator);
            result.Should().BeNull();
        }

        [Fact]
        public void FillBoard()
        {
            var board = new CrossBoard().LoadFromCsv(Path + "cross2_5.csv", CsvSeparator);
            var corpus = new Corpus().LoadFromTxt(CorpusFileName);
            var result = board.Fill(corpus, null);
            result.Should().BeNull();
        }

        [Fact]
        public void FillBoard3()
        {
            var board = new CrossBoard().LoadFromCsv(Path + "cross3.csv", CsvSeparator);
            var corpus = new Corpus().LoadFromTxt(CorpusFileName);
            var result = board.Fill(corpus, null);
            result.Should().NotBeNull();
        }

        [Fact]
        public void FillBoard4()
        {
            var board = new CrossBoard().LoadFromCsv(Path + "cross5_3.csv", CsvSeparator);
            board.GetPlaces().Count.Should().Be(76);
            var corpus = new Corpus().LoadFromTxt(CorpusFileName);
            var result = board.Fill(corpus, null);
            result.Should().NotBeNull();

            board.SaveToCsv(Path + "cross5_3res.csv", CsvSeparator);
        }
    }
}
