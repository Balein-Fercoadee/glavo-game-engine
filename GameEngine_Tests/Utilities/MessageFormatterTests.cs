using GameEngine;
using GameEngine.GameData;
using GameEngine.Utilities;

namespace GameEngine_Tests.Utilities;

[TestClass]
public class MessageFormatterTests
{
    [TestMethod]
    public void Error()
    {
        List<string> errorList = ["This is error 1", "This is error 2"];
        string errors = string.Join("\n", errorList);

        string expectedMessage = string.Empty;
        expectedMessage += "\n";
        expectedMessage += Constants.SETUP_ERROR_HEADER_TEXT + "\n";
        expectedMessage += errors + "\n";
        expectedMessage += "\n";
        expectedMessage += Constants.LOADING_HELP_TEXT + "\n";

        string actualMessage = MessageFormatter.Error(errorList);

        Assert.AreEqual(expectedMessage, actualMessage);
    }

    [TestMethod]
    public void Exits()
    {
        GameDatabase gameDb = new GameDatabase();
        gameDb.Load(@"test_data/", "test_game_database.gge", true);      
        Room roomN = ObjectFinder.GetRoom(gameDb.Rooms, 0);
        Room roomS = ObjectFinder.GetRoom(gameDb.Rooms, 1);

        string exitN = MessageFormatter.RoomAvailableExits(roomN.AvailableExits());
        string exitES = MessageFormatter.RoomAvailableExits(roomS.AvailableExits());

        Assert.AreEqual("(N)orth", exitN);
        Assert.AreEqual("(E)ast, (S)outh", exitES);
    }

    [TestMethod]
    public void GameLoaded()
    {
        GameDatabase gameDb = new GameDatabase();
        gameDb.Load(@"test_data/", "test_game_database.gge", true);
        string expectedOutput = string.Empty;
        expectedOutput += "\n";
        expectedOutput += "--------------------\n";
        expectedOutput += "Test Game Database\n";
        expectedOutput += "This game is used to test the features of the game engine.\n";
        expectedOutput += "\n";

        string gameLoaded = MessageFormatter.GameLoaded(gameDb);

        Assert.AreEqual(expectedOutput, gameLoaded);
    }

    [TestMethod]
    public void Help()
    {
        string expectedOutput = string.Empty;
        expectedOutput += "Try these words (there are more than just these!):\n";
        expectedOutput += Constants.HELP_STANDARD_VERBS + "\n";
        expectedOutput += Constants.HELP_STANDARD_DIRECTIONS + "\n";
        expectedOutput += Constants.HELP_STANDARD_MISC + "\n";
        expectedOutput += "All items have some usefulness. You need to find out how to use them.\n";
        expectedOutput += "Items enclosed in '*' are treasures (ex. *large diamond*). Collect them!\n";

        string actualOutput = MessageFormatter.Help();

        Assert.AreEqual(expectedOutput, actualOutput);
    }

    [TestMethod]
    public void InventoryEmpty()
    {
        GameDatabase gameDb = new GameDatabase();
        gameDb.Load(@"test_data/", "test_game_database.gge", true);
        Player player = new Player();
        string expectedOutput = "You're not carrying anything!";

        string actualOutput = MessageFormatter.Inventory(player, gameDb);
        Assert.AreEqual(actualOutput, expectedOutput);
    }

    [TestMethod]
    public void InventoryWithItem()
    {
        GameDatabase gameDb = new GameDatabase();
        gameDb.Load(@"test_data/", "test_game_database.gge", true);
        Player player = new Player();
        player.ItemInventory.Add(0);
        player.ItemInventory.Add(2);
        string expectedOutput = "smooth rock, *small ruby*";
        string actualOutput = MessageFormatter.Inventory(player, gameDb);

        Assert.AreEqual(expectedOutput, actualOutput);
    }


    [TestMethod]
    public void Look()
    {
        GameDatabase gameDb = new GameDatabase();
        gameDb.Load(@"test_data/", "test_game_database.gge", true);      
        Player player = new Player();

        string look = MessageFormatter.Look(1, gameDb, player, false);
        string expectedOutput = "Second Room\nThe is the second room. It's so much more interesting.\nAlso visible: heavy stick, *small ruby*\nAvailable exits: (E)ast, (S)outh";

        Assert.AreEqual(expectedOutput, look);
    }
}
