using GameEngine.GameData;

namespace GameDatabaseEditor.Views;

public partial class RoomEditorView : ContentView
{
    private IEnumerable<Room>? _rooms;

    public IEnumerable<Room>? Rooms
    {
        get { return _rooms; }
        set
        {
            _rooms = value;
            SetRoomItemsSource();
        }
    }

    public IEnumerable<Item>? Items { get; set; }

    public RoomEditorView()
    {
        InitializeComponent();
    }

    private void SetRoomItemsSource()
    {
        var castRooms = (System.Collections.IList)_rooms;

        pckDownExitRoomId.ItemsSource = castRooms;
        pckEastExitRoomId.ItemsSource = castRooms;
        pckNorthExitRoomId.ItemsSource = castRooms;
        pckSouthExitRoomId.ItemsSource = castRooms;
        pckUpExitRoomId.ItemsSource = castRooms;
        pckWestExitRoomId.ItemsSource = castRooms;
    }

    private void ResetButton_Clicked(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        string direction = button.Text.Replace("btnReset", string.Empty).ToLowerInvariant();

        switch (direction)
        {
            case "north":
                pckNorthExitRoomId.SelectedIndex = -1;
                break;
        }
    }
}