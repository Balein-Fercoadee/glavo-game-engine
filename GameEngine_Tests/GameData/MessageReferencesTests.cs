using GameDatabaseEditor.Utilities;
using GameEngine.GameData;
using GameAction = GameEngine.GameData.Action;

namespace GameEngine_Tests.GameData;

[TestClass]
public class MessageReferencesTests
{
    [TestMethod]
    public void FindsActionsDisplayingMessage()
    {
        var database = new GameDatabase();
        var action = new GameAction { Id = 7 };
        action.Commands.Add(new ActionCommand { Command = ActionCommands.DisplayMessage, ObjectId = 12 });
        action.Commands.Add(new ActionCommand { Command = ActionCommands.DisplayMessage, ObjectId = 12 });
        database.Actions.Add(action);
        var another = new GameAction { Id = 8 };
        another.Commands.Add(new ActionCommand { Command = ActionCommands.DisplayMessage, ObjectId = 12 });
        database.Actions.Add(another);

        CollectionAssert.AreEqual(new[] { "Action 7: command", "Action 8: command" },
            MessageReferences.Find(database, 12));
    }

    [TestMethod]
    public void IgnoresOtherIdsAndNonMessageCommands()
    {
        var database = new GameDatabase();
        var action = new GameAction();
        action.Commands.Add(new ActionCommand { Command = ActionCommands.DisplayMessage, ObjectId = 13 });
        action.Commands.Add(new ActionCommand { Command = ActionCommands.PutItemInRoom, ObjectId = 12 });
        action.Commands.Add(new ActionCommand { Command = ActionCommands.MoveToRoom, ObjectId = 12 });
        action.Conditions.Add(new ActionCondition { Condition = ActionConditions.PlayerHasItem, ObjectId = 12 });
        database.Actions.Add(action);

        Assert.AreEqual(0, MessageReferences.Find(database, 12).Count);
    }
}
