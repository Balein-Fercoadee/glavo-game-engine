using System.Diagnostics.CodeAnalysis;
using GameEngine.GameData;
using GameEngine.Utilities;

namespace GameEngine_Tests.Utilities;

[TestClass]
public class ProcessorTests
{
    [TestMethod]
    public void Drop()
    {
        var gameState = SetupTestingGameState();
        var player = gameState.PlayerData;
        player.ItemInventory.Add(2);
        var verb = ObjectFinder.GetWord(gameState.GameData.Words, "drop");
        var noun = ObjectFinder.GetWord(gameState.GameData.Words, "ruby");
        bool quit;

        string expectedOutput = "You dropped: small ruby";
        string actualOutput;
        using (StringWriter sw = new StringWriter())
        {
            Console.SetOut(sw);
            quit = Processor.ProcessWords(verb, noun, gameState);
            sw.Flush();

            actualOutput = CleanConsoleOutput(sw.ToString());
        }

        Assert.IsFalse(quit);
        Assert.AreEqual(expectedOutput, actualOutput);
    }

    [TestMethod]
    public void DropAll()
    {
        var gameState = SetupTestingGameState();
        var player = gameState.PlayerData;
        player.ItemInventory.Add(2);
        var verb = ObjectFinder.GetWord(gameState.GameData.Words, "drop");
        var noun = ObjectFinder.GetWord(gameState.GameData.Words, "all");
        bool quit;

        string expectedOutput = "You dropped all carried items into the room!";
        string actualOutput;
        using (StringWriter sw = new StringWriter())
        {
            Console.SetOut(sw);
            quit = Processor.ProcessWords(verb, noun, gameState);
            sw.Flush();

            actualOutput = CleanConsoleOutput(sw.ToString());
        }

        Assert.IsFalse(quit);
        Assert.AreEqual(expectedOutput, actualOutput);
    }

    [TestMethod]
    public void DropAllEmptyInventory()
    {
        var gameState = SetupTestingGameState();
        var verb = ObjectFinder.GetWord(gameState.GameData.Words, "drop");
        var noun = ObjectFinder.GetWord(gameState.GameData.Words, "all");
        bool quit;

        string expectedOutput = "";
        string actualOutput;
        using (StringWriter sw = new StringWriter())
        {
            Console.SetOut(sw);
            quit = Processor.ProcessWords(verb, noun, gameState);
            sw.Flush();

            actualOutput = CleanConsoleOutput(sw.ToString());
        }

        Assert.IsFalse(quit);
        Assert.AreEqual(expectedOutput, actualOutput);
    }

    [TestMethod]
    public void DropItemNotPresent()
    {
        var gameState = SetupTestingGameState();
        var player = gameState.PlayerData;
        var verb = ObjectFinder.GetWord(gameState.GameData.Words, "drop");
        var noun = ObjectFinder.GetWord(gameState.GameData.Words, "rock");
        bool quit;

        string expectedOutput = "You're not carrying that item!";
        string actualOutput;
        using (StringWriter sw = new StringWriter())
        {
            Console.SetOut(sw);
            quit = Processor.ProcessWords(verb, noun, gameState);
            sw.Flush();

            actualOutput = CleanConsoleOutput(sw.ToString());
        }

        Assert.IsFalse(quit);
        Assert.AreEqual(expectedOutput, actualOutput);
    }

    [TestMethod]
    public void DropWithoutItem()
    {
        var gameState = SetupTestingGameState();
        var verb = ObjectFinder.GetWord(gameState.GameData.Words, "drop");
        bool quit;

        string expectedOutput = "Drop what??";
        string actualOutput;
        using (StringWriter sw = new StringWriter())
        {
            Console.SetOut(sw);
            quit = Processor.ProcessWords(verb, null, gameState);
            sw.Flush();

            actualOutput = CleanConsoleOutput(sw.ToString());
        }

        Assert.IsFalse(quit);
        Assert.AreEqual(expectedOutput, actualOutput);
    }

    [TestMethod]
    public void GoNoDirection()
    {
        GameState gameState = SetupTestingGameState();
        var gameDb = gameState.GameData;

        Word? verb = ObjectFinder.GetWord(gameDb.Words, "go");

        bool quit = false;
        string expectedOutput = "Go where??";
        string actualOutput;
        using (StringWriter sw = new StringWriter())
        {
            Console.SetOut(sw);
            quit = Processor.ProcessWords(verb, null, gameState);
            sw.Flush();

            actualOutput = CleanConsoleOutput(sw.ToString());
        }

        Assert.IsFalse(quit);
        Assert.AreEqual(expectedOutput, actualOutput);        
    }

