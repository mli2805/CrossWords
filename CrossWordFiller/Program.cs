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

        private const string JsonFile = "c:\\VsGitProjects\\CrossWords\\Dictionaries\\harrix.dev\\russian_nouns_with_definition.json";

        static void Main()
        {
            var board = new CrossBoard().LoadFromCsv(BoardFilename, CsvSeparator);

            bool oldCorpus = false;
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            var corpus = oldCorpus
                ? new Corpus().LoadFromTxt(CorpusFilename)
                : new Corpus().LoadHarrixEfremovaJson(JsonFile);

            var r = board.Fill(corpus, null);

            var content = new List<string>();
            foreach (var w in r.Words)
            {
                content.Add($"{w.Place.PlaceNumber} {w.Place.Orientation} {w.Word.Word}");
            }
            File.WriteAllLines(ListFilename, content);



        }
    }
}
