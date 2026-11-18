using GameDatabaseEditor.Utilities;
using GameEngine.GameData;
using GameAction = GameEngine.GameData.Action;

namespace GameEngine_Tests.GameData;

[TestClass]
public class ActionEditorValidationTests
{
    [TestMethod]
    public void RequiresCorrectlyTypedExistingTriggers()
    {
        var database = new GameDatabase();
        database.Words.Add(new Word { Id = 1, WordType = WordTypes.Verb });
        database.Words.Add(new Word { Id = 2, WordType = WordTypes.Noun });
        var action = new GameAction { TriggerVerbId = 2, TriggerNounId = 1 };

        Assert.AreEqual("Select an existing trigger verb.", ActionEditorValidation.Validate(database, action));
        action.TriggerVerbId = 1;
        Assert.AreEqual("Select an existing trigger noun.", ActionEditorValidation.Validate(database, action));
        action.TriggerNounId = 2;
        Assert.IsNull(ActionEditorValidation.Validate(database, action));
        database.Words.Add(new Word { Id = -1, WordType = WordTypes.Verb });
        action.TriggerVerbId = -1;
        Assert.IsNotNull(ActionEditorValidation.Validate(database, action));
    }

    [TestMethod]
    public void RequiresTargetsOfCorrectTypeAndRejectsUnknownOperations()
    {
        var database = new GameDatabase();
        database.Words.Add(new Word { Id = 1, WordType = WordTypes.Verb });
        database.Words.Add(new Word { Id = 2, WordType = WordTypes.Noun });
        database.Items.Add(new Item { Id = 3 });
        database.Messages.Add(new Message { Id = 4 });
        database.Rooms.Add(new Room { Id = 5 });
        var action = new GameAction { TriggerVerbId = 1, TriggerNounId = 2 };
        action.Conditions.Add(new ActionCondition { Condition = ActionConditions.PlayerHasItem, ObjectId = 4 });
        Assert.IsNotNull(ActionEditorValidation.Validate(database, action));
        action.Conditions[0].ObjectId = 3;
        action.Commands.Add(new ActionCommand { Command = ActionCommands.DisplayMessage, ObjectId = 3 });
        Assert.IsNotNull(ActionEditorValidation.Validate(database, action));
        action.Commands[0].ObjectId = 4;
        action.Commands.Add(new ActionCommand { Command = ActionCommands.MoveToRoom, ObjectId = 5 });
        action.Commands.Add(new ActionCommand { Command = ActionCommands.PutItemInRoom, ObjectId = 3 });
        action.Commands.Add(new ActionCommand { Command = ActionCommands.RemoveItemFromRoom, ObjectId = 3 });
        Assert.IsNull(ActionEditorValidation.Validate(database, action));
        action.Commands[0].Command = (ActionCommands)999;
        Assert.IsNotNull(ActionEditorValidation.Validate(database, action));
        action.Commands[0].Command = ActionCommands.DisplayMessage;
        action.Conditions[0].Condition = (ActionConditions)999;
        Assert.IsNotNull(ActionEditorValidation.Validate(database, action));
    }
}
