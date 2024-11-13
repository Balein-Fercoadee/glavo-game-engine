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
        _gameDatabase.Load(@"C:\Temp\glavo_game_engine\", "sample_game.gge");

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

    private void btnAddWord_Clicked(object sender, EventArgs e)
    {

    }
}

