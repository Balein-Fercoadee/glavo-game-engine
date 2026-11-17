using CommunityToolkit.Maui.Views;
using GameDatabaseEditor.Utilities;
using GameDatabaseEditor.Views;
using GameEngine.GameData;

namespace GameDatabaseEditor;

public partial class MainPage : ContentPage
{
    public GameDatabase _gameDatabase;
    private bool _roomEditorOpen;
    private bool _itemEditorOpen;
    private bool _messageEditorOpen;

    public MainPage()
    {
        InitializeComponent();

        _gameDatabase = new GameDatabase();
        this.BindingContext = _gameDatabase;
    }

    private async void btnAddRoom_Clicked(object sender, EventArgs e)
    {
        var newRoom = new Room
        {
            Id = GeneralUtilities.GetIdNumber(_gameDatabase.Rooms.OrderBy(r => r.Id).Cast<IIdentifiable>().ToList())
        };
        var roomView = new RoomEditorView();
        roomView.Rooms = _gameDatabase.Rooms;
        roomView.Items = _gameDatabase.Items;
        roomView.EditedRoom = newRoom;


        var popup = SetupEditorPopup(roomView, EditorTypes.Room, EditorModes.Add);

        await this.ShowPopupAsync(popup);

        if (popup.Result?.Result is true)
        {
            _gameDatabase.Rooms.Add(newRoom);
        }
    }

    private Popup SetupEditorPopup(ContentView editorView, EditorTypes editorType, EditorModes editorMode)
    {
        var popup = new GenericEditorPopup()
        {
            Color = this.BackgroundColor,
            CanBeDismissedByTappingOutsideOfPopup = false,
            Parent = this,
        };

        popup.EditorView = editorView;
        popup.EditorType = editorType;
        popup.EditorMode = editorMode;

        return popup;
    }

    private void btnRemoveRoom_Clicked(object sender, EventArgs e)
    {
        Room? removedRoom = lstRooms.SelectedItem as Room;
        if (removedRoom != null)
        {
            GeneralUtilities.RemoveRoom(_gameDatabase, removedRoom);
        }
    }

    private async void btnEditRoom_Clicked(object sender, EventArgs e)
    {
        if (lstRooms.SelectedItem is Room room)
        {
            await EditRoomAsync(room);
        }
    }

    private async void Room_DoubleTapped(object sender, TappedEventArgs e)
    {
        if (sender is BindableObject { BindingContext: Room room })
        {
            await EditRoomAsync(room);
        }
    }

    private async Task EditRoomAsync(Room room)
    {
        if (_roomEditorOpen)
        {
            return;
        }

        _roomEditorOpen = true;
        try
        {
            var editSession = new RoomEditSession(room);
            var roomView = new RoomEditorView
            {
                Rooms = _gameDatabase.Rooms,
                Items = _gameDatabase.Items,
                EditedRoom = editSession.Draft
            };
            var popup = SetupEditorPopup(roomView, EditorTypes.Room, EditorModes.Edit);
            await this.ShowPopupAsync(popup);

            if (popup.Result?.Result is true)
            {
                editSession.Save();
            }
        }
        finally
        {
            _roomEditorOpen = false;
        }
    }

    private void btnAddWord_Clicked(object sender, EventArgs e)
    {

    }

    private async void btnAddItem_Clicked(object sender, EventArgs e)
    {
        if (_itemEditorOpen)
        {
            return;
        }

        _itemEditorOpen = true;
        try
        {
            var item = new Item
            {
                Id = GeneralUtilities.GetIdNumber(_gameDatabase.Items.OrderBy(i => i.Id).Cast<IIdentifiable>().ToList())
            };
            var popup = SetupEditorPopup(new ItemEditorView(item), EditorTypes.Item, EditorModes.Add);
            await this.ShowPopupAsync(popup);

            if (popup.Result?.Result is true)
            {
                _gameDatabase.Items.Add(item);
            }
        }
        finally
        {
            _itemEditorOpen = false;
        }
    }

    private async void btnEditItem_Clicked(object sender, EventArgs e)
    {
        if (lstItems.SelectedItem is Item item)
        {
            await EditItemAsync(item);
        }
    }

    private async void Item_DoubleTapped(object sender, TappedEventArgs e)
    {
        if (sender is BindableObject { BindingContext: Item item })
        {
            await EditItemAsync(item);
        }
    }

    private async Task EditItemAsync(Item item)
    {
        if (_itemEditorOpen)
        {
            return;
        }

        _itemEditorOpen = true;
        try
        {
            var editSession = new ItemEditSession(_gameDatabase.Items, item);
            var popup = SetupEditorPopup(new ItemEditorView(editSession.Draft), EditorTypes.Item, EditorModes.Edit);
            await this.ShowPopupAsync(popup);

            if (popup.Result?.Result is true)
            {
                if (editSession.Save())
                {
                    lstItems.SelectedItem = editSession.Draft;
                }
                else
                {
                    await DisplayAlertAsync("Cannot save item", "The item is no longer in the database.", "OK");
                }
            }
        }
        finally
        {
            _itemEditorOpen = false;
        }
    }

