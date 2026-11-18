using GameDatabaseEditor.Utilities;
using GameEngine.GameData;

namespace GameEngine_Tests.GameData;

[TestClass]
public class WordReferencesTests
{
    [TestMethod]
    public void FindReturnsMatchingActionTriggers()
    {
        var database = new GameDatabase();
        database.Actions.Add(new GameEngine.GameData.Action
        {
            Id = 3,
            TriggerNounId = 16,
            TriggerVerbId = 17
        });
        database.Actions.Add(new GameEngine.GameData.Action
        {
            Id = 4,
            TriggerNounId = 17,
            TriggerVerbId = -1
        });

        var references = WordReferences.Find(database, 17);

        CollectionAssert.AreEqual(
            new[] { "Action 3: verb trigger", "Action 4: noun trigger" },
            references);
    }
}
