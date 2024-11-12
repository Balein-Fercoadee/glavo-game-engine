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
}
