using GameEngine.GameData;

namespace GameDatabaseEditor.Views;

public partial class RoomEditorView : ContentView
{
	public IEnumerable<Room>? Rooms { get; set; }

	public IEnumerable<Item>? Items { get; set; }

	public RoomEditorView()
	{
		InitializeComponent();
	}
}