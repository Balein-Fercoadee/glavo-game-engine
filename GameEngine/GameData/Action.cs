using System.Text.Json.Serialization;

namespace GameEngine.GameData;

/// <summary>
/// The <c>Action</c> class is used to define interactions beyond the built-in words.
/// </summary>
public class Action : IIdentifiable
{
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the description of the Action.
    /// <para>The description is purely for documentation purposes. It won't be displayed in-game.</para>
    /// </summary>
    public string Description { get; set; }

    public int TriggerNounId { get; set; }
    public int TriggerVerbId { get; set; }

    /// <summary>
    /// Gets a collection of <c>ActionCommands</c>.
    /// </summary>
    [JsonInclude]
    public List<ActionCommand> Commands { get; }
    /// <summary>
    /// Gets a collection of <c>ActionConditions</c>.
    /// </summary>
    [JsonInclude]
    public List<ActionCondition> Conditions { get; }

    public Action()
    {
        TriggerNounId = Constants.WORD_ID_UNSET;
        TriggerVerbId = Constants.WORD_ID_UNSET;

        Commands = new List<ActionCommand>();
        Conditions = new List<ActionCondition>();
        Description = string.Empty;
    }
}

public class ActionCommand
{
    public ActionCommands Command { get; set; }
    public int ObjectId { get; set; }
}

/// <summary>
/// <c>ActionCondition</c> defines a condition, when true, allows the <c>Action</c> to execute its <c>ActionCommands</c>.
/// <para>Conditions are defined in the <c>ActionConditions</c> enum.</para>
/// </summary>
public class ActionCondition
{
    public ActionConditions Condition { get; set; }
    public int ObjectId { get; set; }
}

/// <summary>
/// Enumeration of available conditions to use within an <c>Action</c>.
/// </summary>
public enum ActionConditions
{
    /// <summary>
    /// Player has the item, defined in <c>ActionCondition.ObjectId</c>, in inventory.
    /// </summary>
    PlayerHasItem = 1,
    /// <summary>
    /// Player and item, defined in <c>ActionCondition.ObjectId</c>, are in the same room.
    /// </summary>
    PlayerWithItem = 2,
}

/// <summary>
/// Enumeration of available commands to use within an <c>Action</c>.
/// </summary>
public enum ActionCommands
{
    /// <summary>
    /// Display the message defined in <c>ActionCommand.ObjectId</c>.
    /// </summary>
    DisplayMessage = 1,
    /// <summary>
    /// Adds the item, defined in <c>ActionCommand.ObjectId</c>, into the current room.
    /// </summary>
    PutItemInRoom = 2,
    /// <summary>
    /// Removes the item, defined in <c>ActionCommand.ObjectId</c>, from the current room.
    /// </summary>
    RemoveItemFromRoom = 3,
    /// <summary>
    /// Changes the CurrentRoom to the <c>ActionCommand.ObjectId</c>.
    /// </summary>
    MoveToRoom = 4
}
