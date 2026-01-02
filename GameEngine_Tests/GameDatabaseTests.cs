using GameEngine.GameData;
using Action = GameEngine.GameData.Action;

namespace GameEngine_Tests;

[TestClass]
public class GameDatabaseTests
{

    [TestMethod]
    public void DbSave()
    {
        GameDatabase gameDb = new GameDatabase();
        Room room = new Room() { Id = 1001 };

        gameDb.Rooms.Add(room);
        gameDb.Save(@"test_data/", "game_database_test_save.gge", true);

        File.Delete(@"test_data/game_database_test_save.gge");
    }

    [TestMethod]
    public void DbLoad()
    {
        GameDatabase gameDb = new GameDatabase();
        gameDb.Load(@"test_data/", "test_game_database.gge", true);

        Assert.AreEqual(4, gameDb.Rooms.Count);
        Assert.AreEqual(8, gameDb.Items.Count);
        Assert.AreEqual(1, gameDb.Actions.Count);
        Assert.AreEqual(0, gameDb.StartingRoomId);

        Room room1 = gameDb.Rooms.First(r => r.Id == 0);
        Assert.AreEqual("Starting Room", room1.Name);

        Room room2 = gameDb.Rooms.First(r => r.Id == 1);
        Assert.AreEqual("Second Room", room2.Name);

        Action action1 = gameDb.Actions.First(a => a.Id == 0);
        Assert.AreEqual(1, action1.Commands.Count);
        Assert.AreEqual(1, action1.Conditions.Count);
    }
}