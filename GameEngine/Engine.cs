using System.Diagnostics;
using GameEngine.GameData;
using GameEngine.Utilities;

namespace GameEngine;

/// <summary>
/// Engine contains the main game loop as well as 
/// all of the supporting function of a game:
/// <list type="bullet">
/// <item>Loading of a game database.</item>
/// <item>Loading and saving of a game session.</item>
/// <item>Iterpreting user input into game actions.</item>
/// </list>
/// </summary>
public class Engine
{
    private bool _outputDebugStatements;

    public Engine()
    {

    }

    /// <summary>
    /// The main game loop. I.e The method that actually drives the game.
    /// </summary>
    /// <param name="args"></param>
    public void GameLoop(string[] args)
    {
        Console.Clear();
        // Show the engine intro text.
        Console.WriteLine(Constants.LOADING_TEXT);

        var setupReturn = EngineSetup.StartSetup(args);

        if (!setupReturn.SetupSuccessful)
        {
            Console.WriteLine(Formatter.ErrorMessage(setupReturn.ErrorMessages));
            Environment.Exit(1);
        }

        // Command line was parsed so now we load!

        _outputDebugStatements = setupReturn.OutputDebugStatements;

        GameState gameState = new GameState();
        // If there's no save file, load the game data. Otherwise, load the save.
        if (string.IsNullOrWhiteSpace(setupReturn.SaveFilePath))
        {
            gameState.GameData.Load(string.Empty, setupReturn.GameDatabaseFilePath, _outputDebugStatements);
            gameState.PlayerData.CurrentRoomId = gameState.GameData.StartingRoomId;
        }
        else
        {
            gameState.Load(string.Empty, setupReturn.SaveFilePath, _outputDebugStatements);
        }

        // Show the game name & description
        Console.Write(Formatter.GameLoadedMessage(gameState.GameData));

        // Force a Look
        Console.WriteLine(Look(gameState.PlayerData.CurrentRoomId, gameState, true));
        // Game Loop
        while (true)
        {

        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="currentRoomId">The <c>Room.Id</c> of the room where the player is currently at.</param>
    /// <param name="gameState">The <c>GameState</c> object.</param>
    /// <param name="overrideToFullDescription">Should the full description be returned regardless if the <c>Player</c> has arleady visited the <c>Room</c>.</param>
    /// <returns></returns>
    private string Look(int currentRoomId, GameState gameState, bool overrideToFullDescription = false)
    {
        Room currentRoom = ObjectFinder.GetRoom(gameState.GameData.Rooms, currentRoomId);
        List<Item> itemsInRoom = ObjectFinder.GetItems(gameState.GameData.Items, currentRoom.ContainedItemIds);
        List<string> exitsFromRoom = currentRoom.AvailableExits();
        Player player = gameState.PlayerData;

        string description = (!player.HasAlreadyVisitedRoom(currentRoomId) || overrideToFullDescription) ? currentRoom.Description : string.Empty;
        string items = string.Join(", ", itemsInRoom);
        string exits = Formatter.RoomAvailableExitsMessage(exitsFromRoom);
        string availableExits = exits.Length > 0 ? $"Available exits: {exits}" : "Available exits: none apparent";

        string lookResponse = $"{currentRoom.Name}\n{description}\n{availableExits}";

        return lookResponse;
    }
}
