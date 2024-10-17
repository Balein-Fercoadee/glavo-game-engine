using System.Diagnostics.CodeAnalysis;
using GameEngine.GameData;
using GameEngine.Utilities;

namespace GameEngine_Tests.Utilities;

[TestClass]
public class ProcessorTests
{
    [TestMethod]
    public void Help()
    {
        GameState gameState = new GameState();
        Player player = new Player();
        GameDatabase gameDb = new GameDatabase();
        gameDb.Load(@"test_data/", "test_game_database.gge", true);
        gameState.PlayerData = player;
        gameState.GameData = gameDb;

        Word? help = ObjectFinder.GetWord(gameDb.Words, "help");

        bool quit = false;
        string expectedOutput = MessageFormatter.Help();
        string actualOutput;
        using (StringWriter sw = new StringWriter())
        {
            Console.SetOut(sw);
            quit = Processor.ProcessWords(help, null, gameState);
            sw.Flush();

            actualOutput = sw.ToString();
        }

        actualOutput = CleanConsoleOutput(actualOutput);
        Assert.IsFalse(quit);
        Assert.AreEqual(expectedOutput, actualOutput);
    }

    [TestMethod]
    public void NullVerb()
    {
        GameState gameState = new GameState();
        Player player = new Player();
        GameDatabase gameDb = new GameDatabase();
        gameDb.Load(@"test_data/", "test_game_database.gge", true);
        gameState.PlayerData = player;
        gameState.GameData = gameDb;
        bool quit = false;
        // For some reason the actual output appends a carrage return and line feed.
        string expectedOutput = "You don't know how to do that!";
        string actualOutput;
        using (StringWriter sw = new StringWriter())
        {
            Console.SetOut(sw);
            quit = Processor.ProcessWords(null, null, gameState);
            sw.Flush();

            actualOutput = sw.ToString();
        }

        actualOutput = CleanConsoleOutput(actualOutput);
        Assert.IsFalse(quit);
        Assert.AreEqual(expectedOutput, actualOutput);
    }

    [ExcludeFromCodeCoverage]
    private string CleanConsoleOutput(string consoleOutput)
    {
        string cleanOutput = consoleOutput;
        // The IF below makes the Console output work regardless of OS
        // WriteLine on Windows uses '\r\n', Linux uses '\n'.
        if (consoleOutput.EndsWith("\r\n"))
            cleanOutput = consoleOutput[..^2];
        else if (consoleOutput.EndsWith("\n")) // the string ends in '\n'
            cleanOutput = consoleOutput[..^1];

        return cleanOutput;
    }
}
