namespace CrossWord
{
    public class WordSource
    {
        public WordSource(string code, string description)
        {
            Code = code;
            Description = description;
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }

    }
}