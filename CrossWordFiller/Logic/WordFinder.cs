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
                if (word.SearchBetween(words, usedWords, word.StartSearchInDictPos, words.Count))
                    return true;

                return word.SearchBetween(words, usedWords, 0, word.StartSearchInDictPos);
            }

            else if (word.StartSearchInDictPos <= word.FoundInDictPos)
            {
                if (word.SearchBetween(words, usedWords, word.FoundInDictPos + 1, words.Count))
                    return true;

                return word.SearchBetween(words, usedWords, 0, word.StartSearchInDictPos);
            }

            // word.FoundInDictPos < word.StartSearchInDictPos
            else return word.SearchBetween(words, usedWords, word.FoundInDictPos + 1, word.StartSearchInDictPos);

        }

        private static bool SearchBetween(this WordInDict word, List<string> words, List<string> usedWords, int start, int finish)
        {
            for (int i = start; i < finish; i++)
            {
                if (!usedWords.Contains(words[i]) && words[i].IsMatch(word.Mask))
                {
                    word.FoundInDictPos = i;
                    word.Word = words[i];
                    return true;
                }
            }

            return false;
        }
    }


}
