namespace CrossWord
{
    public enum Orientation { Horizontal, Vertical }
    public class Place
    {
        public Orientation Orientation { get; set; }
        public int LineNumber { get; set; }
        public PlaceInLine P { get; set; } = new PlaceInLine();
        public int CrossingCount { get; set; }
        public int PlaceNumber { get; set; }

        public Place Clone()
        {
            var clone = (Place)MemberwiseClone();
            clone.P = this.P.Clone();
            return clone;
        }

        public Place GetRotated()
        {
            var newPlace = this.Clone();
            newPlace.LineNumber = this.P.StartIdx;
            newPlace.P.StartIdx = this.LineNumber;
            newPlace.Orientation = this.Orientation == Orientation.Horizontal ? Orientation.Vertical : Orientation.Horizontal;
            return newPlace;
        }
     }

    public class PlaceInLine
    {
        public int StartIdx { get; set; }
        public int Length { get; set; }

        public PlaceInLine Clone()
        {
            return (PlaceInLine)MemberwiseClone();
        }
    }
}
