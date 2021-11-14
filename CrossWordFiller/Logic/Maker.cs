using System.Collections.Generic;
using System.Linq;

namespace CrossWordFiller
{
    public static class Maker
    {
        public static List<WordOnBoard> Fill(this CrossBoard board, Corpus corpus)
        {
            var places = board.GetPlaces();
            var result = new List<WordOnBoard>();

            for (int i = 0; i < places.Count; i++)
            {
                var word = new WordInDict() { Mask = board.GetMask(places[i]) };
                if (word.Search(corpus, result.Select(w=>w.Word.Word).ToList()))
                {
                    var wordOnBoard = new WordOnBoard()
                    {
                        Word = word,
                        Place = places[i],
                    };
                    result.Add(wordOnBoard);
                    board.FillWord(wordOnBoard);
                }
            }

            return result;
        }
    }
}
