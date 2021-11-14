using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CrossWordFiller
{
    public class CrossBoard
    {
        public string[] Rows { get; set; }

        public CrossBoard LoadFromCsv(string filename, char csvSeparator)
        {
            var content = File.ReadAllLines(filename);
            Rows = new string[content.Length];
            for (int i = 0; i < content.Length; i++)
            {
                var chars = content[i].Split(csvSeparator);
                var row = string.Concat(chars);
                Rows[i] = row;
            }
            return this;
        }

        public void SaveToCsv(string filename, char csvSeparator)
        {
            var content = new List<string>();
            foreach (var row in Rows)
            {
                var sb = new StringBuilder();
                for (int i = 0; i < row.Length - 1; i++)
                {
                    sb = sb.Append(row[i].ToString() + csvSeparator);
                }

                sb = sb.Append(row.Last());
                content.Add(sb.ToString());
            }
            File.WriteAllLines(filename, content);
        }
    }
}
