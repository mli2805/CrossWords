namespace CrossWordFiller
{
    public class WordOnPlace
    {
        public Place Place { get; set; }
        public string Mask { get; set; }

        public int StartSearchInDictPos { get; set; }
        public int FoundInDictPos { get; set; }

    }

    public enum Orientation { Horizontal, Vertical }
    public class Place
    {
        public Orientation Orientation { get; set; }
        public int LineNumber { get; set; }
        public PlaceInLine P { get; set; }
        public int PlaceNumber { get; set; }
     }

    public class PlaceInLine
    {
        public int StartIdx { get; set; }
        public int Length { get; set; }

    }
}
