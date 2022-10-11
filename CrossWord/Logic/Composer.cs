using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace CrossWord
{
    public class ComposerResult
    {
        public List<WordOnBoard>? Words;
        public bool IsCanceled;
        public bool IsFailed;
    }

    public static class Composer
    {
        public static ComposerResult Fill(this CrossBoard board, Corpus corpus, BackgroundWorker worker)
        {
            var result = new ComposerResult() {
                Words = board.GetPlaces().InitializeWordsOnBoard(corpus),
            };

            var logFile = File.CreateText(@"c:\temp\log.txt");
            logFile.AutoFlush = true;
            logFile.WriteLine("Start");

            var currentStep = 0;
            while (currentStep < board.GetPlaces().Count)
            {
                if (worker.CancellationPending)
                {
                    result.IsCanceled = true;
                    break; 
                }

                var wordInDict = result.Words[currentStep].Word;
                wordInDict.Mask = board.GetMask(board.GetPlaces()[currentStep]);
                logFile.LogSearch(currentStep, wordInDict);

                if (wordInDict.Search(corpus, result.Words.Select(w => w.Word.Word).ToList()))
                {
                    board.FillWordIntoPlace(result.Words[currentStep]);
                    logFile.LogFilled(currentStep, wordInDict);
                    currentStep++;
                }
                else
                {
                    logFile.LogCantFind(currentStep, wordInDict);
                    var problemMask = result.Words[currentStep].Word.Mask;
                    if (problemMask.IndexOfNot('0', 0) == -1)
                    {
                        result.IsFailed = true;
                        break;
                    }

                    var problemPlace = board.GetPlaces()[currentStep];

                    while (board.GetMask(problemPlace) == problemMask)
                    {
                        result.Words[currentStep].Word.FoundInDictPos = -1;
                        currentStep--;

                        if (currentStep == -1)
                            break;

                        result.Words[currentStep].Word.Word = result.Words[currentStep].Word.Mask;
                        board.FillWordIntoPlace(result.Words[currentStep]);
                    }
                }

                worker.ReportProgress(currentStep);
            }

            logFile.Close();
            return result;
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
