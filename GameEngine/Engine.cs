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
            Console.WriteLine(MessageFormatter.Error(setupReturn.ErrorMessages));
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
        Console.Write(MessageFormatter.GameLoaded(gameState.GameData));

        // Force a Look
        Console.WriteLine(MessageFormatter.Look(gameState.PlayerData.CurrentRoomId, gameState.GameData, gameState.PlayerData, true));
        // Game Loop
        while (true)
        {
            string? playerInput = string.Empty;
            do
            {
                Console.Write("\n" + Constants.INPUT_PROMPT);
                playerInput = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(playerInput));

            string cleanedInput = InputFormatter.CleanPlayerInput(playerInput);
            (Word? verb, Word? noun) = Parser.GetWordsFromInput(cleanedInput, gameState.GameData.Words);

            if (verb != null)
            {
                bool quitGame = false;
                switch (verb.Name)
                {
                    case "go":
                        if (noun != null)
                        {
                            Room currentRoom = ObjectFinder.GetRoom(gameState.GameData.Rooms, gameState.PlayerData.CurrentRoomId);
                            int roomId = -1;
                            switch (noun.Name)
                            {
                                case "north":
                                    roomId = currentRoom.ExitIdNorth;
                                    break;
                                case "south":
                                    roomId = currentRoom.ExitIdSouth;
                                    break;
                                case "east":
                                    roomId = currentRoom.ExitIdEast;
                                    break;
                                case "west":
                                    roomId = currentRoom.ExitIdWest;
                                    break;
                            }

                            if (roomId != Constants.ROOM_ID_UNSET)
                            {
                                Console.WriteLine(MessageFormatter.Look(roomId, gameState.GameData, gameState.PlayerData));
                                gameState.PlayerData.CurrentRoomId = roomId;
                            }
                            else
                            {
                                Console.WriteLine("You can't go in that direction!!");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Go where??");
                        }
                        break;
                    case "help":
                        Console.WriteLine("Can't help you, yet!");
                        break;
                    case "look":
                        if (noun == null)
                            Console.WriteLine(MessageFormatter.Look(gameState.PlayerData.CurrentRoomId, gameState.GameData, gameState.PlayerData, true));
                        else
                        {
                            if (noun.Name == "inventory")
                            {
                                Console.WriteLine("You don't have an inventory yet!");
                            }
                        }
                        break;
                    case "quit":
                        quitGame = true;
                        break;
                }
                if (quitGame)
                {
                    Console.WriteLine("Such a quiter. BOOOOOOOO!");
                    break;
                }
            }
            else
            {
                Console.WriteLine("You don't know how to do that!");
            }
        }
    }
}