    [TestMethod]
    public void GoBadDirection()
    {
        GameState gameState = SetupTestingGameState();
        var gameDb = gameState.GameData;
        var player = gameState.PlayerData;
        player.CurrentRoomId = gameDb.StartingRoomId;

        Word? verb = ObjectFinder.GetWord(gameDb.Words, "go");
        Word? noun = ObjectFinder.GetWord(gameDb.Words, "down");

        bool quit = false;
        string expectedOutput = "You can't go in that direction!!";
        string actualOutput;
        using (StringWriter sw = new StringWriter())
        {
            Console.SetOut(sw);
            quit = Processor.ProcessWords(verb, noun, gameState);
            sw.Flush();

            actualOutput = CleanConsoleOutput(sw.ToString());
        }

        Assert.IsFalse(quit);
        Assert.AreEqual(expectedOutput, actualOutput);        
    }

    [DataTestMethod]
    [DataRow("north")]
    [DataRow("south")]
    [DataRow("east")]
    [DataRow("west")]
    [DataRow("up")]
    [DataRow("down")]
    public void GoThroughAvailableExit(string direction)
    {
        var gameState = SetupTestingGameState();
        var currentRoom = ObjectFinder.GetRoom(gameState.GameData.Rooms, gameState.PlayerData.CurrentRoomId);

        switch (direction)
        {
            case "north":
                currentRoom.ExitIdNorth = 1;
                break;
            case "south":
                currentRoom.ExitIdSouth = 1;
                break;
            case "east":
                currentRoom.ExitIdEast = 1;
                break;
            case "west":
                currentRoom.ExitIdWest = 1;
                break;
            case "up":
                currentRoom.ExitIdUp = 1;
                break;
            case "down":
                currentRoom.ExitIdDown = 1;
                break;
        }

        var output = CaptureOutput(() =>
            Processor.ProcessWords(
                ObjectFinder.GetWord(gameState.GameData.Words, "go"),
                ObjectFinder.GetWord(gameState.GameData.Words, direction),
                gameState));

        Assert.IsFalse(output.Result);
        Assert.AreEqual(1, gameState.PlayerData.CurrentRoomId);
        StringAssert.Contains(output.Text, "Second Room");
    }

    [TestMethod]
    public void GoUnknownDirection()
    {
        var gameState = SetupTestingGameState();
        var output = CaptureOutput(() =>
            Processor.ProcessWords(
                ObjectFinder.GetWord(gameState.GameData.Words, "go"),
                new Word { Name = "northeast" },
                gameState));

        Assert.IsFalse(output.Result);
        Assert.AreEqual("You can't go in that direction!!", CleanConsoleOutput(output.Text));
    }

    [TestMethod]
    public void Help()
    {
        GameState gameState = SetupTestingGameState();
        var gameDb = gameState.GameData;

        Word? verb = ObjectFinder.GetWord(gameDb.Words, "help");

        bool quit = false;
        string expectedOutput = MessageFormatter.Help();
        string actualOutput;
        using (StringWriter sw = new StringWriter())
        {
            Console.SetOut(sw);
            quit = Processor.ProcessWords(verb, null, gameState);
            sw.Flush();

            actualOutput = CleanConsoleOutput(sw.ToString());
        }

        Assert.IsFalse(quit);
        Assert.AreEqual(expectedOutput, actualOutput);
    }

    [TestMethod]
    public void Look()
    {
        var gameState = SetupTestingGameState();
        var verb = ObjectFinder.GetWord(gameState.GameData.Words, "look");
        bool quit;

        string expectedOutput = "Starting Room\n" +
        "This is the first room. It's very interesting.\n" +
        "Also visible: smooth rock\nAvailable exits: (N)orth";
        string actualOutput;
        using (StringWriter sw = new StringWriter())
        {
            Console.SetOut(sw);
            quit = Processor.ProcessWords(verb, null, gameState);
            sw.Flush();

            actualOutput = CleanConsoleOutput(sw.ToString());
        }

        Assert.IsFalse(quit);
        Assert.AreEqual(expectedOutput, actualOutput);
    }

