using GameEngine.GameData;

namespace GameDatabaseEditor.Views;

public partial class MessageEditorView : ContentView
{
    public MessageEditorView(Message message)
    {
        InitializeComponent();
        BindingContext = message;
    }
}
