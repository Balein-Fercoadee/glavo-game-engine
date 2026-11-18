using System.Collections.ObjectModel;
using GameDatabaseEditor.Utilities;
using GameEngine.GameData;

namespace GameEngine_Tests.GameData;

[TestClass]
public class WordEditSessionTests
{
    [TestMethod]
    public void DraftIsIsolatedUntilSavedAndCopiesAliases()
    {
        var original = new Word
        {
            Id = 16,
            Name = "open",
            WordType = WordTypes.Verb,
            Aliases = ["unlock"]
        };
        var words = new ObservableCollection<Word> { original };
        var session = new WordEditSession(words, original);

        session.Draft.Name = "close";
        session.Draft.WordType = WordTypes.Noun;
        session.Draft.Aliases.Add("shut");

        Assert.AreEqual("open", original.Name);
        CollectionAssert.AreEqual(new[] { "unlock" }, original.Aliases);
        Assert.IsTrue(session.Save());
        Assert.AreSame(session.Draft, words[0]);
        Assert.AreEqual("close", words[0].Name);
        Assert.AreEqual(WordTypes.Noun, words[0].WordType);
        CollectionAssert.AreEqual(new[] { "unlock", "shut" }, words[0].Aliases);
    }

    [TestMethod]
    public void SaveFailsIfOriginalWasRemoved()
    {
        var original = new Word();
        var words = new ObservableCollection<Word> { original };
        var session = new WordEditSession(words, original);
        words.Remove(original);

        Assert.IsFalse(session.Save());
        Assert.AreEqual(0, words.Count);
    }
}
