using CommunityToolkit.Maui.Views;
using GameDatabaseEditor.Utilities;
using GameDatabaseEditor.Views;
using GameEngine.GameData;

namespace GameDatabaseEditor;

public partial class MainPage : ContentPage
{
    public GameDatabase _gameDatabase;
    private DatabaseValidation _databaseValidation;
    private bool _roomEditorOpen;
    private bool _itemEditorOpen;
    private bool _messageEditorOpen;
    private bool _wordEditorOpen;

    public MainPage()
    {
        InitializeComponent();

        _gameDatabase = new GameDatabase();
        _databaseValidation = new DatabaseValidation(_gameDatabase);
        BindableLayout.SetItemsSource(validationErrorsLayout, _databaseValidation.Errors);
        this.BindingContext = _gameDatabase;
    }

    private void SetDatabase(GameDatabase database)
    {
        _databaseValidation.Dispose();
        _gameDatabase = database;
        _databaseValidation = new DatabaseValidation(database);
        BindableLayout.SetItemsSource(validationErrorsLayout, _databaseValidation.Errors);
        BindingContext = database;
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

    private async void btnRemoveRoom_Clicked(object sender, EventArgs e)
    {
        if (lstRooms.SelectedItem is not Room room)
        {
            return;
        }

        var references = RoomReferences.Find(_gameDatabase, room.Id);
        if (references.Count > 0)
        {
            await DisplayAlertAsync("Cannot remove room",
                $"Change these references before removing the room:\n{string.Join("\n", references)}", "OK");
            return;
        }

        lstRooms.SelectedItem = null;
        GeneralUtilities.RemoveRoom(_gameDatabase, room);
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

    private async void btnAddWord_Clicked(object sender, EventArgs e)
    {
        await AddWordAsync();
    }

    private async Task AddWordAsync()
    {
        if (_wordEditorOpen)
        {
            return;
        }

        _wordEditorOpen = true;
        try
        {
            var word = new Word
            {
                Id = GeneralUtilities.GetIdNumber(_gameDatabase.Words.OrderBy(w => w.Id).Cast<IIdentifiable>().ToList()),
                WordType = WordTypes.Noun
            };
            var wordView = new WordEditorView(word);
            var popup = SetupEditorPopup(wordView, EditorTypes.Word, EditorModes.Add);
            await this.ShowPopupAsync(popup);

            if (popup.Result?.Result is true)
            {
                wordView.ApplyChanges();
                string? duplicateToken = WordValidation.FindDuplicateToken(_gameDatabase.Words, word);
                if (duplicateToken is not null)
                {
                    await DisplayAlertAsync("Cannot add word",
                        $"The name or alias '{duplicateToken}' is already used by another word.", "OK");
                }
                else
                {
                    _gameDatabase.Words.Add(word);
                    lstWords.SelectedItem = word;
                }
            }
        }
        finally
        {
            _wordEditorOpen = false;
        }
    }

    private async void btnEditWord_Clicked(object sender, EventArgs e)
    {
        if (lstWords.SelectedItem is Word word)
        {
            await EditWordAsync(word);
        }
    }

    private async void Word_DoubleTapped(object sender, TappedEventArgs e)
    {
        if (sender is BindableObject { BindingContext: Word word })
        {
            await EditWordAsync(word);
        }
    }

    private async Task EditWordAsync(Word word)
    {
        if (_wordEditorOpen)
        {
            return;
        }

        _wordEditorOpen = true;
        try
        {
            var editSession = new WordEditSession(_gameDatabase.Words, word);
            var wordView = new WordEditorView(editSession.Draft);
            var popup = SetupEditorPopup(wordView, EditorTypes.Word, EditorModes.Edit);
            await this.ShowPopupAsync(popup);

            if (popup.Result?.Result is true)
            {
                wordView.ApplyChanges();
                if (editSession.Save())
                {
                    lstWords.SelectedItem = editSession.Draft;
                }
                else
                {
                    await DisplayAlertAsync("Cannot save word", "The word is no longer in the database.", "OK");
                }
            }
        }
        finally
        {
            _wordEditorOpen = false;
        }
    }

    private async void btnRemoveWord_Clicked(object sender, EventArgs e)
    {
        if (lstWords.SelectedItem is not Word word)
        {
            return;
        }

        var references = WordReferences.Find(_gameDatabase, word.Id);
        if (references.Count > 0)
        {
            await DisplayAlertAsync("Cannot remove word",
                $"Change these references before removing the word:\n{string.Join("\n", references)}", "OK");
            return;
        }

        lstWords.SelectedItem = null;
        _gameDatabase.Words.Remove(word);
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
                var database = new GameDatabase();
                database.Load(filePath, fileName);
                SetDatabase(database);
            }
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
            SetDatabase(GeneralUtilities.GetFreshDatabase());
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
