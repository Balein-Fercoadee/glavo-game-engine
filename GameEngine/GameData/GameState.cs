using System.Text.Json;
using System.Text.Json.Serialization;

namespace GameEngine.GameData;

public class GameState
{
    [JsonInclude]
    public GameDatabase GameData { get; set; }

    [JsonInclude]
    public Player PlayerData { get; set; }

    public GameState()
    {
        GameData = new GameDatabase();
        PlayerData = new Player();
    }

    public void Load(string saveDirectory, string databaseFileName, bool debug = false)
    {
        string fileFullPath = Path.Join(saveDirectory, databaseFileName);
        string gameStateJson = File.ReadAllText(fileFullPath);
        var loadedState = JsonSerializer.Deserialize<GameState>(gameStateJson);

        if (loadedState == null)
        {
            throw new InvalidOperationException($"Unable to load a valid game state from '{fileFullPath}'.");
        }

        if (debug)
        {
            Console.WriteLine();
            Console.WriteLine($"Game State Loaded: {loadedState.GameData.Name}");
            Console.WriteLine($"  Player Room: {loadedState.PlayerData.CurrentRoomId}");
            Console.WriteLine($"  Inventory Count: {loadedState.PlayerData.ItemInventory.Count}");
            Console.WriteLine($"  Visited Rooms: {loadedState.PlayerData.VisitedRoomIds.Count}");
        }

        GameData = loadedState.GameData ?? new GameDatabase();
        PlayerData = loadedState.PlayerData ?? new Player();
    }
}
