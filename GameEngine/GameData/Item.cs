namespace GameEngine.GameData;

public class Item : IIdentifiable
{
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets whether the <c>Item</c> is a treasure.
    /// <para>
    /// <remarks>
    /// Treasure <c>Items</c> are meant to be collect and place in a <c>Room</c> to score or complete the game.
    /// </remarks>
    /// </para>
    /// </summary>
    public bool IsTreasure { get; set; }

    /// <summary>
    /// Gets or sets the description of the <c>Item</c>.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the name of the <c>Item</c>.
    /// </summary>
    public string Name { get; set; }

    public Item()
    {
        Description = string.Empty;
        IsTreasure = false;
        Name = string.Empty;
    }
}