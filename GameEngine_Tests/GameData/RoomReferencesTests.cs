using GameDatabaseEditor.Utilities;
using GameEngine.GameData;
using GameAction = GameEngine.GameData.Action;

namespace GameEngine_Tests.GameData;

[TestClass]
public class RoomReferencesTests
{
    [TestMethod]
    public void ReferencedRoomCannotBeRemoved()
    {
        var database = new GameDatabase { StartingRoomId = 12, TreasureRoomId = 12 };
        var room = new Room { Id = 12 };
        var other = new Room { Id = 13, ExitIdNorth = 12 };
        database.Rooms.Add(room);
        database.Rooms.Add(other);
        var first = new GameAction { Id = 7 };
        first.Commands.Add(new ActionCommand { Command = ActionCommands.MoveToRoom, ObjectId = 12 });
        first.Commands.Add(new ActionCommand { Command = ActionCommands.MoveToRoom, ObjectId = 12 });
        var second = new GameAction { Id = 8 };
        second.Commands.Add(new ActionCommand { Command = ActionCommands.MoveToRoom, ObjectId = 12 });
        database.Actions.Add(first);
        database.Actions.Add(second);

        CollectionAssert.AreEqual(new[] { "Action 7: command", "Action 8: command" },
            RoomReferences.Find(database, 12));
        Assert.ThrowsException<InvalidOperationException>(() => GeneralUtilities.RemoveRoom(database, room));
        Assert.IsTrue(database.Rooms.Contains(room));
        Assert.AreEqual(12, other.ExitIdNorth);
        Assert.AreEqual(12, database.StartingRoomId);
        Assert.AreEqual(12, database.TreasureRoomId);
        Assert.AreEqual(2, first.Commands.Count);
        Assert.AreEqual(1, second.Commands.Count);
    }

    [TestMethod]
    public void UnrelatedActionIdsDoNotBlockRemovalOrRoomCleanup()
    {
        var database = new GameDatabase { StartingRoomId = 12, TreasureRoomId = 12 };
        var room = new Room { Id = 12 };
        var other = new Room
        {
            Id = 13, ExitIdNorth = 12, ExitIdSouth = 12, ExitIdEast = 12,
            ExitIdWest = 12, ExitIdUp = 12, ExitIdDown = 12
        };
        database.Rooms.Add(room);
        database.Rooms.Add(other);
        var action = new GameAction();
        action.Commands.Add(new ActionCommand { Command = ActionCommands.MoveToRoom, ObjectId = 13 });
        action.Commands.Add(new ActionCommand { Command = ActionCommands.DisplayMessage, ObjectId = 12 });
        action.Commands.Add(new ActionCommand { Command = ActionCommands.PutItemInRoom, ObjectId = 12 });
        action.Conditions.Add(new ActionCondition { Condition = ActionConditions.PlayerInRoomWithItem, ObjectId = 12 });
        database.Actions.Add(action);

        Assert.AreEqual(0, RoomReferences.Find(database, 12).Count);
        GeneralUtilities.RemoveRoom(database, room);

        Assert.IsFalse(database.Rooms.Contains(room));
        Assert.AreEqual(-1, database.StartingRoomId);
        Assert.AreEqual(-1, database.TreasureRoomId);
        CollectionAssert.AreEqual(new[] { -1, -1, -1, -1, -1, -1 },
            new[] { other.ExitIdNorth, other.ExitIdSouth, other.ExitIdEast, other.ExitIdWest, other.ExitIdUp, other.ExitIdDown });
        Assert.AreEqual(3, action.Commands.Count);
    }
}
