using System.Text.Json.Serialization;

namespace GameEngine.GameData;

public class Action : IIdentifiable
{
    public int Id { get; set; }

    public int TriggerNounId { get; set; }
    public int TriggerVerbId { get; set; }

    [JsonInclude]
    public List<ActionCommand> Commands;
    [JsonInclude]
    public List<ActionCondition> Conditions;

    public Action()
    {
        TriggerNounId = Constants.WORD_ID_UNSET;
        TriggerVerbId = Constants.WORD_ID_UNSET;

        Commands = new List<ActionCommand>();
        Conditions = new List<ActionCondition>();
    }
}

public class ActionCommand
{
    public ActionCommands Command { get; set; }
    public int ObjectId { get; set; }
}

public class ActionCondition
{
    public ActionConditions Condition { get; set; }
    public int ObjectId { get; set; }
}

public enum ActionConditions
{
    /// <summary>
    /// Player has the item in inventory.
    /// </summary>
    PlayerHasItem = 1,
    /// <summary>
    /// Player is in the room with the item.
    /// </summary>
    PlayerWithItem = 2,
}

public enum ActionCommands
{
    /// <summary>
    /// Display the message.
    /// </summary>
    DisplayMessage = 1,
    /// <summary>
    /// Adds the item into the current room.
    /// </summary>
    PutItemInRoom = 2,
    /// <summary>
    /// Removes the item from the current room.
    /// </summary>
    RemoveItemFromRoom = 3
}
