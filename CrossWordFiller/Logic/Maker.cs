using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CrossWordFiller
{
    public static class Maker
    {
        public static List<WordOnBoard> Fill(this CrossBoard board, Corpus corpus)
        {
            var places = board.GetPlaces();

            var r = new Random();
            var result = new List<WordOnBoard>();
            foreach (var place in places)
            {
                var wordOnBoard = new WordOnBoard()
                {
                    Word = new WordInDict()
                    {
                        StartSearchInDictPos = r.Next(0, corpus.WLists[place.P.Length].Count),
                    },
                    Place = place,
                };
                result.Add(wordOnBoard);
            }

            var file = File.CreateText(@"c:\temp\log.txt");
            file.AutoFlush = true;
            file.WriteLine("Start");


            var currentStep = 0;
            while (currentStep < places.Count)
            {
                var wordInDict = result[currentStep].Word;
                wordInDict.Mask = board.GetMask(places[currentStep]);
                file.WriteLine($"Step {currentStep}: Search: startIdx {wordInDict.StartSearchInDictPos}, previous found pos, {wordInDict.FoundInDictPos} mask {wordInDict.Mask}::{wordInDict.Mask.Length}");
                if (wordInDict.Search(corpus, result.Select(w => w.Word.Word).ToList()))
                {
                    board.FillWordIntoPlace(result[currentStep]);
                    file.WriteLine($"Step {currentStep}: Successfully filled {wordInDict.Word}::{wordInDict.Mask.Length}, found pos {wordInDict.FoundInDictPos}.");
                }
                else
                {
                    file.WriteLine($"Step {currentStep}: Can't find word for mask {wordInDict.Mask}::{wordInDict.Mask.Length}");
                    currentStep--;
                    if (currentStep == -1)
                        break;
                    result[currentStep].Word.Word = result[currentStep].Word.Mask;
                    board.FillWordIntoPlace(result[currentStep]);

                    continue;
                }

                currentStep++;
            }

            file.Close();
            
            return currentStep == places.Count ? result : null;
        }
    }
}
