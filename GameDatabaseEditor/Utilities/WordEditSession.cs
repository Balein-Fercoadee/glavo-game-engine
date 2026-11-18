using System.Collections.ObjectModel;
using GameEngine.GameData;

namespace GameDatabaseEditor.Utilities;

internal sealed class WordEditSession
{
    private readonly ObservableCollection<Word> _words;
    private readonly Word _word;

    public Word Draft { get; }

    public WordEditSession(ObservableCollection<Word> words, Word word)
    {
        _words = words;
        _word = word;
        Draft = new Word
        {
            Id = word.Id,
            Name = word.Name,
            WordType = word.WordType,
            Aliases = new List<string>(word.Aliases)
        };
    }

    public bool Save()
    {
        int index = _words.IndexOf(_word);
        if (index < 0)
        {
            return false;
        }

        _words[index] = Draft;
        return true;
    }
}
