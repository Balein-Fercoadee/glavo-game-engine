using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GameEngine.GameData;

/// <summary>
/// The base class for all game objects.
/// This class enables data binding via PropertyChanged event.
/// </summary>
public class BaseGameObject : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public BaseGameObject()
    {
    }

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    /// <summary>
    /// Creates a deep clone of Game Object. This method uses Json Serialize/Deserialize to create the new object.
    /// </summary>
    /// <returns>A deep clone of the Game Object.</returns>
    /// <exception cref="NotImplementedException">If the Game Object does not support cloning, this exception is thrown.</exception>
    public virtual object Clone()
    {
        throw new NotImplementedException();
    }
}
