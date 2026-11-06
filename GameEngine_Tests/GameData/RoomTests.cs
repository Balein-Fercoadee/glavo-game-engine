using System;
using GameEngine.GameData;

namespace GameEngine_Tests.GameData;

[TestClass]
public class RoomTests
{
    [TestMethod]
    public void RoomAvailableExits()
    {
        GameDatabase gameDb = new GameDatabase();
        gameDb.Load(@"test_data/", "test_game_database.gge", true);

        Room room = gameDb.Rooms[0];
        var exits = room.AvailableExits();

        Assert.AreEqual(1, exits.Count);
        Assert.AreEqual("n", exits[0]);

        room = gameDb.Rooms[1];
        exits = room.AvailableExits();

        Assert.AreEqual(2, exits.Count);
        Assert.AreEqual("e", exits[0]);
    }

    [TestMethod]
    public void RoomAvailableExitsIncludesEveryDirection()
    {
        Room room = new Room
        {
            ExitIdEast = 1,
            ExitIdNorth = 2,
            ExitIdSouth = 3,
            ExitIdWest = 4,
            ExitIdUp = 5,
            ExitIdDown = 6
        };

        CollectionAssert.AreEqual(
            new[] { "e", "n", "s", "w", "u", "d" },
            room.AvailableExits());
    }

    [TestMethod]
    public void RoomCloneCopiesPropertiesAndContainedItems()
    {
        Room room = new Room
        {
            Id = 7,
            Name = "Test room",
            Description = "A room to clone.",
            ExitIdEast = 1,
            ExitIdNorth = 2,
            ExitIdSouth = 3,
            ExitIdWest = 4,
            ExitIdUp = 5,
            ExitIdDown = 6
        };
        room.ContainedItemIds.Add(10);
        room.ContainedItemIds.Add(11);

        Room clone = (Room)room.Clone();

        Assert.AreNotSame(room, clone);
        Assert.AreEqual(room.Id, clone.Id);
        Assert.AreEqual(room.Name, clone.Name);
        Assert.AreEqual(room.Description, clone.Description);
        Assert.AreEqual(room.ExitIdEast, clone.ExitIdEast);
        Assert.AreEqual(room.ExitIdNorth, clone.ExitIdNorth);
        Assert.AreEqual(room.ExitIdSouth, clone.ExitIdSouth);
        Assert.AreEqual(room.ExitIdWest, clone.ExitIdWest);
        Assert.AreEqual(room.ExitIdUp, clone.ExitIdUp);
        Assert.AreEqual(room.ExitIdDown, clone.ExitIdDown);
        CollectionAssert.AreEqual(room.ContainedItemIds.ToList(), clone.ContainedItemIds.ToList());
        Assert.AreNotSame(room.ContainedItemIds, clone.ContainedItemIds);
    }

    [TestMethod]
    public void RoomDeserializeThrowsWhenJsonIsNull()
    {
        InvalidOperationException exception = Assert.ThrowsException<InvalidOperationException>(
            () => Room.Deserialize("null"));

        Assert.AreEqual("The room could not be cloned.", exception.Message);
    }
}
