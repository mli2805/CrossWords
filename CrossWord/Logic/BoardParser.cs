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
                .OrderBy(v1 => v1.LineNumber).ThenBy(v2 => v2.P.StartIdx).ToList();

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

        /// <summary>
        /// сравнивает горизонтальное и вертикальное место - не начинаются ли они из одной клетки
        /// для назначения одного и того же номера
        /// </summary>
        /// <param name="horizontal"></param>
        /// <param name="vertical"></param>
        /// <returns></returns>
        public static int ComparePlaces(this Place horizontal, Place vertical)
        {
            if (horizontal.LineNumber < vertical.LineNumber) return -1;
            if (horizontal.LineNumber > vertical.LineNumber) return 1;

            if (horizontal.P.StartIdx < vertical.P.StartIdx) return -1;
            if (horizontal.P.StartIdx > vertical.P.StartIdx) return 1;

            return 0;
        }

        public static IEnumerable<Place> GetAllOf(this CrossBoard board, Orientation orientation)
        {
            var rows = orientation == Orientation.Horizontal
                ? board.Rows
                : board.Rows.RotateRows();

            var place = rows.GetNextPlace( null);
            while (place != null)
            {
                yield return orientation == Orientation.Horizontal ? place : place.GetRotated();
                place = rows.GetNextPlace(place);
            }
        }

        public static Place? GetNextPlace(this string[] rows, Place? lastFound)
        {
            var lineNumber = lastFound?.LineNumber ?? 0;
            var startIndex = lastFound?.P.StartIdx + lastFound?.P.Length ?? 0;

            while (true)
            {
                var line = rows[lineNumber];

                var pl = line.FindFirstPlaceForWord(startIndex);
                if (pl != null)
                {
                    return new Place()
                    {
                        Orientation = Orientation.Horizontal,
                        LineNumber = lineNumber,
                        P = pl,
                    }.CountCrossings(rows);
                }

                var limit = rows.Length - 1;

                if (lineNumber == limit)
                    return null;

                lineNumber++;
                startIndex = 0;
            }
        }

        private static Place CountCrossings(this Place place, string[] rows)
        {
            for (int i = place.P.StartIdx; i < place.P.StartIdx + place.P.Length; i++)
            {
                if (place.Orientation == Orientation.Horizontal)
                {
                    if (rows[place.LineNumber - 1][i] != '1' || rows[place.LineNumber + 1][i] != '1')
                        place.CrossingCount++;
                }
                else
                {
                    if (rows[i][place.LineNumber - 1] != '1' || rows[i][place.LineNumber + 1] != '1')
                        place.CrossingCount++;
                }
            }

            return place;
        }
    }
}
