using GameDatabaseEditor.Utilities;
using GameEngine.GameData;
using GameAction = GameEngine.GameData.Action;

namespace GameEngine_Tests.GameData;

[TestClass]
public class ItemReferencesTests
{
    [TestMethod]
    public void FindsRoomAndItemActionReferences()
    {
        var database = new GameDatabase();
        var room = new Room { Id = 3, Name = "Hall" };
        room.ContainedItemIds.Add(12);
        database.Rooms.Add(room);
        var action = new GameAction { Id = 7 };
        action.Conditions.Add(new ActionCondition { Condition = ActionConditions.PlayerHasItem, ObjectId = 12 });
        action.Commands.Add(new ActionCommand { Command = ActionCommands.PutItemInRoom, ObjectId = 12 });
        database.Actions.Add(action);

        CollectionAssert.AreEqual(
            new[] { "Room 3: Hall", "Action 7: condition", "Action 7: command" },
            ItemReferences.Find(database, 12));

        action.Conditions[0].Condition = ActionConditions.PlayerInRoomWithItem;
        action.Commands[0].Command = ActionCommands.RemoveItemFromRoom;
        CollectionAssert.AreEqual(
            new[] { "Room 3: Hall", "Action 7: condition", "Action 7: command" },
            ItemReferences.Find(database, 12));
    }

    [TestMethod]
    public void IgnoresOtherIdsAndNonItemCommands()
    {
        var database = new GameDatabase();
        var room = new Room();
        room.ContainedItemIds.Add(13);
        database.Rooms.Add(room);
        var action = new GameAction();
        action.Conditions.Add(new ActionCondition { Condition = ActionConditions.PlayerHasItem, ObjectId = 13 });
        action.Commands.Add(new ActionCommand { Command = ActionCommands.RemoveItemFromRoom, ObjectId = 13 });
        action.Commands.Add(new ActionCommand { Command = ActionCommands.MoveToRoom, ObjectId = 12 });
        action.Commands.Add(new ActionCommand { Command = ActionCommands.DisplayMessage, ObjectId = 12 });
        database.Actions.Add(action);

        Assert.AreEqual(0, ItemReferences.Find(database, 12).Count);
    }
}
