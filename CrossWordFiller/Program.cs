using System;

namespace CrossWordFiller
{
    class Program
    {

        private const string Path = "C:\\VsGitProjects\\CrossWords\\CrossWordFiller\\Resources\\";
        private const string FileName = Path + "words.txt";
        private const string FileName2 = Path + "cross2_5.csv";
        private const char CsvSeparator = ';';

        static void Main()
        {
            Console.WriteLine("Hello World!");

            var board = new CrossBoard().LoadFromCsv(FileName2, CsvSeparator);
            var corpus = new Corpus().LoadFromTxt(FileName);
            var _ = board.Fill(corpus);

        }
    }
}
