using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CrossWordFiller
{
    public static class Maker
    {
        public static List<WordOnBoard> Fill(this CrossBoard board, Corpus corpus, List<string> log)
        {
            var result = board.GetPlaces().InitializeWordsOnBoard(corpus);

            var file = File.CreateText(@"c:\temp\log.txt");
            file.AutoFlush = true;
            file.WriteLine("Start");

            var currentStep = 0;
            while (currentStep < board.GetPlaces().Count)
            {
                var wordInDict = result[currentStep].Word;
                wordInDict.Mask = board.GetMask(board.GetPlaces()[currentStep]);
                file.LogSearch(currentStep, wordInDict);

                if (wordInDict.Search(corpus, result.Select(w => w.Word.Word).ToList()))
                {
                    board.FillWordIntoPlace(result[currentStep]);
                    file.LogFilled(currentStep, wordInDict);
                    currentStep++;
                }
                else
                {
                    file.LogCantFind(currentStep, wordInDict);
                    var problemMask = result[currentStep].Word.Mask;
                    if (problemMask.IndexOfNot('0', 0) == -1)
                    {
                        log.Add("Impossible to complete !!!");
                        break;
                    }

                    var problemPlace = board.GetPlaces()[currentStep];

                    while (board.GetMask(problemPlace) == problemMask)
                    {
                        result[currentStep].Word.FoundInDictPos = -1;
                        currentStep--;

                        if (currentStep == -1)
                            break;

                        result[currentStep].Word.Word = result[currentStep].Word.Mask;
                        board.FillWordIntoPlace(result[currentStep]);
                    }
                }
            }

            file.Close();

            return currentStep == board.GetPlaces().Count ? result : null;
        }

        private static List<WordOnBoard> InitializeWordsOnBoard(this List<Place> places, Corpus corpus)
        {
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

            return result;
        }

        private static void LogSearch(this StreamWriter file, int currentStep, WordInDict wordInDict)
        {
            file.WriteLine($"Step {currentStep}: Search: startIdx {wordInDict.StartSearchInDictPos}, previous found pos {wordInDict.FoundInDictPos}, mask {wordInDict.Mask}");
        }

        private static void LogFilled(this StreamWriter file, int currentStep, WordInDict wordInDict)
        {
            file.WriteLine($"Step {currentStep}: Successfully filled {wordInDict.Word}, found pos {wordInDict.FoundInDictPos}.");
        }

        private static void LogCantFind(this StreamWriter file, int currentStep, WordInDict wordInDict)
        {
            file.WriteLine($"Step {currentStep}: Can't find word for mask {wordInDict.Mask}::{wordInDict.Mask.Length}");
        }


    }
}
