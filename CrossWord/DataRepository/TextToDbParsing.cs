namespace CrossWord
{
    public static class TextToDbParsing
    {
        public static DbWord OzhegovToDbWord(string s, string sourceCode)
        {
            var word = new DbWord();
            word.TheWord = s.Trim().ToLowerInvariant();
            word.Source = sourceCode;
            return word;
        }

        //  https://harrix.dev/blog/2018/russian-nouns/
        public static DbWord EfremovaToDbWord(string s, string sourceCode)
        {
            var ind = s.IndexOf(' ');

            var word = new DbWord();
            word.TheWord = s.Substring(0, ind - 1);
            word.Source = sourceCode;
            word.Description = s.Substring(ind + 1);

            return word;
        }

        public static DbWord GeographyToDbWord(string s, string sourceCode)
        {
            var word = new DbWord();
            var geo = s.Trim().ToLowerInvariant();
            var cleanForm = geo
                .Replace(" ", "")
                .Replace("'", "")
                .Replace("-", "");
            if (geo == cleanForm)
            {
                word.TheWord = geo;
            }
            else
            {
                word.TheWord = cleanForm;
                word.AnotherForm = geo;
                word.Level = 3;
            }
            word.Source = sourceCode;
            word.Description = "Одна из высочайших вершин мира";
            return word;
        }
    }
}