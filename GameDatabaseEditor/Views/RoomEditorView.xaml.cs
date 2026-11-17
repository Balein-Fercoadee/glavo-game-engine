using GameEngine.GameData;

namespace GameDatabaseEditor.Views;

public partial class RoomEditorView : ContentView
{
    private Room? _editedRoom;
    private List<Room> _rooms = new();

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

    private IEnumerable<Item>? _items;

    public IEnumerable<Item>? Items
    {
        get { return _items; }
        set
        {
            _items = value;
            pckAddRoomItem.ItemsSource = _items?.ToList();
        }
    }

    public RoomEditorView()
    {
        InitializeComponent();
    }

    private void btnAddRoomItem_Clicked(object? sender, EventArgs e)
    {
        if (pckAddRoomItem.SelectedItem is Item item && _editedRoom is not null)
        {
            if (!_editedRoom.ContainedItemIds.Contains(item.Id))
            {
                _editedRoom.ContainedItemIds.Add(item.Id);
            }

            pckAddRoomItem.SelectedItem = null;
        }
    }

    private void btnRemoveRoomItem_Clicked(object? sender, EventArgs e)
    {
        if (lstRoomItems.SelectedItem is int itemId)
        {
            lstRoomItems.SelectedItem = null;
            _editedRoom?.ContainedItemIds.Remove(itemId);
        }
    }

    private List<Room> SetupRoomCollection(IEnumerable<Room>? rooms)
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
        pckDownExitRoomId.ItemsSource = _rooms;
        pckEastExitRoomId.ItemsSource = _rooms;
        pckNorthExitRoomId.ItemsSource = _rooms;
        pckSouthExitRoomId.ItemsSource = _rooms;
        pckUpExitRoomId.ItemsSource = _rooms;
        pckWestExitRoomId.ItemsSource = _rooms;
    }
}