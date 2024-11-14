using GameEngine.GameData;

namespace GameDatabaseEditor.Windows;

public partial class RoomEditorPage : ContentPage
{
	public EditorModes EditorMode { get; set; }

	public Room? ReturnedRoom { get; set; }

	public RoomEditorPage()
	{
		InitializeComponent();
	}
}