namespace CrossWordFiller
{
    public class WordInDict
    {
        public string Word { get; set; }
        public string Mask { get; set; }

        public int StartSearchInDictPos { get; set; }
        public int FoundInDictPos { get; set; }
    }

    public class WordOnBoard
    {
        public WordInDict Word { get; set; }
        public Place Place { get; set; }
    }
}
