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

        Assert.AreEqual("Original", room.Name);
        Assert.AreEqual("Original description", room.Description);
        CollectionAssert.AreEqual(
            new[] { 1, 2, 3, 4, 5, 6 },
            new[] { room.ExitIdNorth, room.ExitIdSouth, room.ExitIdEast, room.ExitIdWest, room.ExitIdUp, room.ExitIdDown });

        session.Save();

        Assert.AreEqual(42, room.Id);
        Assert.AreEqual("Edited", room.Name);
        Assert.AreEqual("Edited description", room.Description);
        CollectionAssert.AreEqual(
            new[] { -1, 12, 13, 14, 15, 16 },
            new[] { room.ExitIdNorth, room.ExitIdSouth, room.ExitIdEast, room.ExitIdWest, room.ExitIdUp, room.ExitIdDown });
        CollectionAssert.AreEqual(new[] { 99 }, room.ContainedItemIds.ToArray());
    }
}
