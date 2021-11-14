using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CrossWordFiller
{
    public class Corpus
    {
        public List<string>[] WLists { get; private set; }

        public Corpus LoadFromTxt(string filename)
        {
            const int maxLength = 25;
            WLists = new List<string>[maxLength + 1];

            var content = File.ReadAllLines(filename);
            for (int i = 2; i <= maxLength; i++)
            {
                WLists[i] = content.Where(w => w.Length == i).ToList();
            }

            return this;
        }
    }
}
