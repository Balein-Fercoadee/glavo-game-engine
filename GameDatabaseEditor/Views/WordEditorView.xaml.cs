using GameEngine.GameData;

namespace GameDatabaseEditor.Views;

public partial class WordEditorView : ContentView
{
    private readonly Word _word;

    public WordEditorView(Word word)
    {
        InitializeComponent();
        _word = word;
        pckWordType.ItemsSource = Enum.GetValues<WordTypes>();
        BindingContext = word;
        entAliases.Text = string.Join(Environment.NewLine, word.Aliases);
    }

    public void ApplyChanges()
    {
        _word.Name = _word.Name.ToLowerInvariant();
        _word.Aliases = (entAliases.Text ?? string.Empty)
            .Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries)
            .Select(alias => alias.Trim().ToLowerInvariant())
            .Where(alias => alias.Length > 0)
            .ToList();
    }
}
