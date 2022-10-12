using System;
using System.Collections.Generic;
using System.Linq;

namespace CrossWord
{
    public static class ReadBoardExt
    {
        public static bool[,] ToBool(this string[] rows, int placesInQuarter)
        {
            var result = new bool[placesInQuarter, placesInQuarter];

            var j = placesInQuarter - 1;
            foreach (var row in rows.Reverse())
            {
                var chars = row.ToCharArray().Reverse();
                var i = placesInQuarter - 1;
                foreach (var c in chars)
                {
                    result[i--, j] = c != '1';
                }

                j--;
            }

            return result;
        }

        public static IEnumerable<string> GetQuarterWithoutSeparator(this string[] rows, char separator)
        {
            var columns = rows[0].Length / 2 + 1; // всегда нечетное, .5 отбрасывает при делении
            var m = (columns - 1) / 2;
            // пропускаем 0ю (окружающую) строку единиц
            for (int j = 1; j < Math.Round(rows.Length / 2.0, MidpointRounding.AwayFromZero); j++)
                yield return rows[j]
                    .Replace(separator.ToString(), "")
                    // отбрасываем 0й (окружающий) столбец единиц
                    .Substring(1, m);
        }
    }
}
