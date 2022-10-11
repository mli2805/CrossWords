using System.Collections.Generic;
using Newtonsoft.Json;

namespace CrossWord
{
    // есть множ число, несколько имен собственных и слова переведенные из других частей речи
    // идея в том, чтобы использовать как есть, а затем реальзовать интерфейс для удобной отбраковки слов

    //    private const string JsonFile = "c:\\VsGitProjects\\CrossWords\\Dictionaries\\harrix.dev\\russian_nouns_with_definition.json";
    //    var jsonString = File.ReadAllText(JsonFile);
    //    var words = HarrixEfremovaDictionary.FromJson(jsonString).Keys.ToList();

    public class HarrixEfremovaDictionary
    {
        [JsonProperty("definition")]
        public string? Definition { get; set; }

        public static Dictionary<string, HarrixEfremovaDictionary>? FromJson(string json) 
            => JsonConvert.DeserializeObject<Dictionary<string, HarrixEfremovaDictionary>>(json);
    }
}
