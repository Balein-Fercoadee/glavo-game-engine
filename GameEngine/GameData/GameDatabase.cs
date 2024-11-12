using System.Collections.ObjectModel;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GameEngine.GameData;

public class GameDatabase : BaseGameObject
{
    private string _description;
    private string _name;
    private int _startingRoomId;
    private int _treasureRoomId;

    /// <summary>
    /// Gets or set the name of the Game.
    /// </summary>
    public string Name
    {
        get
        {
            return _name;
        }
        set
        {
            _name = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Gets or sets the description of the Game.
    /// <para>The description will be displayed once the Game is loaded
    /// and before the first <c>Room</c> is displayed.</para>
    /// </summary>
    public string Description
    {
        get
        {
            return _description;
        }
        set
        {
            _description = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Gets or sets the <c>Room.Id</c> where the <c>Player</c> will start the Game.
    /// </summary>
    public int StartingRoomId
    {
        get
        {
            return _startingRoomId;
        }
        set
        {
            _startingRoomId = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Gets or sets the <c>Room.Id</c> where <c>Items</c> need to be placed
    /// in order to be scored.
    /// </summary>
    public int TreasureRoomId
    {
        get
        {
            return _treasureRoomId;
        }
        set
        {
            _treasureRoomId = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Gets a collection of <c>Actions</c> available in the game.
    /// </summary>
    [JsonInclude]
    public ObservableCollection<Action> Actions;

    /// <summary>
    /// Gets or sets a collection of <c>Items</c> available in the game.
    /// </summary>
    [JsonInclude]
    public ObservableCollection<Item> Items { get; set; }

    /// <summary>
    /// Gets or sets a collection of <c>Messages</c> available in the game.
    /// </summary>
    [JsonInclude]
    public ObservableCollection<Message> Messages { get; set; }

    /// <summary>
    /// Gets or sets a collection of <c>Rooms</c> available in the game.
    /// </summary>
    [JsonInclude]
    public ObservableCollection<Room> Rooms { get; set; }

    /// <summary>
    /// Gets or sets a collection of <c>Words</c> available in the game.
    /// </summary>
    [JsonInclude]
    public ObservableCollection<Word> Words { get; set; }

    /// <summary>
    /// Gets all <c>Words</c> that are nouns.
    /// </summary>
    [JsonIgnore]
    public IEnumerable<Word> Nouns
    {
        get { return Words.Where(w => w.WordType == WordTypes.Noun); }
    }

    /// <summary>
    /// Gets all <c>Words</c> that are verbs.
    /// </summary>
    [JsonIgnore]
    public IEnumerable<Word> Verbs
    {
        get { return Words.Where(w => w.WordType == WordTypes.Verb); }
    }

    public GameDatabase()
    {
        _description = string.Empty;
        _name = string.Empty;
        StartingRoomId = Constants.ROOM_ID_UNSET;
        TreasureRoomId = Constants.ROOM_ID_UNSET;

        Actions = new ObservableCollection<Action>();
        Items = new ObservableCollection<Item>();
        Messages = new ObservableCollection<Message>();
        Rooms = new ObservableCollection<Room>();
        Words = new ObservableCollection<Word>();
    }

    /// <summary>
    /// Save the <c>GameDatabase</c> object to a JSON file.
    /// </summary>
    /// <param name="saveDirectory">The directory to save file.</param>
    /// <param name="databaseFileName">The file name for the database.</param>
    public void Save(string saveDirectory, string databaseFileName, bool debug = false)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string gameDatabaseJson = JsonSerializer.Serialize(this, options);
        string fileFullPath = Path.Join(saveDirectory, databaseFileName);

        if (debug)
        {
            Console.WriteLine(gameDatabaseJson);
        }

        File.WriteAllText(fileFullPath, gameDatabaseJson);
    }

    public void Load(string saveDirectory, string databaseFileName, bool debug = false)
    {
        string fileFullPath = Path.Join(saveDirectory, databaseFileName);
        string gameDatabaseJson = File.ReadAllText(fileFullPath);
        var loadedDatabase = JsonSerializer.Deserialize<GameDatabase>(gameDatabaseJson);

        if (loadedDatabase != null && debug)
        {
            Console.WriteLine();
            Console.WriteLine($"Game Database Name: {loadedDatabase.Name}");
            Console.WriteLine($"  Game Description: {loadedDatabase.Description}");
            Console.WriteLine($"  Starting Room Id: {loadedDatabase.StartingRoomId}");
            Console.WriteLine($"      # of Actions: {loadedDatabase.Actions.Count}");
            Console.WriteLine($"        # of Items: {loadedDatabase.Items.Count}");
            Console.WriteLine($"     # of Messages: {loadedDatabase.Messages.Count}");
            Console.WriteLine($"        # of Rooms: {loadedDatabase.Rooms.Count}");
            Console.WriteLine($"        # of Words: {loadedDatabase.Words.Count}");
        }

        if (loadedDatabase != null)
        {
            Description = loadedDatabase.Description;
            Name = loadedDatabase.Name;
            StartingRoomId = loadedDatabase.StartingRoomId;
            TreasureRoomId = loadedDatabase.TreasureRoomId;

            Actions = loadedDatabase.Actions;
            Items = loadedDatabase.Items;
            Messages = loadedDatabase.Messages;
            Rooms = loadedDatabase.Rooms;
            Words = loadedDatabase.Words;
        }
    }
}
