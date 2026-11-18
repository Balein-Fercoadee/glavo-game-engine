using GameDatabaseEditor.Utilities;
using GameEngine.GameData;

namespace GameEngine_Tests.GameData;

[TestClass]
public class WordValidationTests
{
    [TestMethod]
    public void FindDuplicateTokenFindsExistingNamesAndAliasesIgnoringCase()
    {
        var words = new[]
        {
            new Word { Name = "look", Aliases = ["examine"] },
            new Word { Name = "north" }
        };

        var duplicateName = WordValidation.FindDuplicateToken(words, new Word { Name = "LOOK" });
        var duplicateAlias = WordValidation.FindDuplicateToken(words,
            new Word { Name = "inspect", Aliases = ["ExAmInE"] });

        Assert.AreEqual("LOOK", duplicateName);
        Assert.AreEqual("ExAmInE", duplicateAlias);
    }

    [TestMethod]
    public void FindDuplicateTokenReturnsNullWhenAllTokensAreUnique()
    {
        var words = new[] { new Word { Name = "look", Aliases = ["examine"] } };

        var duplicate = WordValidation.FindDuplicateToken(words,
            new Word { Name = "inspect", Aliases = ["check"] });

        Assert.IsNull(duplicate);
    }
}
