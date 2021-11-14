using System.Collections.Generic;
using System.Linq;

namespace CrossWordFiller
{
    public static class BoardParser
    {
        /// <summary>
        /// не самый лучший способ выбрать порядок заполнения
        /// лучше наверное упорядочить слова по количеству пересечений, от большего к меньшему
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        public static List<Place> GetPlaces(this CrossBoard board)
        {
            var horizontals = board.GetAllOf(Orientation.Horizontal).ToList();
            var verticals = board.GetAllOf(Orientation.Vertical).ToList();

            var hCounter = horizontals.Count;
            var vCounter = verticals.Count;
            var coeff = (double) vCounter / hCounter + 0.01;

            var places = new List<Place>();
            while (hCounter + vCounter > 0)
            {
                places.Add(
                    hCounter * coeff > vCounter 
                        ? horizontals[^hCounter--] 
                        : verticals[^vCounter--]);
            }

            return places;
        }

        public static IEnumerable<Place> GetAllOf(this CrossBoard board, Orientation orientation)
        {
            var counter = 1;
            var place = board.GetNextPlace(orientation, null);
            while (place != null)
            {
                place.PlaceNumber = counter++;
                yield return place;
                place = board.GetNextPlace(orientation, place);
            }
        }

        public static Place GetNextPlace(this CrossBoard board, Orientation orientation, Place lastFound)
        {
            var lineNumber = lastFound?.LineNumber ?? 0;
            var startIndex = lastFound?.P.StartIdx + lastFound?.P.Length ?? 0;
          
            while (true)
            {
                var line = orientation == Orientation.Horizontal
                    ? board.Rows[lineNumber]
                    : board.GetColumnAsString(lineNumber);

                var pl = line.FindFirstPlaceForWord(startIndex);
                if (pl != null)
                {
                    return new Place()
                    {
                        Orientation = orientation,
                        LineNumber = lineNumber,
                        P = pl,
                    };
                }

                var limit = orientation == Orientation.Horizontal
                    ? board.Rows.Length - 1
                    : board.Rows[0].Length - 1;

                if (lineNumber == limit)
                    return null;

                lineNumber++;
                startIndex = 0;
            }
        }

        public static string GetColumnAsString(this CrossBoard board, int number)
        {
            return string.Concat(board.Rows.Select(r=>r[number]));
        }

        public static PlaceInLine FindFirstPlaceForWord(this string str, int startIndex)
        {
            var open = -1;
            for (int i = startIndex; i < str.Length; i++)
            {
                if (open == -1 && str[i] != '1')
                    open = i;

                if (open != -1 && str[i] == '1')
                {
                    if (i - open < 2)
                    {
                        open = -1;
                        continue;
                    }

                    return new PlaceInLine()
                    {
                        StartIdx = open,
                        Length = i - open,
                    };
                }
            }

            return null;
        }

        public static int IndexOfNot(this string str, char ch, int startIndex)
        {
            for (int i = startIndex; i < str.Length; i++)
            {
                if (str[i] != ch)
                    return i;
            }

            return -1;
        }
    }
}
