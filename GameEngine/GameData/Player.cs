using System.Text.Json.Serialization;

namespace GameEngine.GameData;

public class Player
{
    int _currentRoomId;
    /// <summary>
    /// Gets or sets the <c>Room.Id</c> where the Player currently is.
    /// </summary>
    public int CurrentRoomId
    {
        get { return _currentRoomId; }
        set
        {
            if (value != Constants.ROOM_ID_UNSET)
            {
                AddVisitedRoom(value);
            }
            _currentRoomId = value;
        }
    }

    /// <summary>
    /// Gets a collection of <c>Item.Id</c> of what the <c>Player</c> is carrying.
    /// </summary>
    [JsonInclude]
    public List<int> ItemInventory { get; set; }

    /// <summary>
    /// Gets a list of <c>Room.Ids</c> where the <c>Player</c> has already visited.
    /// </summary>
    [JsonInclude]
    public List<int> VisitedRoomIds { get; set; }

    public Player()
    {
        CurrentRoomId = Constants.ROOM_ID_UNSET;
        ItemInventory = new List<int>();
        VisitedRoomIds = new List<int>();
    }

    private void AddVisitedRoom(int visitedRoomId)
    {
        if (!HasAlreadyVisitedRoom(visitedRoomId))
        {
            VisitedRoomIds.Add(visitedRoomId);
        }
    }

    public bool HasAlreadyVisitedRoom(int roomId)
    {
        bool hasVisited = VisitedRoomIds.Contains(roomId);

        return hasVisited;
    }
}
