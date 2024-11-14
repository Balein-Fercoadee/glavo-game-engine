using GameEngine.GameData;
using System.Text.Json;

namespace GameDatabaseEditor;

public partial class MainPage : ContentPage
{
    public GameDatabase _gameDatabase;

    bool _hasChanged = false;

    public MainPage()
    {
        InitializeComponent();
    }

    private async void btnAddRoom_Clicked(object sender, EventArgs e)
    {
        var roomWindow = new Windows.RoomEditorPage();
        roomWindow.Title = "Room - Add New";
        roomWindow.EditorMode = EditorModes.Add;
        roomWindow.BindingContext = new Room();

        await Navigation.PushAsync(roomWindow);

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

    private static List<Word> GetSacredWords()
    {
        string filePath = Path.Join(Environment.CurrentDirectory, "sacred-word-list.json");
        string wordsJson = File.ReadAllText(filePath);
        var words = JsonSerializer.Deserialize<List<Word>>(wordsJson);

        return words;
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
            string filePath = Path.GetDirectoryName(file.FullPath);
            string fileName = file.FileName;

            _gameDatabase = new GameDatabase();
            _gameDatabase.Load(filePath, fileName);

            this.BindingContext = _gameDatabase;
        }
    }
}

