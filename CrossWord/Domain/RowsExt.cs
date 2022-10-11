using System.Linq;

namespace CrossWord
{
    public static class RowsExt
    {
        public static string GetColumnAsString(this string[] rows, int number)
        {
            return string.Concat(rows.Select(r => r[number]));
        }

        /// <summary>
        /// повернуть чтобы столбцы стали строками
        /// </summary>
        /// <returns></returns>
        public static string[] RotateRows(this string[] rows)
        {
            return Enumerable.Range(0, rows[0].Length).Select(i => string.Concat(rows.Select(r => r[i]))).ToArray();
        }

        /// <summary>
        /// отразить строки вниз относительно горизонтальной оси
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="isAxisLineIncluded">отражать ли нижнюю строку</param>
        /// <returns></returns>
        public static string[] TurnV(this string[] rows, bool isAxisLineIncluded)
        {
            var s = isAxisLineIncluded ? rows.Length : rows.Length - 1;
            return Enumerable.Range(s, 0).Select(i => rows[i]).ToArray();
        }

        /// <summary>
        /// отразить строки вправо относительно вертикальной оси
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="isAxisLineIncluded">отражать ли правый столбец</param>
        /// <returns></returns>
        public static string[] TurnH(this string[] rows, bool isAxisLineIncluded)
        {
            string GetLine(string s2)
            {
                return isAxisLineIncluded ? s2 : s2.Substring(s2.Length - 1);
            }

            return Enumerable.Range(0, rows.Length).Select(i => GetLine(rows[i]).Reverse()).ToArray();
        }
    }
}