    [TestMethod]
    public void LookInventoryEmpty()
    {
        var gameState = SetupTestingGameState();
        var verb = ObjectFinder.GetWord(gameState.GameData.Words, "look");
        var noun = ObjectFinder.GetWord(gameState.GameData.Words, "inventory");
        bool quit;

        string expectedOutput = "You're not carrying anything!";
        string actualOutput;
        using (StringWriter sw = new StringWriter())
        {
            Console.SetOut(sw);
            quit = Processor.ProcessWords(verb, noun, gameState);
            sw.Flush();

            actualOutput = CleanConsoleOutput(sw.ToString());
        }

        Assert.IsFalse(quit);
        Assert.AreEqual(expectedOutput, actualOutput);
    }

    [TestMethod]
    public void LookInventoryFilled()
    {
        var gameState = SetupTestingGameState();
        var verb = ObjectFinder.GetWord(gameState.GameData.Words, "look");
        var noun = ObjectFinder.GetWord(gameState.GameData.Words, "inventory");
        var player = gameState.PlayerData;
        player.ItemInventory.Add(0);
        player.ItemInventory.Add(2);
        bool quit;

        string expectedOutput = "smooth rock, *small ruby*";
        string actualOutput;
        using (StringWriter sw = new StringWriter())
        {
            Console.SetOut(sw);
            quit = Processor.ProcessWords(verb, noun, gameState);
            sw.Flush();

            actualOutput = CleanConsoleOutput(sw.ToString());
        }

        Assert.IsFalse(quit);
        Assert.AreEqual(expectedOutput, actualOutput);
    }

    [TestMethod]
    public void LookItemInventory()
    {
        var gameState = SetupTestingGameState();
        var verb = ObjectFinder.GetWord(gameState.GameData.Words, "look");
        var noun = ObjectFinder.GetWord(gameState.GameData.Words, "ruby");
        var player = gameState.PlayerData;
        player.ItemInventory.Add(2);
        bool quit;

        string expectedOutput = "small ruby: A brilliant, red, precious stone.";
        string actualOutput;
        using (StringWriter sw = new StringWriter())
        {
            Console.SetOut(sw);
            quit = Processor.ProcessWords(verb, noun, gameState);
            sw.Flush();

            actualOutput = CleanConsoleOutput(sw.ToString());
        }

        Assert.IsFalse(quit);
        Assert.AreEqual(expectedOutput, actualOutput);
    }

    [TestMethod]
    public void LookItemRoom()
    {
        var gameState = SetupTestingGameState();
        var verb = ObjectFinder.GetWord(gameState.GameData.Words, "look");
        var noun = ObjectFinder.GetWord(gameState.GameData.Words, "rock");
        bool quit;

        string expectedOutput = "smooth rock: A very smooth rock.";
        string actualOutput;
        using (StringWriter sw = new StringWriter())
        {
            Console.SetOut(sw);
            quit = Processor.ProcessWords(verb, noun, gameState);
            sw.Flush();

            actualOutput = CleanConsoleOutput(sw.ToString());
        }

        Assert.IsFalse(quit);
        Assert.AreEqual(expectedOutput, actualOutput);
    }

    [TestMethod]
    public void LookItemNotPresent()
    {
        var gameState = SetupTestingGameState();
        var verb = ObjectFinder.GetWord(gameState.GameData.Words, "look");
        var noun = ObjectFinder.GetWord(gameState.GameData.Words, "ruby");
        bool quit;

        string expectedOutput = "You don't see that here!";
        string actualOutput;
        using (StringWriter sw = new StringWriter())
        {
            Console.SetOut(sw);
            quit = Processor.ProcessWords(verb, noun, gameState);
            sw.Flush();

            actualOutput = CleanConsoleOutput(sw.ToString());
        }

        Assert.IsFalse(quit);
        Assert.AreEqual(expectedOutput, actualOutput);
    }

    [TestMethod]
    public void QuitNoTreasuresStored()
    {
        var gameState = SetupTestingGameState();
        var verb = ObjectFinder.GetWord(gameState.GameData.Words, "quit");
        bool quit;

        string expectedOutput = "Such a quiter. BOOOOOOOO!\n" +
        "You have stored 0 out of 1 treasures in the treasure room.\nYou have a score of 0 %.";
        string actualOutput;
        using (StringWriter sw = new StringWriter())
        {
            Console.SetOut(sw);
            quit = Processor.ProcessWords(verb, null, gameState);
            sw.Flush();

            actualOutput = CleanConsoleOutput(sw.ToString());
        }

        Assert.IsTrue(quit);
        Assert.AreEqual(expectedOutput, actualOutput);
    }

