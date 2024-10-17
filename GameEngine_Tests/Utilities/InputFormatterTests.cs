using System;
using GameEngine.Utilities;

namespace GameEngine_Tests.Utilities;

[TestClass]
public class InputFormatterTests
{
    [TestMethod]
    public void CleanInput()
    {
        string testInput = "This is a Test String ";
        string expectedOutput = "this is a test string";

        string actualOutput = InputFormatter.CleanPlayerInput(testInput);

        Assert.AreEqual(expectedOutput, actualOutput);
    }

    [TestMethod]
    public void NullInput()
    {
        string? testInput = null;
        string expectedOutput = string.Empty;

#pragma warning disable CS8604 // Possible null reference argument.
        string output = InputFormatter.CleanPlayerInput(testInput);
#pragma warning restore CS8604 // Possible null reference argument.

        Assert.AreEqual(expectedOutput, output);
    }
}
