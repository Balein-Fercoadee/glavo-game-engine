using GameEngine;

namespace GameEngine_Tests;

[TestClass]
public class EngineSetupTests
{
    /// <summary>
    /// Test Setup when an empty args array is passed.
    /// </summary>
    [TestMethod]
    public void ArgsEmpty()
    {
        string[] args = new string[0];

        var result = EngineSetup.StartSetup(args);

        Assert.IsTrue(!result.SetupSuccessful);
        Assert.IsTrue(result.ErrorMessages.Count == 1);
        Assert.IsTrue(result.ErrorMessages[0].Equals("<game_db_name> is required!"));
    }

    /// <summary>
    /// Test when a good path to a game file is passed.
    /// </summary>
    [TestMethod]
    public void ArgsWithGoodGameFileLocation()
    {
        string gameLocation = @"test_data/fake_game.gge";
        string[] args = new string[1];
        args[0] = gameLocation;

        var result = EngineSetup.StartSetup(args);
        Assert.IsTrue(result.SetupSuccessful);
        Assert.AreEqual(gameLocation, result.GameDatabaseFilePath);
        Assert.IsTrue(result.ErrorMessages.Count == 0);
    }

    /// <summary>
    /// Test when a bad path to a game file is passed
    /// </summary>
    [TestMethod]
    public void ArgsWithBadGameFileLocation()
    {
        string gameLocation = @"test_data/bad_fake_game.gge";
        string[] args = new string[1];
        args[0] = gameLocation;

        var result = EngineSetup.StartSetup(args);
        Assert.IsTrue(!result.SetupSuccessful);
        Assert.IsTrue(result.ErrorMessages.Count == 1);

    }

    [TestMethod]
    public void ArgsWithGoodSaveFileLocation()
    {
        string gameLocation = @"test_data/fake_game.gge";
        string saveLocation = @"test_data/fake_save.gges";
        string[] args = new string[2];
        args[0] = gameLocation;
        args[1] = saveLocation;

        var result = EngineSetup.StartSetup(args);
        Assert.IsTrue(result.SetupSuccessful);
        Assert.AreEqual(gameLocation, result.GameDatabaseFilePath);
        Assert.AreEqual(saveLocation, result.SaveFilePath);
        Assert.IsTrue(result.ErrorMessages.Count == 0);
    }

    [TestMethod]
    public void ArgsWithBadSaveFileLocation()
    {
        string gameLocation = @"test_data/fake_game.gge";
        string saveLocation = @"test_data/bad_save.gges";
        string[] args = new string[2];
        args[0] = gameLocation;
        args[1] = saveLocation;

        var result = EngineSetup.StartSetup(args);
        Assert.IsTrue(!result.SetupSuccessful);
        Assert.IsTrue(result.ErrorMessages.Count == 1);
    }

    [TestMethod]
    public void ArgsWithDebugFlag()
    {
        string gameLocation = @"test_data/fake_game.gge";
        string saveLocation = @"test_data/fake_save.gges";
        string[] args = new string[3];
        args[0] = gameLocation;
        args[1] = saveLocation;
        args[2] = "-d";

        var result = EngineSetup.StartSetup(args);
        Assert.IsTrue(result.SetupSuccessful);
        Assert.AreEqual(gameLocation, result.GameDatabaseFilePath);
        Assert.AreEqual(saveLocation, result.SaveFilePath);
        Assert.IsTrue(result.OutputDebugStatements);
        Assert.IsTrue(result.ErrorMessages.Count == 0);
    }

    [TestMethod]
    public void ArgsWithBadFlag()
    {
        string gameLocation = @"test_data/fake_game.gge";
        string saveLocation = @"test_data/fake_save.gges";
        string[] args = new string[3];
        args[0] = gameLocation;
        args[1] = saveLocation;
        args[2] = "-!";

        var result = EngineSetup.StartSetup(args);
        Assert.IsTrue(!result.SetupSuccessful);
        Assert.IsTrue(result.ErrorMessages.Count == 1);
    }
}