    [TestMethod]
    public void ScoreAllTreasuresStored()
    {
        var gameState = SetupTestingGameState();
        var treasureRoom = ObjectFinder.GetRoom(gameState.GameData.Rooms, gameState.GameData.TreasureRoomId);
        treasureRoom.ContainedItemIds.Add(2);
        var verb = ObjectFinder.GetWord(gameState.GameData.Words, "score");
        bool quit;

        string expectedOutput = "You have stored 1 out of 1 treasures in the treasure room.\nYou have a score of 100 %.\n";
        expectedOutput += "CONGRATULATIONS!!! YOU WIN!!!";

        string actualOutput;
        using (StringWriter sw = new StringWriter())
        {
            Console.SetOut(sw);
            quit = Processor.ProcessWords(verb, null, gameState);
            sw.Flush();

            actualOutput = CleanConsoleOutput(sw.ToString());
        }

        Assert.IsTrue(quit);
        Assert.AreEqual(expectedOutput, actualOutput);
    }

    [TestMethod]
    public void ScoreNoTreasuresStored()
    {
        var gameState = SetupTestingGameState();
        var verb = ObjectFinder.GetWord(gameState.GameData.Words, "score");
        bool quit;

        string expectedOutput = "You have stored 0 out of 1 treasures in the treasure room.\nYou have a score of 0 %.";
        string actualOutput;
        using (StringWriter sw = new StringWriter())
        {
            Console.SetOut(sw);
            quit = Processor.ProcessWords(verb, null, gameState);
            sw.Flush();

            actualOutput = CleanConsoleOutput(sw.ToString());
        }

        Assert.IsFalse(quit);
        Assert.AreEqual(expectedOutput, actualOutput);
    }

    [TestMethod]
    public void Take()
    {
        var gameState = SetupTestingGameState();
        var verb = ObjectFinder.GetWord(gameState.GameData.Words, "take");
        var noun = ObjectFinder.GetWord(gameState.GameData.Words, "rock");
        bool quit;

        string expectedOutput = "You picked up: smooth rock";
        string actualOutput;
        using (StringWriter sw = new StringWriter())
        {
            Console.SetOut(sw);
            quit = Processor.ProcessWords(verb, noun, gameState);
            sw.Flush();

            actualOutput = CleanConsoleOutput(sw.ToString());
        }

        Assert.IsFalse(quit);
        Assert.AreEqual(expectedOutput, actualOutput);
    }

    [TestMethod]
    public void TakeAll()
    {
        var gameState = SetupTestingGameState();
        var verb = ObjectFinder.GetWord(gameState.GameData.Words, "take");
        var noun = ObjectFinder.GetWord(gameState.GameData.Words, "all");
        bool quit;

        string expectedOutput = "You picked all the items from the room!";
        string actualOutput;
        using (StringWriter sw = new StringWriter())
        {
            Console.SetOut(sw);
            quit = Processor.ProcessWords(verb, noun, gameState);
            sw.Flush();

            actualOutput = CleanConsoleOutput(sw.ToString());
        }

        Assert.IsFalse(quit);
        Assert.AreEqual(expectedOutput, actualOutput);
    }

    [TestMethod]
    public void TakeItemNotInRoom()
    {
        var gameState = SetupTestingGameState();
        var verb = ObjectFinder.GetWord(gameState.GameData.Words, "take");
        var noun = ObjectFinder.GetWord(gameState.GameData.Words, "stick");
        bool quit;

        string expectedOutput = "You don't see that here!";
        string actualOutput;
        using (StringWriter sw = new StringWriter())
        {
            Console.SetOut(sw);
            quit = Processor.ProcessWords(verb, noun, gameState);
            sw.Flush();

            actualOutput = CleanConsoleOutput(sw.ToString());
        }

        Assert.IsFalse(quit);
        Assert.AreEqual(expectedOutput, actualOutput);
    }

