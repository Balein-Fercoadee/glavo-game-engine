using GameEngine.GameData;

namespace GameDatabaseEditor.Views;

public partial class RoomEditorView : ContentView
{
    private Room? _editedRoom;
    private IEnumerable<Room>? _rooms;

    /// <summary>
    /// Gets or sets the <c>Room</c> to be added/edited.
    /// </summary>
    public Room? EditedRoom
    {
        get { return _editedRoom; }
        set
        {
            _editedRoom = value;
            this.BindingContext = _editedRoom;
        }
    }

    public IEnumerable<Room>? Rooms
    {
        get { return _rooms; }
        set
        {
            var roomList = SetupRoomCollection(value);
            _rooms = roomList;
            SetRoomItemsSource();
        }
    }

    public IEnumerable<Item>? Items { get; set; }

    public RoomEditorView()
    {
        InitializeComponent();
    }

    private IEnumerable<Room> SetupRoomCollection(IEnumerable<Room>? rooms)
    {
        List<Room> roomList = new List<Room>();

        roomList.Add(new Room() { Id = -1, Name = "Unset" });
        if (rooms != null && rooms.Any())
        {
            roomList.AddRange(rooms);
        }

        return roomList;
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
}