using GameDatabaseEditor.Utilities;
using GameEngine.GameData;

namespace GameEngine_Tests.GameData;

[TestClass]
public class DatabaseValidationTests
{
    [TestMethod]
    public void EmptyDatabaseReportsAllFourErrors()
    {
        using var validation = new DatabaseValidation(new GameDatabase());

        CollectionAssert.AreEqual(new[]
        {
            "Add at least one room.",
            "Set the starting room to an existing room.",
            "Set the treasure room to an existing room.",
            "Add at least one item marked as treasure."
        }, validation.Errors);
    }

    [TestMethod]
    public void ErrorsUpdateAsRoomsSelectionsAndTreasuresChange()
    {
        var database = new GameDatabase();
        using var validation = new DatabaseValidation(database);

        database.Rooms.Add(new Room { Id = 3 });
        database.StartingRoomId = 3;
        database.TreasureRoomId = 99;
        database.Items.Add(new Item { IsTreasure = false });

        CollectionAssert.AreEqual(new[]
        {
            "Set the treasure room to an existing room.",
            "Add at least one item marked as treasure."
        }, validation.Errors);

        database.TreasureRoomId = 3;
        database.Items[0] = new Item { IsTreasure = true };
        Assert.AreEqual(0, validation.Errors.Count);

        database.Items.Clear();
        database.Rooms.Clear();
        Assert.AreEqual(4, validation.Errors.Count);
    }

    [TestMethod]
    public void DisposeStopsObservingPreviousDatabase()
    {
        var database = new GameDatabase();
        var validation = new DatabaseValidation(database);
        validation.Dispose();

        database.Rooms.Add(new Room { Id = 1 });
        database.StartingRoomId = 1;
        database.Items.Add(new Item { IsTreasure = true });

        Assert.AreEqual(4, validation.Errors.Count);
    }

    [TestMethod]
    public void MissingRoomReferencesAreInvalidEvenWhenAnotherRoomExists()
    {
        var database = new GameDatabase
        {
            StartingRoomId = 10,
            TreasureRoomId = 11
        };
        database.Rooms.Add(new Room { Id = 1 });
        database.Items.Add(new Item { IsTreasure = true });
        using var validation = new DatabaseValidation(database);

        CollectionAssert.AreEqual(new[]
        {
            "Set the starting room to an existing room.",
            "Set the treasure room to an existing room."
        }, validation.Errors);
    }
}
