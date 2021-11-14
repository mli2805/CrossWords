namespace CrossWordFiller
{
    public static class WordFinder
    {
        public static bool Search(this WordInDict word, Words fullDict)
        {
            var words = fullDict.WLists[word.Mask.Length];

            if (word.StartSearchInDictPos <= word.FoundInDictPos)
            {
                for (int i = word.FoundInDictPos+1; i < words.Count; i++)
                {
                    if (words[i].IsMatch(word.Mask))
                    {
                        word.FoundInDictPos = i;
                        word.Word = words[i];
                        return true;
                    }
                }

                for (int i = 0; i < word.StartSearchInDictPos; i++)
                {
                    if (words[i].IsMatch(word.Mask))
                    {
                        word.FoundInDictPos = i;
                        word.Word = words[i];
                        return true;
                    }
                }
            }
            else
            {
                for (int i = word.FoundInDictPos+1; i < word.StartSearchInDictPos; i++)
                {
                    if (words[i].IsMatch(word.Mask))
                    {
                        word.FoundInDictPos = i;
                        word.Word = words[i];
                        return true;
                    }
                }
            }

            
            return false;
        }
    }
}
