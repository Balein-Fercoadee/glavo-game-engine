using System.Collections.ObjectModel;
using GameDatabaseEditor.Utilities;
using GameEngine.GameData;
using GameAction = GameEngine.GameData.Action;

namespace GameEngine_Tests.GameData;

[TestClass]
public class ActionEditSessionTests
{
    [TestMethod]
    public void DraftIsDeeplyIsolatedAndSaveReplacesOriginal()
    {
        var original = new GameAction { Id = 4, Description = "Open", TriggerVerbId = 2, TriggerNounId = 3 };
        original.Commands.Add(new ActionCommand { Command = ActionCommands.DisplayMessage, ObjectId = 7 });
        original.Conditions.Add(new ActionCondition { Condition = ActionConditions.PlayerHasItem, ObjectId = 8 });
        var actions = new ObservableCollection<GameAction> { original };
        var session = new ActionEditSession(actions, original);

        session.Draft.Description = "Unlock";
        session.Draft.Commands[0].ObjectId = 9;
        session.Draft.Conditions[0].ObjectId = 10;
        session.Draft.Commands.Add(new ActionCommand { Command = ActionCommands.MoveToRoom, ObjectId = 11 });

        Assert.AreEqual("Open", original.Description);
        Assert.AreEqual(7, original.Commands[0].ObjectId);
        Assert.AreEqual(8, original.Conditions[0].ObjectId);
        Assert.AreEqual(1, original.Commands.Count);
        Assert.IsTrue(session.Save());
        Assert.AreSame(session.Draft, actions[0]);
        Assert.AreEqual(4, actions[0].Id);
        Assert.AreEqual(2, actions[0].TriggerVerbId);
        Assert.AreEqual(3, actions[0].TriggerNounId);
        Assert.AreEqual(2, actions[0].Commands.Count);
        Assert.AreEqual(10, actions[0].Conditions[0].ObjectId);
    }

    [TestMethod]
    public void SaveFailsIfOriginalWasRemoved()
    {
        var original = new GameAction();
        var actions = new ObservableCollection<GameAction> { original };
        var session = new ActionEditSession(actions, original);
        actions.Remove(original);

        Assert.IsFalse(session.Save());
        Assert.AreEqual(0, actions.Count);
    }
}
