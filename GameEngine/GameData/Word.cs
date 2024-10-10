namespace GameEngine.GameData;

public class Word
{
    public WordTypes WordType { get; set; }
}

public enum WordTypes
{
    Noun = 1,
    Verb = 2
}