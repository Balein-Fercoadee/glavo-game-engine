using System.Collections.ObjectModel;
using GameDatabaseEditor.Utilities;
using GameEngine.GameData;

namespace GameEngine_Tests.GameData;

[TestClass]
public class MessageEditSessionTests
{
    [TestMethod]
    public void DraftIsIsolatedUntilSavedAndReplacesMessageInCollection()
    {
        var original = new Message { Id = 4, Text = "Original" };
        var messages = new ObservableCollection<Message> { original };
        var session = new MessageEditSession(messages, original);

        Assert.AreEqual(4, session.Draft.Id);
        Assert.AreEqual("Original", session.Draft.Text);
        session.Draft.Text = "Edited\nmessage";

        Assert.AreEqual("Original", messages[0].Text);
        Assert.IsTrue(session.Save());
        Assert.AreSame(session.Draft, messages[0]);
        Assert.AreEqual(4, messages[0].Id);
        Assert.AreEqual("Edited\nmessage", messages[0].Text);
    }

    [TestMethod]
    public void SaveFailsIfOriginalWasRemoved()
    {
        var original = new Message();
        var messages = new ObservableCollection<Message> { original };
        var session = new MessageEditSession(messages, original);
        messages.Remove(original);

        Assert.IsFalse(session.Save());
        Assert.AreEqual(0, messages.Count);
    }
}
