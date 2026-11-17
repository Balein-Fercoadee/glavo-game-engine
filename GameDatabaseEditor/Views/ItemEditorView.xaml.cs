using GameEngine.GameData;

namespace GameDatabaseEditor.Views;

public partial class ItemEditorView : ContentView
{
    public ItemEditorView(Item item)
    {
        InitializeComponent();
        BindingContext = item;
    }
}
