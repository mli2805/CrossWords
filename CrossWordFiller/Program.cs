using System;
using System.Collections.Generic;

namespace CrossWordFiller
{
    class Program
    {

        private const string Path = "C:\\VsGitProjects\\CrossWords\\CrossWordFiller\\Resources\\";
        private const string CorpusFilename = Path + "words.txt";
        private const string BoardFilename = Path + "cross16.csv";
        private const char CsvSeparator = ';';

        static void Main()
        {
            Console.WriteLine("Hello World!");

            var board = new CrossBoard().LoadFromCsv(BoardFilename, CsvSeparator);
            var corpus = new Corpus().LoadFromTxt(CorpusFilename);
            var log = new List<string>();
            var _ = board.Fill(corpus, log);

        }
    }
}