    [TestMethod]
    public void TakeUntakableItem()
    {
        var gameState = SetupTestingGameState();
        var room = ObjectFinder.GetRoom(gameState.GameData.Rooms, 0);
        room.ContainedItemIds.Add(5);
        var verb = ObjectFinder.GetWord(gameState.GameData.Words, "take");
        var noun = ObjectFinder.GetWord(gameState.GameData.Words, "tree");
        bool quit;

        string expectedOutput = "You can't take that!";
        string actualOutput;
        using (StringWriter sw = new StringWriter())
        {
            Console.SetOut(sw);
            quit = Processor.ProcessWords(verb, noun, gameState);
            sw.Flush();

            actualOutput = CleanConsoleOutput(sw.ToString());
        }

        Assert.IsFalse(quit);
        Assert.AreEqual(expectedOutput, actualOutput);
    }

    [TestMethod]
    public void TakeWithoutItem()
    {
        var gameState = SetupTestingGameState();
        var verb = ObjectFinder.GetWord(gameState.GameData.Words, "take");
        bool quit;

        string expectedOutput = "Take what??";
        string actualOutput;
        using (StringWriter sw = new StringWriter())
        {
            Console.SetOut(sw);
            quit = Processor.ProcessWords(verb, null, gameState);
            sw.Flush();

            actualOutput = CleanConsoleOutput(sw.ToString());
        }

        Assert.IsFalse(quit);
        Assert.AreEqual(expectedOutput, actualOutput);
    }

    [TestMethod]
    public void ActionRunsWhenAllConditionsAreMet()
    {
        var gameState = SetupTestingGameState();
        gameState.PlayerData.CurrentRoomId = 2;
        gameState.PlayerData.ItemInventory.Add(1);
        gameState.GameData.Actions[0].Conditions.Add(new ActionCondition
        {
            Condition = ActionConditions.PlayerHasItem,
            ObjectId = 1
        });

        var output = CaptureOutput(() =>
            Processor.ProcessWords(
                ObjectFinder.GetWord(gameState.GameData.Words, "chop"),
                ObjectFinder.GetWord(gameState.GameData.Words, "tree"),
                gameState));

        Assert.IsFalse(output.Result);
        Assert.AreEqual("TIMBER!", CleanConsoleOutput(output.Text));
    }

    [TestMethod]
    public void ActionDoesNotRunWhenPlayerLacksRequiredItem()
    {
        var gameState = SetupTestingGameState();
        gameState.PlayerData.CurrentRoomId = 2;
        gameState.GameData.Actions[0].Conditions.Add(new ActionCondition
        {
            Condition = ActionConditions.PlayerHasItem,
            ObjectId = 1
        });

        var output = CaptureOutput(() =>
            Processor.ProcessWords(
                ObjectFinder.GetWord(gameState.GameData.Words, "chop"),
                ObjectFinder.GetWord(gameState.GameData.Words, "tree"),
                gameState));

        Assert.IsFalse(output.Result);
        Assert.AreEqual(string.Empty, output.Text);
    }

    [TestMethod]
    public void ActionDoesNotRunWhenRequiredItemIsNotInCurrentRoom()
    {
        var gameState = SetupTestingGameState();

        var output = CaptureOutput(() =>
            Processor.ProcessWords(
                ObjectFinder.GetWord(gameState.GameData.Words, "chop"),
                ObjectFinder.GetWord(gameState.GameData.Words, "tree"),
                gameState));

        Assert.IsFalse(output.Result);
        Assert.AreEqual(string.Empty, output.Text);
    }

    [TestMethod]
    public void ActionMovesPlayerToTargetRoom()
    {
        var gameState = SetupTestingGameState();
        gameState.GameData.Actions.Clear();
        gameState.GameData.Actions.Add(new GameEngine.GameData.Action
        {
            TriggerVerbId = 20,
            TriggerNounId = 21,
            Commands =
            [
                new ActionCommand
                {
                    Command = ActionCommands.MoveToRoom,
                    ObjectId = 1
                }
            ]
        });

        var output = CaptureOutput(() =>
            Processor.ProcessWords(
                ObjectFinder.GetWord(gameState.GameData.Words, "chop"),
                ObjectFinder.GetWord(gameState.GameData.Words, "tree"),
                gameState));

        Assert.IsFalse(output.Result);
        Assert.AreEqual(1, gameState.PlayerData.CurrentRoomId);
        StringAssert.Contains(output.Text, "Second Room");
    }

