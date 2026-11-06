using System.Text.Json;
using GameEngine.GameData;

namespace GameEngine_Tests.GameData;

[TestClass]
public class GameStateTests
{
    [TestMethod]
    public void ConstructorInitializesGameDataAndPlayerData()
    {
        GameState gameState = new();

        Assert.IsNotNull(gameState.GameData);
        Assert.IsNotNull(gameState.PlayerData);
    }

    [TestMethod]
    public void LoadRestoresGameDataAndPlayerData()
    {
        GameState savedState = CreateSavedState();
        using TemporarySaveFile saveFile = new(JsonSerializer.Serialize(savedState));
        GameState gameState = new();

        gameState.Load(saveFile.DirectoryPath, saveFile.FileName);

        Assert.AreEqual("Saved game", gameState.GameData.Name);
        Assert.AreEqual("Saved description", gameState.GameData.Description);
        Assert.AreEqual(12, gameState.PlayerData.CurrentRoomId);
        CollectionAssert.AreEqual(new[] { 4, 8 }, gameState.PlayerData.ItemInventory);
        CollectionAssert.AreEqual(new[] { 12, 15 }, gameState.PlayerData.VisitedRoomIds);
    }

    [TestMethod]
    public void LoadWritesDebugInformationWhenEnabled()
    {
        GameState savedState = CreateSavedState();
        using TemporarySaveFile saveFile = new(JsonSerializer.Serialize(savedState));
        GameState gameState = new();
        TextWriter originalOutput = Console.Out;
        using StringWriter output = new();

        try
        {
            Console.SetOut(output);
            gameState.Load(saveFile.DirectoryPath, saveFile.FileName, debug: true);
        }
        finally
        {
            Console.SetOut(originalOutput);
        }

        StringAssert.Contains(output.ToString(), "Game State Loaded: Saved game");
        StringAssert.Contains(output.ToString(), "Player Room: 12");
        StringAssert.Contains(output.ToString(), "Inventory Count: 2");
        StringAssert.Contains(output.ToString(), "Visited Rooms: 2");
    }

    [TestMethod]
    public void LoadThrowsWhenJsonDeserializesToNull()
    {
        using TemporarySaveFile saveFile = new("null");
        GameState gameState = new();

        InvalidOperationException exception = Assert.ThrowsException<InvalidOperationException>(
            () => gameState.Load(saveFile.DirectoryPath, saveFile.FileName));

        Assert.AreEqual(
            $"Unable to load a valid game state from '{Path.Combine(saveFile.DirectoryPath, saveFile.FileName)}'.",
            exception.Message);
    }

    [TestMethod]
    public void LoadReplacesNullGameDataAndPlayerDataWithDefaults()
    {
        using TemporarySaveFile saveFile = new("""{"GameData":null,"PlayerData":null}""");
        GameState gameState = new();
        gameState.GameData.Name = "Old game";
        gameState.PlayerData.ItemInventory.Add(99);

        gameState.Load(saveFile.DirectoryPath, saveFile.FileName);

        Assert.IsNotNull(gameState.GameData);
        Assert.AreEqual(string.Empty, gameState.GameData.Name);
        Assert.IsNotNull(gameState.PlayerData);
        Assert.AreEqual(0, gameState.PlayerData.ItemInventory.Count);
    }

    private static GameState CreateSavedState()
    {
        GameState gameState = new();
        gameState.GameData.Name = "Saved game";
        gameState.GameData.Description = "Saved description";
        gameState.PlayerData.ItemInventory.AddRange([4, 8]);
        gameState.PlayerData.VisitedRoomIds.AddRange([12, 15]);
        gameState.PlayerData.CurrentRoomId = 12;
        return gameState;
    }

    private sealed class TemporarySaveFile : IDisposable
    {
        public string DirectoryPath { get; } = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        public string FileName { get; } = "save.json";

        public TemporarySaveFile(string contents)
        {
            Directory.CreateDirectory(DirectoryPath);
            File.WriteAllText(Path.Combine(DirectoryPath, FileName), contents);
        }

        public void Dispose()
        {
            Directory.Delete(DirectoryPath, recursive: true);
        }
    }
}
