using System.Collections.ObjectModel;
using GameEngine.GameData;

namespace GameDatabaseEditor.Utilities;

internal sealed class RoomEditSession
{
    private readonly Room _room;

    public Room Draft { get; }

    public RoomEditSession(Room room)
    {
        _room = room;
        Draft = new Room
        {
            Id = room.Id,
            Name = room.Name,
            Description = room.Description,
            ContainedItemIds = new ObservableCollection<int>(room.ContainedItemIds),
            ExitIdNorth = room.ExitIdNorth,
            ExitIdSouth = room.ExitIdSouth,
            ExitIdEast = room.ExitIdEast,
            ExitIdWest = room.ExitIdWest,
            ExitIdUp = room.ExitIdUp,
            ExitIdDown = room.ExitIdDown
        };
    }

    public void Save()
    {
        _room.Name = Draft.Name;
        _room.Description = Draft.Description;
        _room.ExitIdNorth = Draft.ExitIdNorth;
        _room.ExitIdSouth = Draft.ExitIdSouth;
        _room.ExitIdEast = Draft.ExitIdEast;
        _room.ExitIdWest = Draft.ExitIdWest;
        _room.ExitIdUp = Draft.ExitIdUp;
        _room.ExitIdDown = Draft.ExitIdDown;

        if (!_room.ContainedItemIds.SequenceEqual(Draft.ContainedItemIds))
        {
            _room.ContainedItemIds.Clear();
            foreach (var itemId in Draft.ContainedItemIds)
            {
                _room.ContainedItemIds.Add(itemId);
            }
        }
    }
}
