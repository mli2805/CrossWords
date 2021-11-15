using System.Collections.Generic;

namespace CrossWordFiller
{
    public static class WordFinder
    {
        public static bool Search(this WordInDict word, Corpus fullDict, List<string> usedWords)
        {
            var words = fullDict.WLists[word.Mask.Length];

            if (word.FoundInDictPos == -1)
            {
                for (int i = word.StartSearchInDictPos; i < words.Count; i++)
                {
                    if (!usedWords.Contains(words[i]) && words[i].IsMatch(word.Mask))
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
            else if (word.StartSearchInDictPos <= word.FoundInDictPos)
            {
                for (int i = word.FoundInDictPos+1; i < words.Count; i++)
                {
                    if (!usedWords.Contains(words[i]) && words[i].IsMatch(word.Mask))
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
            else // word.FoundInDictPos < word.StartSearchInDictPos
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