    [TestMethod]
    public void ActionWithUnknownConditionStillRuns()
    {
        var gameState = SetupTestingGameState();
        gameState.GameData.Actions.Clear();
        gameState.GameData.Actions.Add(new GameEngine.GameData.Action
        {
            TriggerVerbId = 20,
            TriggerNounId = 21,
            Conditions =
            [
                new ActionCondition
                {
                    Condition = (ActionConditions)int.MaxValue
                }
            ],
            Commands =
            [
                new ActionCommand
                {
                    Command = ActionCommands.DisplayMessage,
                    ObjectId = 0
                }
            ]
        });

        var output = CaptureOutput(() =>
            Processor.ProcessWords(
                ObjectFinder.GetWord(gameState.GameData.Words, "chop"),
                ObjectFinder.GetWord(gameState.GameData.Words, "tree"),
                gameState));

        Assert.IsFalse(output.Result);
        Assert.AreEqual("TIMBER!", CleanConsoleOutput(output.Text));
    }

    [TestMethod]
    public void ActionWithUnknownCommandProducesNoOutput()
    {
        var gameState = SetupTestingGameState();
        gameState.GameData.Actions.Clear();
        gameState.GameData.Actions.Add(new GameEngine.GameData.Action
        {
            TriggerVerbId = 20,
            TriggerNounId = 21,
            Commands =
            [
                new ActionCommand
                {
                    Command = (ActionCommands)int.MaxValue
                }
            ]
        });

        var output = CaptureOutput(() =>
            Processor.ProcessWords(
                ObjectFinder.GetWord(gameState.GameData.Words, "chop"),
                ObjectFinder.GetWord(gameState.GameData.Words, "tree"),
                gameState));

        Assert.IsFalse(output.Result);
        Assert.AreEqual(string.Empty, output.Text);
    }

    [TestMethod]
    public void ActionDoesNotRunWhenVerbAndNounDoNotMatch()
    {
        var gameState = SetupTestingGameState();
        gameState.PlayerData.CurrentRoomId = 2;

        var output = CaptureOutput(() =>
            Processor.ProcessWords(
                ObjectFinder.GetWord(gameState.GameData.Words, "chop"),
                ObjectFinder.GetWord(gameState.GameData.Words, "stump"),
                gameState));

        Assert.IsFalse(output.Result);
        Assert.AreEqual(string.Empty, output.Text);
    }

    [TestMethod]
    public void UnknownVerbWithNounProducesNoOutput()
    {
        var gameState = SetupTestingGameState();
        var output = CaptureOutput(() =>
            Processor.ProcessWords(new Word { Name = "unknown" }, null, gameState));

        Assert.IsFalse(output.Result);
        Assert.AreEqual(string.Empty, output.Text);
    }

    [TestMethod]
    public void NullVerb()
    {
        GameState gameState = SetupTestingGameState();
        bool quit = false;
        // For some reason the actual output appends a carrage return and line feed.
        string expectedOutput = "You don't know how to do that!";
        string actualOutput;
        using (StringWriter sw = new StringWriter())
        {
            Console.SetOut(sw);
            quit = Processor.ProcessWords(null, null, gameState);
            sw.Flush();

            actualOutput = CleanConsoleOutput(sw.ToString());
        }

        Assert.IsFalse(quit);
        Assert.AreEqual(expectedOutput, actualOutput);
    }

    /// <summary>
    /// Takes <c>Console</c> output and removes the trailing EOL character(s).
    /// <para>EOL characters differ between Linux and Windows OSes. The removal makes output comparison possible.</para>
    /// </summary>
    /// <param name="consoleOutput"></param>
    /// <returns></returns>
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

    [ExcludeFromCodeCoverage]
    private static (bool Result, string Text) CaptureOutput(Func<bool> action)
    {
        TextWriter originalOutput = Console.Out;
        using StringWriter writer = new();
        try
        {
            Console.SetOut(writer);
            bool result = action();
            writer.Flush();
            return (result, writer.ToString());
        }
        finally
        {
            Console.SetOut(originalOutput);
        }
    }

    [ExcludeFromCodeCoverage]
    private GameState SetupTestingGameState()
    {
        GameState gameState = new GameState();
        Player player = new Player();
        GameDatabase gameDb = new GameDatabase();
        gameDb.Load(@"test_data/", "test_game_database.gge", true);
        gameState.PlayerData = player;
        gameState.GameData = gameDb;
        player.CurrentRoomId = gameDb.StartingRoomId;

        return gameState;
    }
}
