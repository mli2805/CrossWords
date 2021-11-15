using System.Collections.Generic;
using System.Linq;

namespace CrossWordFiller
{
    public static class BoardParser
    {
        /// <summary>
        /// после отладки алгоритма составления,
        /// можно будет протестировать различные варианты ранжирования мест для заполнения в списке -
        /// при каком варианте быстрее будет составлять.
        ///
        /// можно кроме числа пересечений учесть колво слов такой длины в словаре,
        /// длину маски (что лучше искать сначала длинные или короткие с большим колво пересечений)
        /// 
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        public static List<Place> GetPlaces(this CrossBoard board)
        {
            var places = board.GetAllOf(Orientation.Horizontal).ToList();
            places.AddRange(board.GetAllOf(Orientation.Vertical));
            return places.OrderByDescending(p => p.CrossingCount).ToList();
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
                    }.CountCrossings(board);
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

        private static Place CountCrossings(this Place place, CrossBoard board)
        {
            for (int i = place.P.StartIdx; i < place.P.StartIdx + place.P.Length; i++)
            {
                if (place.Orientation == Orientation.Horizontal)
                {
                    if (board.Rows[place.LineNumber - 1][i] != '1' || board.Rows[place.LineNumber + 1][i] != '1')
                        place.CrossingCount++;
                }
                else
                {
                    if (board.Rows[i][place.LineNumber - 1] != '1' || board.Rows[i][place.LineNumber + 1] != '1')
                        place.CrossingCount++;
                }
            }

            return place;
        }
    }
}
