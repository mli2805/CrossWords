using System.Collections.Generic;
using System.Linq;

namespace CrossWord
{
    public static class BoardParser
    {
        /// <summary>
        /// 
        /// 
        /// после отладки алгоритма составления,
        /// можно будет протестировать различные варианты ранжирования мест для заполнения в списке -
        /// при каком варианте быстрее будет составлять.
        ///
        /// можно кроме числа пересечений учесть колво слов такой длины в словаре,
        /// длину маски (что лучше искать сначала длинные или короткие с большим колво пересечений)
        /// 
        /// </summary>
        /// <param name="board"> массив 0/1, где 0 - место для буквы, 1 - фон</param>
        /// <returns></returns>
        public static List<Place> GetPlaces(this CrossBoard board)
        {
            var horizontals = board.GetAllOf(Orientation.Horizontal).ToList();
            var verticals = board.GetAllOf(Orientation.Vertical)
                .OrderBy(v1 => v1.P.StartIdx).ThenBy(v2 => v2.LineNumber).ToList();

            return SetPlaceNumbers(horizontals, verticals).OrderByDescending(p => p.CrossingCount).ToList();
        }

        private static IEnumerable<Place> SetPlaceNumbers(List<Place> horizontals, List<Place> verticals)
        {
            var counter = 1;
            var h = 0;
            var v = 0;
            while (h < horizontals.Count || v < verticals.Count)
            {
                var compareRes = h == horizontals.Count
                    ? 1
                    : v == verticals.Count
                        ? -1
                        : ComparePlaces(horizontals[h], verticals[v]);
                switch (compareRes)
                {
                    case -1:
                        horizontals[h].PlaceNumber = counter;
                        yield return horizontals[h];
                        h++;
                        break;
                    case 0:
                        horizontals[h].PlaceNumber = counter;
                        yield return horizontals[h];
                        h++;
                        verticals[v].PlaceNumber = counter;
                        yield return verticals[v];
                        v++;
                        break;
                    case 1:
                        verticals[v].PlaceNumber = counter;
                        yield return verticals[v];
                        v++;
                        break;
                }

                counter++;
            }
        }

        public static int ComparePlaces(this Place horizontal, Place vertical)
        {
            if (horizontal.LineNumber < vertical.P.StartIdx) return -1;
            if (horizontal.LineNumber > vertical.P.StartIdx) return 1;

            if (horizontal.P.StartIdx < vertical.LineNumber) return -1;
            if (horizontal.P.StartIdx > vertical.LineNumber) return 1;

            return 0;
        }

        public static IEnumerable<Place> GetAllOf(this CrossBoard board, Orientation orientation)
        {
            var place = board.GetNextPlace(orientation, null);
            while (place != null)
            {
                yield return place;
                place = board.GetNextPlace(orientation, place);
            }
        }

        public static Place? GetNextPlace(this CrossBoard board, Orientation orientation, Place lastFound)
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
