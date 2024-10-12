using System.ComponentModel;

namespace GameEngine.GameData;

public class Word : IIdentifiable
{
    public int Id { get; set; }

    public List<string> Aliases { get; set; }

    public string Name { get; set; }
    public WordTypes WordType { get; set; }

    public Word()
    {
        Aliases = new List<string>();
        Name = string.Empty;
    }
}

public enum WordTypes
{
    Noun = 1,
    Verb = 2
}