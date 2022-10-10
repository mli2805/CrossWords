using System;
using System.Collections.Generic;
using System.IO;

namespace CrossWordFiller
{
    class Program
    {
        private const string Path = "C:\\VsGitProjects\\CrossWords\\Data\\";
        private const string CorpusFilename = Path + "words.txt";
        private const string BoardFilename = Path + "cross5_2.csv";
        private const string ListFilename = Path + "cross5_2.lst";
        private const string CsvSeparator = ";";

        static void Main()
        {
            Console.WriteLine("Hello World!");

            var board = new CrossBoard().LoadFromCsv(BoardFilename, CsvSeparator);
            var corpus = new Corpus().LoadFromTxt(CorpusFilename);
            var log = new List<string>();
            var r = board.Fill(corpus, log);

            var content = new List<string>();
            foreach (var w in r)
            {
                content.Add($"{w.Place.PlaceNumber} {w.Place.Orientation} {w.Word.Word}");
            }
            File.WriteAllLines(ListFilename, content);
        }
    }
}
