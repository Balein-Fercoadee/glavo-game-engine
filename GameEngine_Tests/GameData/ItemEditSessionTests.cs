using System.Collections.ObjectModel;
using GameDatabaseEditor.Utilities;
using GameEngine.GameData;

namespace GameEngine_Tests.GameData;

[TestClass]
public class ItemEditSessionTests
{
    [TestMethod]
    public void DraftIsIsolatedUntilSavedAndReplacesItemInCollection()
    {
        var original = new Item
        {
            Id = 4, Name = "Key", ShortName = "key", Description = "Old",
            CanTake = true, IsTreasure = false
        };
        var items = new ObservableCollection<Item> { original };
        var session = new ItemEditSession(items, original);

        Assert.AreEqual(original.Id, session.Draft.Id);
        Assert.AreEqual(original.Name, session.Draft.Name);
        Assert.AreEqual(original.ShortName, session.Draft.ShortName);
        Assert.AreEqual(original.Description, session.Draft.Description);
        Assert.AreEqual(original.CanTake, session.Draft.CanTake);
        Assert.AreEqual(original.IsTreasure, session.Draft.IsTreasure);

        session.Draft.Name = "Coin";
        session.Draft.ShortName = "coin";
        session.Draft.Description = "New";
        session.Draft.CanTake = false;
        session.Draft.IsTreasure = true;

        Assert.AreEqual("Key", items[0].Name);
        Assert.AreEqual("Old", original.Description);
        Assert.IsTrue(session.Save());
        Assert.AreSame(session.Draft, items[0]);
        Assert.AreEqual(4, items[0].Id);
        Assert.AreEqual("Coin", items[0].Name);
        Assert.AreEqual("coin", items[0].ShortName);
        Assert.AreEqual("New", items[0].Description);
        Assert.IsFalse(items[0].CanTake);
        Assert.IsTrue(items[0].IsTreasure);
    }

    [TestMethod]
    public void SaveFailsIfOriginalWasRemoved()
    {
        var original = new Item();
        var items = new ObservableCollection<Item> { original };
        var session = new ItemEditSession(items, original);
        items.Remove(original);

        Assert.IsFalse(session.Save());
        Assert.AreEqual(0, items.Count);
    }
}
