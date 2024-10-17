using GameEngine.GameData;
using GameEngine.Utilities;

namespace GameEngine_Tests.Utilities;

[TestClass]
public class ParserTests
{
    [TestMethod]
    public void GetWordsEmptyStringInput()
    {
        GameDatabase gameDb = new GameDatabase();
        gameDb.Load(@"test_data/", "test_game_database.gge", true);

        (Word? verb, Word? noun) = Parser.GetWordsFromInput(string.Empty, gameDb.Words);

        Assert.IsNull(verb);
        Assert.IsNull(noun);
    }

    [TestMethod]
    public void GetWordsLook()
    {
        GameDatabase gameDb = new GameDatabase();
        gameDb.Load(@"test_data/", "test_game_database.gge", true);

        (Word? verb, Word? noun) = Parser.GetWordsFromInput("look", gameDb.Words);

        Assert.IsNotNull(verb);
        Assert.IsNull(noun);
        Assert.AreEqual("look", verb.Name);
    }

    [TestMethod]
    public void GetWordsInventory()
    {
        GameDatabase gameDb = new GameDatabase();
        gameDb.Load(@"test_data/", "test_game_database.gge", true);

        (Word? verb, Word? noun) = Parser.GetWordsFromInput("i", gameDb.Words);

        Assert.IsNotNull(verb);
        Assert.IsNotNull(noun);
        Assert.AreEqual("look", verb.Name);
        Assert.AreEqual("inventory", noun.Name);
    }

    [TestMethod]
    public void GetWordsNorth()
    {
        GameDatabase gameDb = new GameDatabase();
        gameDb.Load(@"test_data/", "test_game_database.gge", true);

        (Word? verb, Word? noun) = Parser.GetWordsFromInput("n", gameDb.Words);

        Assert.IsNotNull(verb);
        Assert.IsNotNull(noun);
        Assert.AreEqual("go", verb.Name);
        Assert.AreEqual("north", noun.Name);
    }

    [TestMethod]
    public void GetWordsGetRuby()
    {
        GameDatabase gameDb = new GameDatabase();
        gameDb.Load(@"test_data/", "test_game_database.gge", true);

        (Word? verb, Word? noun) = Parser.GetWordsFromInput("get ruby", gameDb.Words);

        Assert.IsNotNull(verb);
        Assert.IsNotNull(noun);
        Assert.AreEqual("take", verb.Name);
        Assert.AreEqual("ruby", noun.Name);        
    }

    [TestMethod]
    public void GetWordsNotFound()
    {
        GameDatabase gameDb = new GameDatabase();
        gameDb.Load(@"test_data/", "test_game_database.gge", true);

        (Word? verb, Word? noun) = Parser.GetWordsFromInput("hello there", gameDb.Words);

        Assert.IsNull(verb);
        Assert.IsNull(noun);       
    }
}
