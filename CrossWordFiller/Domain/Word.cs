namespace CrossWordFiller
{
    public class WordInDict
    {
        public string Word { get; set; }
        public string Mask { get; set; }

        public int StartSearchInDictPos { get; set; } = -1;
        public int FoundInDictPos { get; set; } = -1;
    }

    public class WordOnBoard
    {
        public WordInDict Word { get; set; }
        public Place Place { get; set; }
    }
}