    private async void btnRemoveItem_Clicked(object sender, EventArgs e)
    {
        if (lstItems.SelectedItem is not Item item)
        {
            return;
        }

        var references = ItemReferences.Find(_gameDatabase, item.Id);
        if (references.Count > 0)
        {
            await DisplayAlertAsync("Cannot remove item",
                $"Remove the item from these locations first:\n{string.Join("\n", references)}", "OK");
            return;
        }

        lstItems.SelectedItem = null;
        _gameDatabase.Items.Remove(item);
    }

    private async void btnAddMessage_Clicked(object sender, EventArgs e)
    {
        if (_messageEditorOpen)
        {
            return;
        }

        _messageEditorOpen = true;
        try
        {
            var message = new Message
            {
                Id = GeneralUtilities.GetIdNumber(_gameDatabase.Messages.OrderBy(m => m.Id).Cast<IIdentifiable>().ToList())
            };
            var popup = SetupEditorPopup(new MessageEditorView(message), EditorTypes.Message, EditorModes.Add);
            await this.ShowPopupAsync(popup);

            if (popup.Result?.Result is true)
            {
                _gameDatabase.Messages.Add(message);
            }
        }
        finally
        {
            _messageEditorOpen = false;
        }
    }

    private async void btnEditMessage_Clicked(object sender, EventArgs e)
    {
        if (lstMessages.SelectedItem is Message message)
        {
            await EditMessageAsync(message);
        }
    }

    private async void Message_DoubleTapped(object sender, TappedEventArgs e)
    {
        if (sender is BindableObject { BindingContext: Message message })
        {
            await EditMessageAsync(message);
        }
    }

    private async Task EditMessageAsync(Message message)
    {
        if (_messageEditorOpen)
        {
            return;
        }

        _messageEditorOpen = true;
        try
        {
            var editSession = new MessageEditSession(_gameDatabase.Messages, message);
            var popup = SetupEditorPopup(new MessageEditorView(editSession.Draft), EditorTypes.Message, EditorModes.Edit);
            await this.ShowPopupAsync(popup);

            if (popup.Result?.Result is true)
            {
                if (editSession.Save())
                {
                    lstMessages.SelectedItem = editSession.Draft;
                }
                else
                {
                    await DisplayAlertAsync("Cannot save message", "The message is no longer in the database.", "OK");
                }
            }
        }
        finally
        {
            _messageEditorOpen = false;
        }
    }

    private async void btnRemoveMessage_Clicked(object sender, EventArgs e)
    {
        if (lstMessages.SelectedItem is not Message message)
        {
            return;
        }

        var references = MessageReferences.Find(_gameDatabase, message.Id);
        if (references.Count > 0)
        {
            await DisplayAlertAsync("Cannot remove message",
                $"Remove the message from these locations first:\n{string.Join("\n", references)}", "OK");
            return;
        }

        lstMessages.SelectedItem = null;
        _gameDatabase.Messages.Remove(message);
    }

    private async void btnOpenDatabase_Clicked(object sender, EventArgs e)
    {
        var file = await FilePicker.Default.PickAsync(new PickOptions
        {
            PickerTitle = "Load GGE Database",
            FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.WinUI, new[] { ".gge" } }, // file extension
                })
        });

        if (file != null)
        {
            string? filePath = Path.GetDirectoryName(file.FullPath);
            string fileName = file.FileName;

            if (filePath != null)
            {
                _gameDatabase = new GameDatabase();
                _gameDatabase.Load(filePath, fileName);
            }

            this.BindingContext = _gameDatabase;
        }
    }

    private async void btnNewDatabase_Clicked(object sender, EventArgs e)
    {
        bool isEmpty = GeneralUtilities.IsDatabaseEmpty(_gameDatabase);

        bool setupNewDatabase = true;
        if (!isEmpty)
        {
            setupNewDatabase = await DisplayAlertAsync("Database not empty!", "The current database has objects. Are you sure you want to overwrite?", "Yes", "No");
        }

        if (setupNewDatabase)
        {
            _gameDatabase = GeneralUtilities.GetFreshDatabase();

            this.BindingContext = _gameDatabase;
        }
    }

    private async void btnSaveDatabase_Clicked(object sender, EventArgs e)
    {
        var file = await FilePicker.Default.PickAsync(new PickOptions
        {
            PickerTitle = "Save GGE Database",
            FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.WinUI, new[] { ".gge" } }, // file extension
                })
        });

        if (file != null)
        {
            string? filePath = Path.GetDirectoryName(file.FullPath);
            string fileName = file.FileName;

            if (filePath != null)
            {
                _gameDatabase.Save(filePath, fileName);
            }
        }
    }
}
