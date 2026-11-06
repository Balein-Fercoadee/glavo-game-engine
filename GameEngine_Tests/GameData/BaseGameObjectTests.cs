using System.ComponentModel;
using GameEngine.GameData;

namespace GameEngine_Tests.GameData;

[TestClass]
public class BaseGameObjectTests
{
    [TestMethod]
    public void OnPropertyChangedRaisesPropertyChangedWithCallerMemberName()
    {
        TestGameObject gameObject = new();
        PropertyChangedEventArgs? eventArgs = null;
        object? eventSender = null;
        gameObject.PropertyChanged += (sender, args) =>
        {
            eventSender = sender;
            eventArgs = args;
        };

        gameObject.Name = "Updated";

        Assert.AreSame(gameObject, eventSender);
        Assert.IsNotNull(eventArgs);
        Assert.AreEqual(nameof(TestGameObject.Name), eventArgs.PropertyName);
    }

    [TestMethod]
    public void OnPropertyChangedWithoutSubscribersDoesNotThrow()
    {
        TestGameObject gameObject = new();

        gameObject.RaisePropertyChanged("Name");
    }

    [TestMethod]
    public void CloneThrowsWhenNotImplementedByDerivedType()
    {
        TestGameObject gameObject = new();

        Assert.ThrowsException<NotImplementedException>(() => gameObject.Clone());
    }

    private sealed class TestGameObject : BaseGameObject
    {
        private string _name = string.Empty;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public void RaisePropertyChanged(string propertyName)
        {
            OnPropertyChanged(propertyName);
        }
    }
}
