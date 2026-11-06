using System;
using GameEngine.Utilities;
using Microsoft.VisualStudio.TestPlatform.Common.Exceptions;

namespace GameEngine_Tests.Utilities;

[TestClass]
public class GeneralUtilitiesTests
{
    [TestMethod]
    public void EmptyListOfIds()
    {
        List<int> idList = new List<int>();
        int actualId = -1;

        actualId = GeneralUtilities.GetNextIdNumber(idList);

        Assert.AreEqual(0, actualId);
    }

    [TestMethod]
    public void FullListOfIds()
    {
        List<int> idList = [0, 1, 2, 3, 4];
        int actualId = -1;

        actualId = GeneralUtilities.GetNextIdNumber(idList);

        Assert.AreEqual(5, actualId);
    }

    [TestMethod]
    public void PartialListOfIds()
    {
        List<int> idList = [0, 1, 3, 4];
        int actualId = -1;

        actualId = GeneralUtilities.GetNextIdNumber(idList);

        Assert.AreEqual(2, actualId);
    }
}
