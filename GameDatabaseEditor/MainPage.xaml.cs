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
        Room room1 = new Room();
        room1.Id = 0;
        room1.Name = "Starting Room";
        Room room2 = new Room();
        room2.Id = 1;
        room2.Name = "Treasure Room";
        _gameDatabase.Rooms.Add(room1);
        _gameDatabase.Rooms.Add(room2);
        //_gameDatabase.StartingRoomId = 0;

        this.BindingContext = _gameDatabase;

        InitializeControls();
    }

    /// <summary>
    /// Sets the intial states for some of the window controls.
    /// This is due to data binding not working in some instances.
    /// </summary>
    private void InitializeControls()
    {
        btnEditRoom.IsEnabled = false;
        btnRemoveRoom.IsEnabled = false;
    }

    private async void btnAddRoom_Clicked(object sender, EventArgs e)
    {
        await DisplayAlert("Pop-up", "You clicks AddRoom!", "OK");
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

    private void lstRooms_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        /*
         * For some reason databinding wasn't working on IsEnabled (as of Nov-11-2024).
         * Specifically, binding IsEnabled to another control's property.
         * That's why the retro code.
         */
        if (e.SelectedItem != null)
        {
            btnEditRoom.IsEnabled = true;
            btnRemoveRoom.IsEnabled = true;
        }
        else
        {
            btnEditRoom.IsEnabled = false;
            btnRemoveRoom.IsEnabled = false;
        }
    }

    private void pckStartingRoomId_SelectedIndexChanged(object sender, EventArgs e)
    {
        /*
        * For some reason databinding wasn't working on IsEnabled (as of Nov-11-2024).
        * Specifically, binding Stroke to another control's property.
        * That's why the retro code.
        */
        Picker picker = (Picker)sender;
        if (picker.SelectedIndex == -1)
        {
            brdStartingRoom.Stroke = Brush.Red;
        }
        else
        {
            brdStartingRoom.Stroke = Brush.Transparent;
        }

    }
}

