using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CrossWord
{
    public class Corpus
    {
        public List<string>[] WLists { get; private set; } = Array.Empty<List<string>>();

        public Corpus LoadFromTxt(string filename)
        {
            var content = File.ReadAllLines(filename);
            Calibrate(content.ToList());
            return this;
        }

        public Corpus LoadHarrixEfremovaJson(string jsonFile)
        {
            var jsonString = File.ReadAllText(jsonFile);
            var words = HarrixEfremovaDictionary.FromJson(jsonString)?.Keys.ToList() ?? new List<string>();
            Calibrate(words);
            return this;
        }

        private void Calibrate(List<string> content)
        {
            const int maxLength = 25;
            WLists = new List<string>[maxLength + 1];
            for (int i = 2; i <= maxLength; i++)
            {
                WLists[i] = content.Where(w => w.Length == i).ToList();
            }
        }
    }
}
