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
        Room room3 = new Room();
        room3.Id = 2;
        room3.Name = "Regular Room";
        _gameDatabase.Rooms.Add(room1);
        _gameDatabase.Rooms.Add(room2);
        _gameDatabase.Rooms.Add(room3);

        this.BindingContext = _gameDatabase;
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
}

