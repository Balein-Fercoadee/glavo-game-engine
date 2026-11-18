using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using GameEngine.GameData;

namespace GameDatabaseEditor.Utilities;

internal sealed class DatabaseValidation : IDisposable
{
    private readonly GameDatabase _database;

    public ObservableCollection<string> Errors { get; } = new();

    public DatabaseValidation(GameDatabase database)
    {
        _database = database;
        _database.PropertyChanged += Database_PropertyChanged;
        _database.Rooms.CollectionChanged += Collection_Changed;
        _database.Items.CollectionChanged += Collection_Changed;
        Refresh();
    }

    private void Database_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName is nameof(GameDatabase.StartingRoomId) or nameof(GameDatabase.TreasureRoomId))
        {
            Refresh();
        }
    }

    private void Collection_Changed(object? sender, NotifyCollectionChangedEventArgs e)
    {
        Refresh();
    }

    private void Refresh()
    {
        Errors.Clear();
        if (_database.Rooms.Count == 0)
        {
            Errors.Add("Add at least one Room.");
        }

        if (_database.StartingRoomId < 0 || !_database.Rooms.Any(room => room.Id == _database.StartingRoomId))
        {
            Errors.Add("Set the starting Room to an existing Room.");
        }

        if (_database.TreasureRoomId < 0 || !_database.Rooms.Any(room => room.Id == _database.TreasureRoomId))
        {
            Errors.Add("Set the treasure Room to an existing Room.");
        }

        if (!_database.Items.Any(item => item.IsTreasure))
        {
            Errors.Add("Add at least one Item marked as treasure.");
        }
    }

    public void Dispose()
    {
        _database.PropertyChanged -= Database_PropertyChanged;
        _database.Rooms.CollectionChanged -= Collection_Changed;
        _database.Items.CollectionChanged -= Collection_Changed;
    }
}
