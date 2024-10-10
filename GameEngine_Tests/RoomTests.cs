using System;
using GameEngine.GameData;

namespace GameEngine_Tests;

[TestClass]
public class RoomTests
{
    [TestMethod]
    public void RoomAvailableExits()
    {
        GameDatabase gameDb = new GameDatabase();
        gameDb.Load(@"test_data\", "test_game_database.gge", true);
        
        Room room = gameDb.Rooms[0];
        var exits = room.AvailableExits();

        Assert.AreEqual(1, exits.Count);
        Assert.AreEqual("n", exits[0]);
        
        room = gameDb.Rooms[1];
        exits = room.AvailableExits();

        Assert.AreEqual(1, exits.Count);
        Assert.AreEqual("s", exits[0]);
    }
}
