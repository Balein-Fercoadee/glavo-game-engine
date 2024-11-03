
namespace GameEngine.GameData;

public class Room : IIdentifiable
{
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the <c>Item.Id</c> of <c>Items</c> located in the <c>Room</c>.
    /// </summary>
    public List<int> ContainedItemIds { get; set; }

    /// <summary>
    /// Gets or sets the descriptionf of the <c>Room</c>.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the <c>Room.Id</c> that the down exit leads to.
    /// </summary>
    public int ExitIdDown { get; set; }

    /// <summary>
    /// Gets or sets the <c>Room.Id</c> that east exit leads to.
    /// </summary>
    public int ExitIdEast { get; set; }

    /// <summary>
    /// Gets or sets the <c>Room.Id</c> that north exit leads to.
    /// </summary>
    public int ExitIdNorth { get; set; }

    /// <summary>
    /// Gets or sets the <c>Room.Id</c> that south exit leads to.
    /// </summary>
    public int ExitIdSouth { get; set; }

    /// <summary>
    /// Gets or sets the <c>Room.Id</c> that the up exit leads to.
    /// </summary>
    public int ExitIdUp { get; set; }

    /// <summary>
    /// Gets or sets the <c>Room.Id</c> that west exit leads to.
    /// </summary>
    public int ExitIdWest { get; set; }

    /// <summary>
    /// Gets or sets the name of the <c>Room</c>.
    /// </summary>
    public string Name { get; set; }

    public Room()
    {
        Description = string.Empty;
        ExitIdEast = Constants.ROOM_ID_UNSET;
        ExitIdNorth = Constants.ROOM_ID_UNSET;
        ExitIdSouth = Constants.ROOM_ID_UNSET;
        ExitIdWest = Constants.ROOM_ID_UNSET;
        ExitIdUp = Constants.ROOM_ID_UNSET;
        ExitIdDown = Constants.ROOM_ID_UNSET;
        Id = Constants.ROOM_ID_UNSET;
        Name = string.Empty;
        ContainedItemIds = new List<int>();
    }

    /// <summary>
    /// Gets a list of string that represent the Room's available exits.
    /// </summary>
    /// <returns></returns>
    public List<string> AvailableExits()
    {
        List<string> exits = new List<string>();

        if (!(ExitIdEast == Constants.ROOM_ID_UNSET))
            exits.Add("e");
        if (!(ExitIdNorth == Constants.ROOM_ID_UNSET))
            exits.Add("n");
        if (!(ExitIdSouth == Constants.ROOM_ID_UNSET))
            exits.Add("s");
        if (!(ExitIdWest == Constants.ROOM_ID_UNSET))
            exits.Add("w");
        if (!(ExitIdUp == Constants.ROOM_ID_UNSET))
            exits.Add("u");
        if (!(ExitIdDown == Constants.ROOM_ID_UNSET))
            exits.Add("d");

        return exits;
    }
}