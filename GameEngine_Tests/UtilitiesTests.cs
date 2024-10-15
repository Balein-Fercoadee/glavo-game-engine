using GameEngine.GameData;
using GameEngine.Utilities;

namespace GameEngine_Tests;

[TestClass]
public class UtilitiesTests
{
    [TestMethod]
    public void ObjectFinderItemFound()
    {
        GameDatabase gameDb = new GameDatabase();
        gameDb.Load(@"test_data/", "test_game_database.gge", true);

        var returnedItems = ObjectFinder.GetItems(gameDb.Items, new List<int>() { 1, 2 });

        Assert.AreEqual(2, returnedItems.Count);
    }

    [TestMethod]
    public void ObjectFinderRoomFound()
    {
        GameDatabase gameDb = new GameDatabase();
        gameDb.Load(@"test_data/", "test_game_database.gge", true);

        var returnedRoom = ObjectFinder.GetRoom(gameDb.Rooms, 1);

        Assert.AreEqual(1, returnedRoom.Id);
        Assert.AreEqual("Second Room", returnedRoom.Name);
    }

    [TestMethod]
    public void ObjectFinderWordFound()
    {
        GameDatabase gameDb = new GameDatabase();
        gameDb.Load(@"test_data/", "test_game_database.gge", true);

        var returnedWord = ObjectFinder.GetWord(gameDb.Words, "n");

        Assert.IsNotNull(returnedWord);
        Assert.AreEqual(returnedWord.Name, "north");
    }

    [TestMethod]
    public void MessageFormatterExits()
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
    public void MessageFormatterLook()
    {
        GameDatabase gameDb = new GameDatabase();
        gameDb.Load(@"test_data/", "test_game_database.gge", true);      
        Player player = new Player();

        string look = MessageFormatter.Look(1, gameDb, player, false);
        string expectedOutput = "Second Room\nThe is the second room. It's so much more interesting.\nAlso visible: heavy stick, *small ruby*\nAvailable exits: (E)ast, (S)outh";

        Assert.AreEqual(expectedOutput, look);
    }
}
