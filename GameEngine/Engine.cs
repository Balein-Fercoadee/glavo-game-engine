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
    /// The main game loop. I.e The code that actually drives the game.
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
                Console.Write(Constants.INPUT_PROMPT);
                playerInput = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(playerInput));

            string cleanedInput = InputFormatter.CleanPlayerInput(playerInput);
            (Word? verb, Word? noun) = Parser.GetWordsFromInput(cleanedInput, gameState.GameData.Words);

            bool quitGame = Processor.ProcessWords(verb, noun, gameState);

            if (quitGame)
                break;
        }
    }
}
