using GameDatabaseEditor.Utilities;
using GameEngine.GameData;

namespace GameEngine_Tests.GameData;

[TestClass]
public class RoomEditSessionTests
{
    [TestMethod]
    public void DraftChangesDoNotAffectOriginalUntilSaved()
    {
        var room = new Room
        {
            Id = 42, Name = "Original", Description = "Original description",
            ExitIdNorth = 1, ExitIdSouth = 2, ExitIdEast = 3,
            ExitIdWest = 4, ExitIdUp = 5, ExitIdDown = 6
        };
        room.ContainedItemIds.Add(99);
        var session = new RoomEditSession(room);
        var draft = session.Draft;

        Assert.AreEqual(room.Id, draft.Id);
        Assert.AreEqual(room.Name, draft.Name);
        Assert.AreEqual(room.Description, draft.Description);
        CollectionAssert.AreEqual(new[] { 99 }, draft.ContainedItemIds.ToArray());
        Assert.AreNotSame(room.ContainedItemIds, draft.ContainedItemIds);
        CollectionAssert.AreEqual(
            new[] { 1, 2, 3, 4, 5, 6 },
            new[] { draft.ExitIdNorth, draft.ExitIdSouth, draft.ExitIdEast, draft.ExitIdWest, draft.ExitIdUp, draft.ExitIdDown });

        draft.Name = "Edited";
        draft.Description = "Edited description";
        draft.ExitIdNorth = -1;
        draft.ExitIdSouth = 12;
        draft.ExitIdEast = 13;
        draft.ExitIdWest = 14;
        draft.ExitIdUp = 15;
        draft.ExitIdDown = 16;
        draft.ContainedItemIds.Clear();
        draft.ContainedItemIds.Add(17);
        draft.ContainedItemIds.Add(18);

        Assert.AreEqual("Original", room.Name);
        Assert.AreEqual("Original description", room.Description);
        CollectionAssert.AreEqual(new[] { 99 }, room.ContainedItemIds.ToArray());
        CollectionAssert.AreEqual(
            new[] { 1, 2, 3, 4, 5, 6 },
            new[] { room.ExitIdNorth, room.ExitIdSouth, room.ExitIdEast, room.ExitIdWest, room.ExitIdUp, room.ExitIdDown });

        var originalItemIds = room.ContainedItemIds;
        session.Save();

        Assert.AreEqual(42, room.Id);
        Assert.AreEqual("Edited", room.Name);
        Assert.AreEqual("Edited description", room.Description);
        CollectionAssert.AreEqual(
            new[] { -1, 12, 13, 14, 15, 16 },
            new[] { room.ExitIdNorth, room.ExitIdSouth, room.ExitIdEast, room.ExitIdWest, room.ExitIdUp, room.ExitIdDown });
        Assert.AreSame(originalItemIds, room.ContainedItemIds);
        CollectionAssert.AreEqual(new[] { 17, 18 }, room.ContainedItemIds.ToArray());
    }

    [TestMethod]
    public void SavingEmptyDraftClearsOriginalItemIds()
    {
        var room = new Room();
        room.ContainedItemIds.Add(99);
        var session = new RoomEditSession(room);
        session.Draft.ContainedItemIds.Clear();

        session.Save();

        Assert.AreEqual(0, room.ContainedItemIds.Count);
    }
}
