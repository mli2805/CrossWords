using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrossWord
{
    public static class BoardExt
    {
        public static IEnumerable<string> Wrap(this IEnumerable<string> rows)
        {
            var list = rows.ToList();
            var str = new string('1', list.First().Length + 2);
            yield return str;
            foreach (var st in list)
            {
                yield return "1" + st + "1";
            }
            yield return str;
        }

        public static IEnumerable<string> Trim(this IEnumerable<string> rows)
        {
            return rows
                .Where(row => row.Contains('0'))
                .ToArray().RotateRows()
                .Where(col => col.Contains('0'))
                .ToArray().RotateRows();
        }

      
        public static IEnumerable<string> ToStrings(this char[,] board)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                var sb = new StringBuilder();
                for (int i = 0; i < board.GetLength(0); i++)
                {
                    sb.Append(board[i, j]);
                }
                yield return sb.ToString();
            }
        }

        public static char[,] ToFullBoard(this bool[,] quarter)
        {
            var width = quarter.GetLength(0) * 2;
            var height = quarter.GetLength(1) * 2;
            var full = new char[width, height];

            for (int j = 0; j < quarter.GetLength(1); j++)
            {
                for (int i = 0; i < quarter.GetLength(0); i++)
                {
                    var c = quarter[i, j] ? '0' : '1';
                    full[i, j] = c;
                    full[width - i - 1, j] = c;
                    full[i, height - j - 1] = c;
                    full[width - i - 1, height - j - 1] = c;
                }
            }

            return full;
        }
    }
}
