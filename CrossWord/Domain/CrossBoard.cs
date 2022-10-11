﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CrossWord
{
    public class CrossBoard
    {
        public string[] Rows { get; set; } = Array.Empty<string>();

        public CrossBoard LoadFromCsv(string filename, string csvSeparator)
        {
            var content = File.ReadAllLines(filename);
            Rows = new string[content.Length];
            for (int i = 0; i < content.Length; i++)
            {
                Rows[i] = content[i].Replace(csvSeparator, "");
            }
            return this;
        }

        public void SaveToCsv(string filename, string csvSeparator)
        {
            var content = new List<string>();
            foreach (var row in Rows)
            {
                var sb = new StringBuilder();
                for (int i = 0; i < row.Length - 1; i++)
                {
                    sb = sb.Append(row[i] + csvSeparator);
                }

                sb = sb.Append(row.Last());
                content.Add(sb.ToString());
            }
            File.WriteAllLines(filename, content);
        }

        public string GetMask(Place place)
        {
            var line = place.Orientation == Orientation.Horizontal
                ? Rows[place.LineNumber]
                : Rows.GetColumnAsString(place.P.StartIdx);
            return place.Orientation == Orientation.Horizontal 
                ? line.Substring(place.P.StartIdx, place.P.Length)
                : line.Substring(place.LineNumber, place.P.Length);
        }

        public void FillWordIntoPlace(WordOnBoard word)
        {
            if (word.Place.Orientation == Orientation.Horizontal)
            {
                Rows[word.Place.LineNumber]
                    = Rows[word.Place.LineNumber].Substring(0, word.Place.P.StartIdx)
                      + word.Word.Word
                      + Rows[word.Place.LineNumber].Substring(word.Place.P.StartIdx + word.Word.Word.Length);
            }
            else
            {
                for (int i = 0; i < word.Word.Word.Length; i++)
                {
                    Rows[word.Place.LineNumber + i]
                        = Rows[word.Place.LineNumber + i].Substring(0, word.Place.P.StartIdx)
                          + word.Word.Word[i]
                          + Rows[word.Place.LineNumber + i].Substring(word.Place.P.StartIdx + 1);
                }
            }
        }
    }
}
