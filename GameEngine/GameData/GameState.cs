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
        
    }
}
