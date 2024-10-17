using GameEngine.GameData;
using GameEngine.Utilities;

namespace GameEngine_Tests.Utilities;

[TestClass]
public class ObjectFinderTests
{
    [TestMethod]
    public void ItemsFoundListIds()
    {
        GameDatabase gameDb = new GameDatabase();
        gameDb.Load(@"test_data/", "test_game_database.gge", true);

        var returnedItems = ObjectFinder.GetItems(gameDb.Items, new List<int>() { 1, 2 });

        Assert.AreEqual(2, returnedItems.Count);
    }

    [TestMethod]
    public void ItemFoundId()
    {
        GameDatabase gameDb = new GameDatabase();
        gameDb.Load(@"test_data/", "test_game_database.gge", true);

        var returnedItem = ObjectFinder.GetItem(gameDb.Items, 0);

        Assert.IsNotNull(returnedItem);
        Assert.AreEqual(0, returnedItem.Id);
    }

    [TestMethod]
    public void ItemFoundShortName()
    {
        GameDatabase gameDb = new GameDatabase();
        gameDb.Load(@"test_data/", "test_game_database.gge", true);

        var returnedItem = ObjectFinder.GetItem(gameDb.Items, "rock");

        Assert.IsNotNull(returnedItem);
        Assert.AreEqual(0, returnedItem.Id);
    }

    [TestMethod]
    public void MessageFoundId()
    {
        GameDatabase gameDb = new GameDatabase();
        gameDb.Load(@"test_data/", "test_game_database.gge", true);

        var returnedMessage = ObjectFinder.GetMessage(gameDb.Messages, 0);

        Assert.IsNotNull(returnedMessage);
        Assert.AreEqual("TIMBER!", returnedMessage.Text);
    }

    [TestMethod]
    public void RoomFoundId()
    {
        GameDatabase gameDb = new GameDatabase();
        gameDb.Load(@"test_data/", "test_game_database.gge", true);

        var returnedRoom = ObjectFinder.GetRoom(gameDb.Rooms, 1);

        Assert.AreEqual(1, returnedRoom.Id);
        Assert.AreEqual("Second Room", returnedRoom.Name);
    }

    [TestMethod]
    public void WordFoundString()
    {
        GameDatabase gameDb = new GameDatabase();
        gameDb.Load(@"test_data/", "test_game_database.gge", true);

        var returnedWord = ObjectFinder.GetWord(gameDb.Words, "n");

        Assert.IsNotNull(returnedWord);
        Assert.AreEqual(returnedWord.Name, "north");
    }

}
