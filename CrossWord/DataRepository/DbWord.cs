namespace CrossWord
{
    public class DbWord
    {
        public int Id { get; set; }
        public string? Source { get; set; } // из какого словаря взят
        public int Level { get; set; } // если слово не нравится ему можно поставить низкий уровень, чтобы использовать только в крайнем случае
        public string TheWord { get; set; } = "";
        public string? AnotherForm { get; set; }
        public string? Description { get; set; }
    }
}