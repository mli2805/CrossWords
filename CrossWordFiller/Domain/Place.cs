namespace CrossWordFiller
{
    public enum Orientation { Horizontal, Vertical }
    public class Place
    {
        public Orientation Orientation { get; set; }
        public int LineNumber { get; set; }
        public PlaceInLine P { get; set; }
        public int CrossingCount { get; set; }
        public int PlaceNumber { get; set; }
     }

    public class PlaceInLine
    {
        public int StartIdx { get; set; }
        public int Length { get; set; }
    }
}
