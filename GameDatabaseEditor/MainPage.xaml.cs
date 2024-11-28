﻿using CommunityToolkit.Maui.Views;
using GameDatabaseEditor.Utilities;
using GameDatabaseEditor.Views;
using GameEngine.GameData;

namespace GameDatabaseEditor;

public partial class MainPage : ContentPage
{
    public GameDatabase _gameDatabase;

    bool _hasChanged = false;

    public MainPage()
    {
        InitializeComponent();

        _gameDatabase = new GameDatabase();
        this.BindingContext = _gameDatabase;
    }

    private void btnAddRoom_Clicked(object sender, EventArgs e)
    {
        var newRoom = new Room();
        var roomView = new RoomEditorView();
        roomView.BindingContext = newRoom;
        roomView.Rooms = _gameDatabase.Rooms;
        roomView.Items = _gameDatabase.Items;

        var popup = SetupEditorPopup(roomView, EditorTypes.Room);

        this.ShowPopup(popup);

        if (popup.Result != null && popup.Result.Result != null && (bool)(popup.Result.Result) == true)
        {
            // Add the Room to the Room collection
        }
    }

    private Popup SetupEditorPopup(ContentView editorView, EditorTypes editorTypes)
    {
        var popup = new GenericEditorPopup()
        {
            Color = this.BackgroundColor,
            CanBeDismissedByTappingOutsideOfPopup = false,
            Parent = this,
        };

        popup.EditorView = editorView;
        popup.EditorType = editorTypes;

        return popup;
    }

    private void btnRemoveRoom_Clicked(object sender, EventArgs e)
    {
        Room? removedRoom = lstRooms.SelectedItem as Room;
        if (removedRoom != null)
        {
            _gameDatabase.Rooms.Remove(removedRoom);
            _hasChanged = true;
        }
    }

    private void btnEditRoom_Clicked(object sender, EventArgs e)
    {

    }

    private void btnAddWord_Clicked(object sender, EventArgs e)
    {

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
            setupNewDatabase = await DisplayAlert("Database not empty!", "The current database has objects. Are you sure you want to overwrite?", "Yes", "No");
        }

        if (setupNewDatabase)
        {
            _gameDatabase = GeneralUtilities.GetFreshDatabase();

            this.BindingContext = _gameDatabase;
        }
    }
}

